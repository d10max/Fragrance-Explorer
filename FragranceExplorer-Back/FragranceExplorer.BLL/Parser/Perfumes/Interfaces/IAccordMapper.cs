using FragranceExplorer.BLL.Parser.Perfumes.Models;
using FragranceExplorer.BLL.Models;

namespace FragranceExplorer.BLL.Parser.Perfumes.Interfaces;

public interface IAccordMapper
{
    PerfumeAccord? Map(RawAccordDto? dto);

    IEnumerable<PerfumeAccord> MapMany(IEnumerable<RawAccordDto>? dtos);
}
