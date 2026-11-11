using FluentAssertions;
using FragranceExplorer.BLL.Parser.Houses.Models;
using FragranceExplorer.BLL.Parser.Houses.Validators;
using Xunit;

namespace FragranceExplorer.BLL.Tests.Parser.Houses.Validators;

public class HouseDtoValidatorTests
{
    private readonly HouseDtoValidator _validator = new();

    private static RawHouseDto CreateValidDto() => new()
    {
        Id = "zara",
        Name = "ZARA",
        DisplayName = "Zara",
        ArchiveCount = 1183,
        Rating = 4.05
    };

    [Fact]
    public void Validate_WhenDtoIsValid_ShouldReturnTrueAndNoErrors()
    {
        var dto = CreateValidDto();

        var isValid = _validator.Validate(dto, out var errors);

        isValid.Should().BeTrue();
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WhenDtoIsNull_ShouldReturnFalse()
    {
        var isValid = _validator.Validate(null, out var errors);

        isValid.Should().BeFalse();
        errors.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WhenIdIsEmptyOrWhitespace_ShouldReturnFalse(string invalidId)
    {
        var dto = CreateValidDto();
        dto.Id = invalidId;

        var isValid = _validator.Validate(dto, out var errors);

        isValid.Should().BeFalse();
        errors.Should().Contain(e => e.Contains("ID"));
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(5.5)]
    public void Validate_WhenRatingIsOutOfRange_ShouldReturnFalse(double invalidRating)
    {
        var dto = CreateValidDto();
        dto.Rating = invalidRating;

        var isValid = _validator.Validate(dto, out var errors);

        isValid.Should().BeFalse();
        errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Validate_WhenArchiveCountIsNegative_ShouldReturnFalse()
    {
        var dto = CreateValidDto();
        dto.ArchiveCount = -5;

        var isValid = _validator.Validate(dto, out var errors);

        isValid.Should().BeFalse();
        errors.Should().NotBeEmpty();
    }
}

