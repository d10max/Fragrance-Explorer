using FragranceExplorer.BLL.Models.MathModels;

namespace FragranceExplorer.BLL.Strategies;

public class AccordCosineSimilarityStrategy : ISimilarityStrategy
{
    public double CalculateSimilarity(PerfumeMathModel target, PerfumeMathModel candidate)
    {
        var targetAccordVector = target.Accords;
        var candidateAccordVector = candidate.Accords;

        var similarity = targetAccordVector.DotProduct(candidateAccordVector) / (targetAccordVector.Magnitude() * candidateAccordVector.Magnitude());

        return similarity;
    }
}
