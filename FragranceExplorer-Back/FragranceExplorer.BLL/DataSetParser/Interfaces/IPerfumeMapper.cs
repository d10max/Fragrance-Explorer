using FragranceExplorer.BLL.DataSetParser.Models;
using FragranceExplorer.BLL.Models;

namespace FragranceExplorer.BLL.DataSetParser.Interfaces;

public interface IPerfumeMapper
{
    Perfume? MapToDomain(RawPerfumeDto? dto);
}
