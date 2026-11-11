using FragranceExplorer.BLL.Models;
using FragranceExplorer.BLL.Parser.Common;

namespace FragranceExplorer.BLL.Parser.Houses.Interfaces;

public interface IHouseDataSetParser
{
    IAsyncEnumerable<House> ParseAsync(ParserOptions? options = null, CancellationToken cancellationToken = default);

    ParsingStatistics Statistics { get; }

    string GetStatisticsReport();
}
