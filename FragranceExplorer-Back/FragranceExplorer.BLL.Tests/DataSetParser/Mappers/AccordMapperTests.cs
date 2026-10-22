using FragranceExplorer.BLL.DataSetParser.Mappers;
using FragranceExplorer.BLL.DataSetParser.Models;
using FluentAssertions;
using Xunit;

namespace FragranceExplorer.BLL.Tests.DataSetParser.Mappers;

public class AccordMapperTests
{
    private readonly AccordMapper _mapper = new();

    [Fact]
    public void Map_WhenDtoIsValid_ShouldReturnPerfumeAccordWithNormalizedIntensity()
    {
        var dto = new RawAccordDto
        {
            Name = "woody",
            Strength = 80
        };

        var result = _mapper.Map(dto);

        result.Should().NotBeNull();
        result!.Name.Should().Be("woody");
        result.Intensity.Should().BeApproximately(0.8, precision: 1e-5);
    }

    [Fact]
    public void Map_WhenNameHasWhitespace_ShouldTrimName()
    {
        var dto = new RawAccordDto
        {
            Name = "   citrus   ",
            Strength = 50
        };

        var result = _mapper.Map(dto);

        result.Should().NotBeNull();
        result!.Name.Should().Be("citrus");
        result.Intensity.Should().BeApproximately(0.5, precision: 1e-5);
    }

    [Fact]
    public void Map_WhenDtoIsNull_ShouldReturnNull()
    {
        var result = _mapper.Map(null);

        result.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Map_WhenNameIsNullOrWhitespace_ShouldReturnNull(string? invalidName)
    {
        var dto = new RawAccordDto
        {
            Name = invalidName!,
            Strength = 70
        };

        var result = _mapper.Map(dto);

        result.Should().BeNull();
    }

    [Fact]
    public void Map_WhenStrengthExceeds100_ShouldClampIntensityToOne()
    {
        var dto = new RawAccordDto
        {
            Name = "vanilla",
            Strength = 150
        };

        var result = _mapper.Map(dto);

        result.Should().NotBeNull();
        result!.Intensity.Should().Be(1.0);
    }

    [Fact]
    public void Map_WhenStrengthIsNegative_ShouldClampIntensityToZero()
    {
        var dto = new RawAccordDto
        {
            Name = "floral",
            Strength = -25
        };

        var result = _mapper.Map(dto);

        result.Should().NotBeNull();
        result!.Intensity.Should().Be(0.0);
    }

    [Fact]
    public void MapMany_WhenListIsNull_ShouldReturnEmptySequence()
    {
        var result = _mapper.MapMany(null);

        result.Should().BeEmpty();
    }

    [Fact]
    public void MapMany_WhenListContainsValidDtos_ShouldReturnAllMappedAccords()
    {
        var dtos = new List<RawAccordDto>
        {
            new() { Name = "woody", Strength = 80 },
            new() { Name = "amber", Strength = 60 }
        };

        var result = _mapper.MapMany(dtos).ToList();

        result.Should().HaveCount(2);
        result[0].Name.Should().Be("woody");
        result[0].Intensity.Should().BeApproximately(0.8, precision: 1e-5);
        result[1].Name.Should().Be("amber");
        result[1].Intensity.Should().BeApproximately(0.6, precision: 1e-5);
    }

    [Fact]
    public void MapMany_WhenListContainsInvalidOrNullDtos_ShouldSkipInvalidAndReturnOnlyValid()
    {
        var dtos = new List<RawAccordDto?>
        {
            new() { Name = "woody", Strength = 80 },
            null,
            new() { Name = "   ", Strength = 40 },
            new() { Name = "spicy", Strength = 50 }
        };

        var result = _mapper.MapMany(dtos!).ToList();

        result.Should().HaveCount(2);
        result.Select(a => a.Name).Should().Contain(new[] { "woody", "spicy" });
    }
}
