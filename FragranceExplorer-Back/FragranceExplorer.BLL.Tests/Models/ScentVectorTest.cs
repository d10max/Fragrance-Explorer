using FragranceExplorer.BLL.Models;
using Xunit;
using FluentAssertions;

namespace FragranceExplorer.BLL.Tests.Models;

public class ScentVectorTest
{
    [Fact]
    public void Magnitude_WhenVectorHasComponents_ShouldReturnCorrectResult() 
    {
        // Arrange
        var vector = new ScentVector<PerfumeAccord> ();
        vector.AddSignificance(new PerfumeAccord("woody", 0.3));
        vector.AddSignificance(new PerfumeAccord("citrus", 0.4));
        // expect sqrt(0.3^2 + 0.4^2) = sqrt(0.09 + 0.16) = 0.5
        
        // Act
        double actualMagnitude = vector.Magnitude();
        // Assert
        actualMagnitude.Should().BeApproximately(0.5, precision: 1e-5);
    }

    [Fact]
    public void Magnitude_WhenVectorHasNoComponents_ShouldReturnZero()
    {
        // Arrange
        var vector = new ScentVector<PerfumeAccord> ();
        // Act
        double actualMagnitude = vector.Magnitude();
        // Assert
        actualMagnitude.Should().Be(0.0);
    }

    [Fact]
    public void DotProduct_WhenVectorsHaveCommonComponents_ShouldReturnCorrectResult()
    {
        var vector1 = new ScentVector<PerfumeAccord>();
        var vector2 = new ScentVector<PerfumeAccord>();

        vector1.AddSignificance(new PerfumeAccord("woody", 0.5));
        vector1.AddSignificance(new PerfumeAccord("vanilla", 0.3));

        vector2.AddSignificance(new PerfumeAccord("woody", 0.4));
        vector2.AddSignificance(new PerfumeAccord("citrus", 0.8));
        
        double actualDotProduct = vector1.DotProduct(vector2); // expect (0.5 * 0.4) = 0.2
        
        actualDotProduct.Should().BeApproximately(0.2, precision: 1e-5);
    }

    [Fact]
    public void DotProduct_WhenVectorsHaveNoCommonComponents_ShouldReturnZero()
    {
        // Arrange
        var vector1 = new ScentVector<PerfumeAccord>();
        var vector2 = new ScentVector<PerfumeAccord>();
        
        vector1.AddSignificance(new PerfumeAccord("woody", 0.5));
        vector1.AddSignificance(new PerfumeAccord("vanilla", 0.3));

        vector2.AddSignificance(new PerfumeAccord("citrus", 0.8));
        vector2.AddSignificance(new PerfumeAccord("floral", 0.9));
        
        // Act
        double actualDotProduct = vector1.DotProduct(vector2);
        
        // Assert
        actualDotProduct.Should().BeApproximately(0.0, precision: 1e-5);
    }

    [Fact]
    public void DotProduct_WhenVectorIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var vector = new ScentVector<PerfumeAccord>();
        
        // Act
        Action act = () => vector.DotProduct(null!);
        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}