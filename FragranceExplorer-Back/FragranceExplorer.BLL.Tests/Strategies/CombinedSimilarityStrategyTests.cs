using FragranceExplorer.BLL.Constants;
using FragranceExplorer.BLL.Enums;
using FragranceExplorer.BLL.Strategies;
using FragranceExplorer.BLL.Models;
using FluentAssertions;
using Xunit;

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
        var perfume1 = new Perfume(1, "Perfume A", "Brand A", GenderCategory.Unisex, 4.5);
        perfume1.AddAccord(new PerfumeAccord("woody", 0.5));
        perfume1.AddNote(new PerfumeNote("bergamot", 0.8, NoteLayer.Top));

        var perfume2 = new Perfume(2, "Perfume B", "Brand B", GenderCategory.Unisex, 4.0);
        perfume2.AddAccord(new PerfumeAccord("woody", 0.5));
        perfume2.AddNote(new PerfumeNote("rose", 0.8, NoteLayer.Top));

        double actualSimilarity = combinedStrategy.CalculateSimilarity(perfume1, perfume2);

        // AccordScore = 1.0 (ідентичні акорди)
        // NoteScore = 0.0 (різні ноти)
        // Expected = (1.0 * 0.7) + (0.0 * 0.3) = 0.7
        actualSimilarity.Should().BeApproximately(0.7, precision: 1e-5);
    }
    
}