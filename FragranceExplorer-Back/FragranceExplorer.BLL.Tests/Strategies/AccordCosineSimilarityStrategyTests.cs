using FragranceExplorer.BLL.Strategies;
using FluentAssertions;
using FragranceExplorer.BLL.Models.MathModels;
using FragranceExplorer.DAL.Entities.MathDataEntities;

namespace FragranceExplorer.BLL.Tests.Strategies;

public class AccordCosineSimilarityStrategyTests
{
    [Fact]
    public void CalculateSimilarity_WhenPerfumesHaveIdenticalAccords_ShouldReturnOne()
    {
        // 1. Arrange
        var strategy = new AccordCosineSimilarityStrategy();

        var accords1 = new List<AccordWeightData>([new AccordWeightData(1, 0.3), new AccordWeightData(2, 0.4)]);
        var perfumeData1 = new PerfumeFormulaData(1, 1, 1, [], accords1);
        var perfume1 = PerfumeMathModel.FromFormulaData(perfumeData1);

        var accords2 = new List<AccordWeightData>([new AccordWeightData(1, 0.3), new AccordWeightData(2, 0.4)]);
        var perfumeData2 = new PerfumeFormulaData(2, 2, 2, [], accords2);
        var perfume2 = PerfumeMathModel.FromFormulaData(perfumeData2);

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

        var accords1 = new List<AccordWeightData>([new AccordWeightData(1, 0.3), new AccordWeightData(2, 0.4)]);
        var perfumeData1 = new PerfumeFormulaData(1, 2, 2, [], accords1);
        var perfume1 = PerfumeMathModel.FromFormulaData(perfumeData1);

        var accords2 = new List<AccordWeightData>([new AccordWeightData(3, 0.3), new AccordWeightData(4, 0.4)]);
        var perfumeData2 = new PerfumeFormulaData(2, 2, 2, [], accords2);
        var perfume2 = PerfumeMathModel.FromFormulaData(perfumeData2);

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

        var accords1 = new List<AccordWeightData>([new AccordWeightData(1, 0.3), new AccordWeightData(2, 0.4)]);
        var perfumeData1 = new PerfumeFormulaData(1, 2, 2, [], accords1);
        var perfume1 = PerfumeMathModel.FromFormulaData(perfumeData1);

        var accords2 = new List<AccordWeightData>([new AccordWeightData(1, 0.5), new AccordWeightData(3, 0.3)]);
        var perfumeData2 = new PerfumeFormulaData(2, 2, 2, [], accords2);
        var perfume2 = PerfumeMathModel.FromFormulaData(perfumeData2);

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
