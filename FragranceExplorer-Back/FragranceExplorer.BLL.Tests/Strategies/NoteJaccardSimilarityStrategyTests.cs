using FragranceExplorer.BLL.Enums;
using FragranceExplorer.BLL.Models;
using FragranceExplorer.BLL.Strategies;
using FluentAssertions;
using Xunit;

namespace FragranceExplorer.BLL.Tests.Strategies;

public class NoteJaccardSimilarityStrategyTests
{
    [Fact]
    public void CalculateSimilarity_WhenPerfumesHaveIdenticalPyramidNotes_ShouldReturnOne()
    {
        // 1. Arrange
        var strategy = new NoteJaccardSimilarityStrategy();

        var perfume1 = new Perfume(1, "Perfume A", "Brand A", GenderCategory.Unisex, 4.5);
        perfume1.AddNote(new PerfumeNote("bergamot", 0.8, NoteLayer.Top));
        perfume1.AddNote(new PerfumeNote("rose", 0.6, NoteLayer.Middle));
        perfume1.AddNote(new PerfumeNote("amber", 0.9, NoteLayer.Base));

        var perfume2 = new Perfume(2, "Perfume B", "Brand B", GenderCategory.Unisex, 4.0);
        perfume2.AddNote(new PerfumeNote("bergamot", 0.8, NoteLayer.Top));
        perfume2.AddNote(new PerfumeNote("rose", 0.6, NoteLayer.Middle));
        perfume2.AddNote(new PerfumeNote("amber", 0.9, NoteLayer.Base));

        // 2. Act
        double actualSimilarity = strategy.CalculateSimilarity(perfume1, perfume2);

        // 3. Assert
        actualSimilarity.Should().BeApproximately(1.0, precision: 1e-5);
    }

    [Fact]
    public  void CalculateSimilarity_WhenPerfumesHaveNotIdenticalPyramidNotes_ShouldReturnZero (){
        var strategy = new NoteJaccardSimilarityStrategy();

        var perfume1 = new Perfume(1, "Perfume A", "Brand A", GenderCategory.Unisex, 4.5);
        perfume1.AddNote(new PerfumeNote("bergamot", 0.8, NoteLayer.Top));
        perfume1.AddNote(new PerfumeNote("rose", 0.6, NoteLayer.Middle));
        perfume1.AddNote(new PerfumeNote("amber", 0.9, NoteLayer.Base));

        var perfume2 = new Perfume(2, "Perfume B", "Brand B", GenderCategory.Unisex, 4.0);
        perfume2.AddNote(new PerfumeNote("lather", 0.8, NoteLayer.Top));
        perfume2.AddNote(new PerfumeNote("lemon", 0.6, NoteLayer.Middle));
        perfume2.AddNote(new PerfumeNote("woody", 0.9, NoteLayer.Base));

        double actualSimilarity = strategy.CalculateSimilarity(perfume1, perfume2);

        actualSimilarity.Should().BeApproximately(0.0, precision: 1e-5);
    }

    [Fact]
    public void CalculateSimilarity_WhenPerfumesHaveSimilarityInBaseNotesOnly_ShouldReturnZeropointFive()
    {
        var strategy = new NoteJaccardSimilarityStrategy();

        var perfume1 = new Perfume(1, "Perfume A", "Brand A", GenderCategory.Unisex, 4.5);
        perfume1.AddNote(new PerfumeNote("bergamot", 0.8, NoteLayer.Top));
        perfume1.AddNote(new PerfumeNote("rose", 0.6, NoteLayer.Middle));
        perfume1.AddNote(new PerfumeNote("amber", 0.9, NoteLayer.Base));

        var perfume2 = new Perfume(2, "Perfume B", "Brand B", GenderCategory.Unisex, 4.0);
        perfume2.AddNote(new PerfumeNote("lather", 0.8, NoteLayer.Top));
        perfume2.AddNote(new PerfumeNote("lemon", 0.6, NoteLayer.Middle));
        perfume2.AddNote(new PerfumeNote("amber", 0.9, NoteLayer.Base));

        double actualSimilarity = strategy.CalculateSimilarity(perfume1, perfume2);
        // Similar base "amber" note
        // Base  note is similar so result is 0.5
        actualSimilarity.Should().BeApproximately(0.5, precision: 1e-5);
    }

    [Fact]
    public void CalculateSimilarity_WhenPerfumesHaveSimilarityInDifferentLayers_ShouldReturnZero () {
        var strategy = new NoteJaccardSimilarityStrategy();

        var perfume1 = new Perfume(1, "Perfume A", "Brand A", GenderCategory.Unisex, 4.5);
        perfume1.AddNote(new PerfumeNote("bergamot", 0.8, NoteLayer.Top));
        perfume1.AddNote(new PerfumeNote("lather", 0.6, NoteLayer.Middle));
        perfume1.AddNote(new PerfumeNote("amber", 0.9, NoteLayer.Base));

        var perfume2 = new Perfume(2, "Perfume B", "Brand B", GenderCategory.Unisex, 4.0);
        perfume2.AddNote(new PerfumeNote("lather", 0.8, NoteLayer.Top));
        perfume2.AddNote(new PerfumeNote("lemon", 0.6, NoteLayer.Middle));
        perfume2.AddNote(new PerfumeNote("rose", 0.9, NoteLayer.Base));

        double actualSimilarity = strategy.CalculateSimilarity(perfume1, perfume2);

        actualSimilarity.Should().BeApproximately(0.0, precision: 1e-5);
    }

    [Fact]
    public void CalculateSimilarity_WhenPerfumesHaveNotNotes_ShouldReturnZero (){
        var strategy = new NoteJaccardSimilarityStrategy();

        var perfume1 = new Perfume(1, "Perfume A", "Brand A", GenderCategory.Unisex, 4.5);
        var perfume2 = new Perfume(2, "Perfume B", "Brand B", GenderCategory.Unisex, 4.0);

        double actualSimilarity = strategy.CalculateSimilarity(perfume1, perfume2);

        actualSimilarity.Should().BeApproximately(0.0, precision: 1e-5);
    }
}