using FragranceExplorer.BLL.DataSetParser.Models;
using FragranceExplorer.BLL.Models;

namespace FragranceExplorer.BLL.DataSetParser.Interfaces;

public interface IAccordMapper
{
    PerfumeAccord? Map(RawAccordDto? dto);

    IEnumerable<PerfumeAccord> MapMany(IEnumerable<RawAccordDto>? dtos);
}
