using FragranceExplorer.BLL.Enums;
using FragranceExplorer.BLL.Models;
using FragranceExplorer.BLL.Strategies;
using FluentAssertions;
using Xunit;

namespace FragranceExplorer.BLL.Tests.Strategies;

public class AccordCosineSimilarityStrategyTests
{
    [Fact]
    public void CalculateSimilarity_WhenPerfumesHaveIdenticalAccords_ShouldReturnOne()
    {
        // 1. Arrange
        var strategy = new AccordCosineSimilarityStrategy();

        var perfume1 = new Perfume(1, "Perfume A", "Brand A", GenderCategory.Unisex, 4.5);
        perfume1.AddAccord(new PerfumeAccord("woody", 0.3));
        perfume1.AddAccord(new PerfumeAccord("citrus", 0.4));

        var perfume2 = new Perfume(2, "Perfume B", "Brand B", GenderCategory.Unisex, 4.0);
        perfume2.AddAccord(new PerfumeAccord("woody", 0.3));
        perfume2.AddAccord(new PerfumeAccord("citrus", 0.4));

        // 2. Act
        double actualSimilarity = strategy.CalculateSimilarity(perfume1, perfume2);

        // 3. Assert
        actualSimilarity.Should().BeApproximately(1.0, precision: 1e-5);
    }

    [Fact]
    public void CalculateSimilarity_WhenPerfumesHaveNoCommonAccords_ShouldReturnZero()
    {
        // 1. Arrange
        var strategy = new AccordCosineSimilarityStrategy();

        var perfume1 = new Perfume(1, "Perfume A", "Brand A", GenderCategory.Unisex, 4.5);
        perfume1.AddAccord(new PerfumeAccord("woody", 0.3));
        perfume1.AddAccord(new PerfumeAccord("citrus", 0.4));

        var perfume2 = new Perfume(2, "Perfume B", "Brand B", GenderCategory.Unisex, 4.0);
        perfume2.AddAccord(new PerfumeAccord("floral", 0.3));
        perfume2.AddAccord(new PerfumeAccord("fruity", 0.4));

        // 2. Act
        double actualSimilarity = strategy.CalculateSimilarity(perfume1, perfume2);

        // 3. Assert
        actualSimilarity.Should().BeApproximately(0.0, precision: 1e-5);
    }

    [Fact]
    public void CalculateSimilarity_WhenPerfumesHaveParcialCommonAccords_ShouldReturnCorrectResult()
    {
        // 1. Arrange
        var strategy = new AccordCosineSimilarityStrategy();

        var perfume1 = new Perfume(1, "Perfume A", "Brand A", GenderCategory.Unisex, 4.5);
        perfume1.AddAccord(new PerfumeAccord("woody", 0.3));
        perfume1.AddAccord(new PerfumeAccord("citrus", 0.4));

        var perfume2 = new Perfume(2, "Perfume B", "Brand B", GenderCategory.Unisex, 4.0);
        perfume2.AddAccord(new PerfumeAccord("woody", 0.5));
        perfume2.AddAccord(new PerfumeAccord("vanilla", 0.3));

        // 2. Act
        double actualSimilarity = strategy.CalculateSimilarity(perfume1, perfume2);

        // 3. Assert 
        // DotProduct = 0.3 * 0.5 = 0.15 (similar only for "woody")
        // Mag(perfume1) = sqrt(0.3^2 + 0.4^2) = sqrt(0.25) = 0.5
        // Mag(perfume2) = sqrt(0.5^2 + 0.3^2) = sqrt(0.34) = 0.583095
        // Expected = 0.15 / (0.5 * sqrt(0.34)) = 0.514496
        actualSimilarity.Should().BeApproximately(0.514496, precision: 1e-5);
    }

    
}
