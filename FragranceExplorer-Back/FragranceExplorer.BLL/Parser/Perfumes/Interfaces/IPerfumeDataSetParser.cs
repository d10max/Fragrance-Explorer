using FragranceExplorer.BLL.Parser.Common;
using FragranceExplorer.BLL.Models;

namespace FragranceExplorer.BLL.Parser.Perfumes.Interfaces;

public interface IPerfumeDataSetParser
{
    IAsyncEnumerable<Perfume> ParseAsync(ParserOptions? options = null, CancellationToken cancellationToken = default);

    ParsingStatistics Statistics { get; }

    string GetStatisticsReport();
}

