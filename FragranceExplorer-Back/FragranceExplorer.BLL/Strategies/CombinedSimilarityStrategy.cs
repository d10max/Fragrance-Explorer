using FragranceExplorer.BLL.Constants;
using FragranceExplorer.BLL.Models;

namespace FragranceExplorer.BLL.Strategies;

public class CombinedSimilarityStrategy : ISimilarityStrategy
{
    private readonly ISimilarityStrategy _noteStrategy;
    private readonly ISimilarityStrategy _accordStrategy;
    private readonly double _noteWeight; 
    private readonly double _accordWeight; 

    public CombinedSimilarityStrategy(
        ISimilarityStrategy noteStrategy,
        ISimilarityStrategy accordStrategy,
        double accordWeight,
        double noteWeight)
    {
        ArgumentNullException.ThrowIfNull(noteStrategy);
        ArgumentNullException.ThrowIfNull(accordStrategy);

        if (Math.Abs(noteWeight + accordWeight - 1.0) > ScentConstants.Epsilon)
        {
            throw new ArgumentException(ErrorMessagesConstants.InvalidSimilarityWeights);
        }

        _accordStrategy = accordStrategy;
        _noteStrategy = noteStrategy;
        _accordWeight = accordWeight;
        _noteWeight = noteWeight;
    }
    public double CalculateSimilarity(Perfume target, Perfume candidate)
    {
        double accordSimilarityScore = _accordStrategy.CalculateSimilarity(target, candidate);
        double noteSimilarityScore = _noteStrategy.CalculateSimilarity(target, candidate);

        return (accordSimilarityScore * _accordWeight) + (noteSimilarityScore * _noteWeight);
    }
}
