using FragranceExplorer.BLL.Models.MathModels;

namespace FragranceExplorer.BLL.Strategies;

public interface ISimilarityStrategy
{
    double CalculateSimilarity(PerfumeMathModel target, PerfumeMathModel candidate);
}
