using FragranceExplorer.BLL.Models;

namespace FragranceExplorer.BLL.Repositories;

public interface IPerfumeRepository
{
    IEnumerable<Perfume> GetAll();

    Perfume? GetById(int id);
}
