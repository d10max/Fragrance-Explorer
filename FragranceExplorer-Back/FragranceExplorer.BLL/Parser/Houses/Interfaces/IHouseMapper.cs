using FragranceExplorer.BLL.Models;
using FragranceExplorer.BLL.Parser.Houses.Models;

namespace FragranceExplorer.BLL.Parser.Houses.Interfaces;

public interface IHouseMapper
{
    House? MapToDomain(RawHouseDto? dto);
}
