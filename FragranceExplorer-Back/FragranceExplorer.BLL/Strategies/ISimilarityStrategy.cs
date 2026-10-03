using FragranceExplorer.BLL.Models;

namespace FragranceExplorer.BLL.Strategies;

public interface ISimilarityStrategy
{
    double CalculateSimilarity(Perfume target, Perfume candidate);
}
