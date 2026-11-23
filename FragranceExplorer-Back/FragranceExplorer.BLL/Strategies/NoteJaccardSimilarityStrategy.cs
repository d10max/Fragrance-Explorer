using FragranceExplorer.BLL.Constants;
using FragranceExplorer.BLL.Models.MathModels;

namespace FragranceExplorer.BLL.Strategies;

public class NoteJaccardSimilarityStrategy : ISimilarityStrategy
{
    public double CalculateSimilarity(PerfumeMathModel target, PerfumeMathModel candidate)
    {
        if (target.IsFlatStructure || candidate.IsFlatStructure)
        {
            return CalculateSingleLayerSimilarity(
                target.Notes,
                candidate.Notes);
        }

        double topScore = CalculateSingleLayerSimilarity(
            target.TopNotes,
            candidate.TopNotes);

        double midScore = CalculateSingleLayerSimilarity(
            target.MiddleNotes,
            candidate.MiddleNotes);

        double baseScore = CalculateSingleLayerSimilarity(
            target.BaseNotes,
            candidate.BaseNotes);

        return (topScore * ScentConstants.TopNoteLayerWeight) + (baseScore * ScentConstants.BaseNoteLayerWeight) + (midScore * ScentConstants.MiddleNoteLayerWeight);
    }

    private static double CalculateSingleLayerSimilarity(ScentVector<NoteMathModel> targetVector, ScentVector<NoteMathModel> candidateVector)
    {
        var allNoteKeys = targetVector.Significances.Keys
            .Union(candidateVector.Significances.Keys);

        double intersectionSum = 0.0;
        double unionSum = 0.0;

        foreach(var key in allNoteKeys)
        {
            var targetSignificance = targetVector.Significances.TryGetValue(key, out double significance) ? significance : 0.0;
            var candidateSignificance = candidateVector.Significances.TryGetValue(key, out significance) ? significance : 0.0;

            intersectionSum += Math.Min(targetSignificance, candidateSignificance);
            unionSum += Math.Max(targetSignificance, candidateSignificance);
        }

        if (Math.Abs(unionSum) < ScentConstants.Epsilon)
        {
            return 0.0;
        }

        return intersectionSum / unionSum;
    }
}
