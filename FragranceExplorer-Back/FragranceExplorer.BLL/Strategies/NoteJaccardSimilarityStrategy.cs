using FragranceExplorer.BLL.Constants;
using FragranceExplorer.BLL.Models;
using FragranceExplorer.BLL.Enums;

namespace FragranceExplorer.BLL.Strategies;

public class NoteJaccardSimilarityStrategy : ISimilarityStrategy
{
    public double CalculateSimilarity(Perfume target, Perfume candidate)
    {
        if (target.IsFlatStructure() || candidate.IsFlatStructure())
        {
            return CalculateSingleLayerSimilarity(
                target.ToNotesVector(),
                candidate.ToNotesVector());
        }

        double topScore = CalculateSingleLayerSimilarity(
            target.ToNotesVectorByLayer(NoteLayer.Top),
            candidate.ToNotesVectorByLayer(NoteLayer.Top));

        double midScore = CalculateSingleLayerSimilarity(
            target.ToNotesVectorByLayer(NoteLayer.Middle),
            candidate.ToNotesVectorByLayer(NoteLayer.Middle));

        double baseScore = CalculateSingleLayerSimilarity(
            target.ToNotesVectorByLayer(NoteLayer.Base),
            candidate.ToNotesVectorByLayer(NoteLayer.Base));

        return (topScore * ScentConstants.TopNoteLayerWeight) + (baseScore * ScentConstants.BaseNoteLayerWeight) + (midScore * ScentConstants.MiddleNoteLayerWeight);
    }

    private static double CalculateSingleLayerSimilarity(ScentVector<PerfumeNote> targetVector, ScentVector<PerfumeNote> candidateVector)
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
