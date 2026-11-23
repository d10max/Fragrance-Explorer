using FluentAssertions;
using FragranceExplorer.BLL.Models;
using FragranceExplorer.BLL.Models.MathModels;
using FragranceExplorer.BLL.Strategies;
using FragranceExplorer.DAL.Entities.MathDataEntities;
using FragranceExplorer.DAL.Enums;
using Xunit;

namespace FragranceExplorer.BLL.Tests.Strategies;

public class NoteJaccardSimilarityStrategyTests
{
    [Fact]
    public void CalculateSimilarity_WhenPerfumesHaveIdenticalPyramidNotes_ShouldReturnOne()
    {
        // 1. Arrange
        var strategy = new NoteJaccardSimilarityStrategy();

        var notes1 = new List<NoteWeightData>([
            new NoteWeightData(1, 0.8, NoteLayer.Top),
            new NoteWeightData(2, 0.6, NoteLayer.Middle),
            new NoteWeightData(3, 0.9, NoteLayer.Base)]);
        var perfumeData1 = new PerfumeFormulaData(1, 2, 2, notes1, []);
        var perfume1 = PerfumeMathModel.FromFormulaData(perfumeData1);

        var notes2 = new List<NoteWeightData>([
            new NoteWeightData(1, 0.8, NoteLayer.Top),
            new NoteWeightData(2, 0.6, NoteLayer.Middle),
            new NoteWeightData(3, 0.9, NoteLayer.Base)]);
        var perfumeData2 = new PerfumeFormulaData(2, 2, 2, notes2, []);
        var perfume2 = PerfumeMathModel.FromFormulaData(perfumeData2);

        // 2. Act
        double actualSimilarity = strategy.CalculateSimilarity(perfume1, perfume2);

        // 3. Assert
        actualSimilarity.Should().BeApproximately(1.0, precision: 1e-5);
    }

    [Fact]
    public  void CalculateSimilarity_WhenPerfumesHaveNotIdenticalPyramidNotes_ShouldReturnZero (){
        var strategy = new NoteJaccardSimilarityStrategy();

        var notes1 = new List<NoteWeightData>([
            new NoteWeightData(1, 0.8, NoteLayer.Top),
            new NoteWeightData(2, 0.6, NoteLayer.Middle),
            new NoteWeightData(3, 0.9, NoteLayer.Base)]);
        var perfumeData1 = new PerfumeFormulaData(1, 2, 2, notes1, []);
        var perfume1 = PerfumeMathModel.FromFormulaData(perfumeData1);

        var notes2 = new List<NoteWeightData>([
            new NoteWeightData(4, 0.8, NoteLayer.Top),
            new NoteWeightData(5, 0.6, NoteLayer.Middle),
            new NoteWeightData(6, 0.9, NoteLayer.Base)]);
        var perfumeData2 = new PerfumeFormulaData(2, 2, 2, notes2, []);
        var perfume2 = PerfumeMathModel.FromFormulaData(perfumeData2);

        double actualSimilarity = strategy.CalculateSimilarity(perfume1, perfume2);

        actualSimilarity.Should().BeApproximately(0.0, precision: 1e-5);
    }

    [Fact]
    public void CalculateSimilarity_WhenPerfumesHaveSimilarityInBaseNotesOnly_ShouldReturnZeropointFive()
    {
        var strategy = new NoteJaccardSimilarityStrategy();

        var notes1 = new List<NoteWeightData>([
            new NoteWeightData(1, 0.8, NoteLayer.Top),
            new NoteWeightData(2, 0.6, NoteLayer.Middle),
            new NoteWeightData(3, 0.9, NoteLayer.Base)]);
        var perfumeData1 = new PerfumeFormulaData(1, 2, 2, notes1, []);
        var perfume1 = PerfumeMathModel.FromFormulaData(perfumeData1);

        var notes2 = new List<NoteWeightData>([
            new NoteWeightData(4, 0.8, NoteLayer.Top),
            new NoteWeightData(5, 0.6, NoteLayer.Middle),
            new NoteWeightData(3, 0.9, NoteLayer.Base)]);
        var perfumeData2 = new PerfumeFormulaData(2, 2, 2, notes2, []);
        var perfume2 = PerfumeMathModel.FromFormulaData(perfumeData2);

        double actualSimilarity = strategy.CalculateSimilarity(perfume1, perfume2);
        // Similar base "amber" note
        // Base  note is similar so result is 0.5
        actualSimilarity.Should().BeApproximately(0.5, precision: 1e-5);
    }

    [Fact]
    public void CalculateSimilarity_WhenPerfumesHaveSimilarityInDifferentLayers_ShouldReturnZero () {
        var strategy = new NoteJaccardSimilarityStrategy();

        var notes1 = new List<NoteWeightData>([
            new NoteWeightData(1, 0.8, NoteLayer.Top),
            new NoteWeightData(2, 0.6, NoteLayer.Middle),
            new NoteWeightData(3, 0.9, NoteLayer.Base)]);
        var perfumeData1 = new PerfumeFormulaData(1, 2, 2, notes1, []);
        var perfume1 = PerfumeMathModel.FromFormulaData(perfumeData1);

        var notes2 = new List<NoteWeightData>([
            new NoteWeightData(2, 0.8, NoteLayer.Top),
            new NoteWeightData(5, 0.6, NoteLayer.Middle),
            new NoteWeightData(6, 0.9, NoteLayer.Base)]);
        var perfumeData2 = new PerfumeFormulaData(2, 2, 2, notes2, []);
        var perfume2 = PerfumeMathModel.FromFormulaData(perfumeData2);

        double actualSimilarity = strategy.CalculateSimilarity(perfume1, perfume2);

        actualSimilarity.Should().BeApproximately(0.0, precision: 1e-5);
    }

    [Fact]
    public void CalculateSimilarity_WhenPerfumesHaveNotNotes_ShouldReturnZero (){
        var strategy = new NoteJaccardSimilarityStrategy();

        var perfumeData1 = new PerfumeFormulaData(1, 2, 2, [], []);
        var perfume1 = PerfumeMathModel.FromFormulaData(perfumeData1);

        var perfumeData2 = new PerfumeFormulaData(2, 2, 2, [], []);
        var perfume2 = PerfumeMathModel.FromFormulaData(perfumeData2);

        double actualSimilarity = strategy.CalculateSimilarity(perfume1, perfume2);

        actualSimilarity.Should().BeApproximately(0.0, precision: 1e-5);
    }
}