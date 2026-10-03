using FragranceExplorer.BLL.Constants;
using FragranceExplorer.BLL.DataSetParser.Common;
using FragranceExplorer.BLL.DataSetParser.Interfaces;
using FragranceExplorer.BLL.Models;
using System.Collections.Generic;

namespace FragranceExplorer.BLL.Repositories;

public class InMemoryPerfumeRepository : IPerfumeRepository
{
    private readonly List<Perfume> _perfumes = [];
    private readonly IPerfumeDataSetParser _parser;

    public InMemoryPerfumeRepository(IPerfumeDataSetParser parser)
    {
        ArgumentNullException.ThrowIfNull(parser);
        _parser = parser;
    }

    public async Task InitializeAsync()
    {
        var currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());
        string? datasetPath = null;

        while (currentDir != null)
        {
            var candidate = Path.Combine(currentDir.FullName, "FragranceExplorer.BLL", "DataSetParser", "perfumes_actual.jsonl");
            if (File.Exists(candidate))
            {
                datasetPath = candidate;
                break;
            }
            currentDir = currentDir.Parent;
        }

        if (datasetPath == null || !File.Exists(datasetPath))
        {
            throw new FileNotFoundException();
        }

        var options = new ParserOptions
        {
            PathToDataset = datasetPath,
            MaxPerfumesToParse = ScentConstants.TestMaxPerfumesAmountToParse
        };

        await foreach (var perfume in _parser.ParseAsync(options))
        {
            if (perfume != null)
            {
                _perfumes.Add(perfume);
            }
        }
    }

    public IEnumerable<Perfume> GetAll()
    {
        return _perfumes.OrderByDescending(p => p.Brand);
    }

    public Perfume? GetById(int id)
    {
        return _perfumes.FirstOrDefault(p => p.Id == id);
    }
}
