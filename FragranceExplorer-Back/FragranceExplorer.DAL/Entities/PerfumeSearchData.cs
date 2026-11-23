namespace FragranceExplorer.DAL.Entities;

public  record PerfumeSearchData(
    int Id,
    string Name,
    string BrandName,
    int? People,
    double Rating
);
