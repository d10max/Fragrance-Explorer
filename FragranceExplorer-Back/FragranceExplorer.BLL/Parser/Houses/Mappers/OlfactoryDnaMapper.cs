using FragranceExplorer.BLL.Models;
using FragranceExplorer.BLL.Parser.Houses.Interfaces;
using FragranceExplorer.BLL.Parser.Houses.Models;

namespace FragranceExplorer.BLL.Parser.Houses.Mappers;

public class OlfactoryDnaMapper : IOlfactoryDnaMapper
{
    public HouseOlfactoryDna? Map(RawOlfactoryDnaDto? dto)
    {
        if (dto is null || string.IsNullOrWhiteSpace(dto.Family) || string.IsNullOrWhiteSpace(dto.Notes))
        {
            return null;
        }

        var clampedPercentage = Math.Clamp(dto.Percentage, 0, 100);

        return new HouseOlfactoryDna(dto.Family.Trim(), dto.Notes.Trim(), clampedPercentage);
    }

    public IEnumerable<HouseOlfactoryDna> MapMany(IEnumerable<RawOlfactoryDnaDto>? dtos)
    {
        if (dtos is null)
        {
            yield break;
        }

        foreach (var dto in dtos)
        {
            var dna = Map(dto);
            if (dna is not null)
            {
                yield return dna;
            }
        }
    }
}

