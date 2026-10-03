using FragranceExplorer.BLL.Models;

namespace FragranceExplorer.BLL.Strategies;

public class AccordCosineSimilarityStrategy : ISimilarityStrategy
{
    public double CalculateSimilarity(Perfume target, Perfume candidate)
    {
        var targetVector = target.ToAccordsVector();
        var candidateVector = candidate.ToAccordsVector();

        var similarity = targetVector.DotProduct(candidateVector) / (targetVector.Magnitude() * candidateVector.Magnitude());

        return similarity;
    }
}
