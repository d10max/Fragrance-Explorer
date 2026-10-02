using FragranceExplorer.BLL.Constants;
using FragranceExplorer.BLL.DataSetParser.Interfaces;
using FragranceExplorer.BLL.DataSetParser.Models;
using FragranceExplorer.BLL.Models;

namespace FragranceExplorer.BLL.DataSetParser.Mappers;

public class AccordMapper : IAccordMapper
{
    public PerfumeAccord? Map(RawAccordDto? dto)
    {
        if (dto is null || string.IsNullOrWhiteSpace(dto.Name))
        {
            return null;
        }

        var normalizedIntensity = Math.Clamp(
            dto.Strength / 100.0,
            ScentConstants.MinIntensity,
            ScentConstants.MaxIntensity);

        return new PerfumeAccord(dto.Name.Trim(), normalizedIntensity);
    }

    public IEnumerable<PerfumeAccord> MapMany(IEnumerable<RawAccordDto>? dtos)
    {
        if (dtos is null)
        {
            yield break;
        }

        foreach (var dto in dtos)
        {
            var accord = Map(dto);
            if (accord is not null)
            {
                yield return accord;
            }
        }
    }
}
