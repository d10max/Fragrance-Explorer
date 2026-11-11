using FragranceExplorer.BLL.Models;
using FragranceExplorer.BLL.Parser.Houses.Models;

namespace FragranceExplorer.BLL.Parser.Houses.Interfaces;

public interface IOlfactoryDnaMapper
{
    HouseOlfactoryDna? Map(RawOlfactoryDnaDto? dto);

    IEnumerable<HouseOlfactoryDna> MapMany(IEnumerable<RawOlfactoryDnaDto>? dtos);
}

