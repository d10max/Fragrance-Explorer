using FluentAssertions;
using FragranceExplorer.BLL.Parser.Houses.Models;
using FragranceExplorer.BLL.Parser.Houses.Validators;
using Xunit;

namespace FragranceExplorer.BLL.Tests.Parser.Houses.Validators;

public class OlfactoryDnaDtoValidatorTests
{
    private readonly OlfactoryDnaDtoValidator _validator = new();

    [Fact]
    public void Validate_WhenDtoIsValid_ShouldReturnTrue()
    {
        var dto = new RawOlfactoryDnaDto
        {
            Family = "Fresh",
            Notes = "Rose · Jasmine · Lavender",
            Percentage = 78
        };

        var isValid = _validator.Validate(dto, out var errors);

        isValid.Should().BeTrue();
        errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void Validate_WhenPercentageIsOutOfRange_ShouldReturnFalse(int invalidPercentage)
    {
        var dto = new RawOlfactoryDnaDto
        {
            Family = "Fresh",
            Notes = "Rose",
            Percentage = invalidPercentage
        };

        var isValid = _validator.Validate(dto, out var errors);

        isValid.Should().BeFalse();
        errors.Should().NotBeEmpty();
    }

    [Fact]
    public void Validate_WhenFamilyOrNotesAreEmpty_ShouldReturnFalse()
    {
        var dto = new RawOlfactoryDnaDto
        {
            Family = "",
            Notes = "   ",
            Percentage = 50
        };

        var isValid = _validator.Validate(dto, out var errors);

        isValid.Should().BeFalse();
        errors.Should().HaveCount(2);
    }
}

