using FluentAssertions;
using FragranceExplorer.BLL.Constants;
using FragranceExplorer.BLL.Models.MathModels;
using FragranceExplorer.BLL.Strategies;
using FragranceExplorer.DAL.Entities.MathDataEntities;
using FragranceExplorer.DAL.Enums;

namespace FragranceExplorer.BLL.Tests.Strategies;

public class CombinedSimilarityStrategyTests
{
    [Fact]
    public void Constructor_WhenWeightsDoNotSumToOne_ShouldThrowArgumentException()
    {
        // Arrange
        var noteStrategy = new NoteJaccardSimilarityStrategy();
        var accordStrategy = new AccordCosineSimilarityStrategy();
        double accordWeight = 0.4;
        double noteWeight = 0.4;

        // Act
        Action act = () => new CombinedSimilarityStrategy(noteStrategy, accordStrategy, accordWeight, noteWeight);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage(ErrorMessagesConstants.InvalidSimilarityWeights);
    }

    [Fact]
    public void Constructor_WhenStrategyIsNull_ShouldThrowArgumentNullException() {
        var validStrategy = new AccordCosineSimilarityStrategy();

        Action actNoteNull = () => new CombinedSimilarityStrategy(null!, validStrategy, 0.5, 0.5);
        Action actAccordNull = () => new CombinedSimilarityStrategy(validStrategy, null!, 0.5, 0.5);

        actNoteNull.Should().Throw<ArgumentNullException>();
        actAccordNull.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CalculateSimilarity_ShouldReturnWeightedSumOfAccordAndNoteScores(){
        var noteStrategy = new NoteJaccardSimilarityStrategy();
        var accordStrategy = new AccordCosineSimilarityStrategy();
        double accordWeight = 0.7;
        double noteWeight = 0.3;

        var combinedStrategy = new CombinedSimilarityStrategy(noteStrategy, accordStrategy, accordWeight, noteWeight);

        var accord1 = new List<AccordWeightData>([new AccordWeightData(1, 0.5)]);
        var note1 = new List<NoteWeightData>([new NoteWeightData(1, 0.8, NoteLayer.Top)]);
        var perfumeData1 = new PerfumeFormulaData(1, 2, 2, note1, accord1);
        var perfume1 = PerfumeMathModel.FromFormulaData(perfumeData1);

        var accord2 = new List<AccordWeightData>([new AccordWeightData(1, 0.5)]);
        var note2 = new List<NoteWeightData>([new NoteWeightData(2, 0.8, NoteLayer.Top)]);
        var perfumeData2 = new PerfumeFormulaData(2, 2, 2, note2, accord2);
        var perfume2 = PerfumeMathModel.FromFormulaData(perfumeData2);

        double actualSimilarity = combinedStrategy.CalculateSimilarity(perfume1, perfume2);

        // AccordScore = 1.0 (ідентичні акорди)
        // NoteScore = 0.0 (різні ноти)
        // Expected = (1.0 * 0.7) + (0.0 * 0.3) = 0.7
        actualSimilarity.Should().BeApproximately(0.7, precision: 1e-5);
    }
    
}