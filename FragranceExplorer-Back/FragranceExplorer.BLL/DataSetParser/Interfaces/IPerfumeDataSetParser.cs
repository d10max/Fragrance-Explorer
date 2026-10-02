using FragranceExplorer.BLL.DataSetParser.Common;
using FragranceExplorer.BLL.Models;

namespace FragranceExplorer.BLL.DataSetParser.Interfaces;

public interface IPerfumeDataSetParser
{
    IAsyncEnumerable<Perfume> ParseAsync(ParserOptions? options = null, CancellationToken cancellationToken = default);

    ParsingStatistics Statistics { get; }

    string GetStatisticsReport();
}

