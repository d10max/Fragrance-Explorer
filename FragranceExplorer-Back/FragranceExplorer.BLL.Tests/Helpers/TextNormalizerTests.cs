using FluentAssertions;
using FragranceExplorer.BLL.Helpers;
using Xunit;

namespace FragranceExplorer.BLL.Tests.Helpers;

public class TextNormalizerTests
{
    [Fact]
    public void NormalizeText_ShouldReturnNormalizedText()
    {
        // Arrange
        string input = "  Acqua di Giò  ";
        string expectedOutput = "acqua di gio";

        // Act
        string actualOutput = TextNormalizer.Normalize(input);

        // Assert
        actualOutput.Should().Be(expectedOutput);
    }

    [Fact]
    public void NormalizeText_ShouldHandleSpecialCharacters()
    {
        // Arrange
        string input = "L'Interdit -  Givenchy";
        string expectedOutput = "l interdit givenchy";

        // Act
        string actualOutput = TextNormalizer.Normalize(input);

        // Assert
        actualOutput.Should().Be(expectedOutput);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("---'///")]
    public void NormalizeText_WhenInputIsEmptyOrOnlySymbols_ShouldReturnEmptyString(string? input)
    {
        // Act
        string actualOutput = TextNormalizer.Normalize(input);

        // Assert
        actualOutput.Should().BeEmpty();
    }
}
