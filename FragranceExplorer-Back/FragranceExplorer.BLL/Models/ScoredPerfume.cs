using FragranceExplorer.BLL.Constants;

namespace FragranceExplorer.BLL.Models;

public class ScoredPerfume
{
    public Perfume Perfume { get; init; }

    public double Score { get; init; }

    public ScoredPerfume(Perfume perfume, double score)
    {
        ArgumentNullException.ThrowIfNull(perfume);
        if (score < ScentConstants.MinPerfumeSimilarityScore || score > ScentConstants.MaxPerfumeSimilarityScore)
        {
            throw new ArgumentOutOfRangeException(ErrorMessagesConstants.PerfumeSimilarityScoreIsOutOfRange());
        }

        Perfume = perfume;
        Score = score;
    }
}
