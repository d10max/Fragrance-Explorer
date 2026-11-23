using FragranceExplorer.BLL.Constants;
using FragranceExplorer.BLL.Models;

namespace FragranceExplorer.BLL.Services;

public interface IPerfumeSearchService
{
    IEnumerable<int> Search(
        string query,
        IEnumerable<PerfumeSearchEntry> catalog,
        int maxResults = ScentConstants.DefaultSearchResult);
}
