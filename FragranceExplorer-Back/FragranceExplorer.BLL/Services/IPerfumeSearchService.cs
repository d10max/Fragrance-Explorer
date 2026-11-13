using FragranceExplorer.BLL.Constants;
using FragranceExplorer.BLL.Models;

namespace FragranceExplorer.BLL.Services;

public interface IPerfumeSearchService
{
    IEnumerable<Perfume> Search(string query, int maxResults = ScentConstants.DefaultSearchResult);
}
