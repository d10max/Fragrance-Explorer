using FragranceExplorer.BLL.Parser.Perfumes.Models;
using FragranceExplorer.BLL.Models;

namespace FragranceExplorer.BLL.Parser.Perfumes.Interfaces;

public interface IPerfumeMapper
{
    Perfume? MapToDomain(RawPerfumeDto? dto);
}
