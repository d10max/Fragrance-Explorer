using FragranceExplorer.BLL.Constants;
using FragranceExplorer.BLL.Enums;
using FragranceExplorer.BLL.Models;
using FragranceExplorer.BLL.Repositories;
using FragranceExplorer.BLL.Services;
using FragranceExplorer.BLL.Strategies;
using Microsoft.AspNetCore.Mvc;

namespace FragranceExplorer_Back.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PerfumesController : ControllerBase
{
    private readonly IPerfumeRepository _repository;
    private readonly RecommendationEngine _engine;
    private readonly NoteJaccardSimilarityStrategy _noteStrategy;
    private readonly AccordCosineSimilarityStrategy _accordStrategy;

    public PerfumesController(
        IPerfumeRepository repository,
        RecommendationEngine engine,
        NoteJaccardSimilarityStrategy noteStrategy,
        AccordCosineSimilarityStrategy accordStrategy)
    {
        _repository = repository;
        _engine = engine;
        _noteStrategy = noteStrategy;
        _accordStrategy = accordStrategy;
    }

    [HttpGet]
    public IActionResult GetPerfumes([FromQuery] int count = 10)
    {
        var perfumes = _repository.GetAll().Take(count);
        return Ok(perfumes);
    }


    [HttpGet("{id}")]
    public IActionResult GetPerfumeById(int id)
    {
        var perfume = _repository.GetById(id);
        if (perfume == null) return NotFound(ErrorMessagesConstants.PerfumeNotFoundMessage);

        return Ok(perfume);
    }

    [HttpGet("compare/notes")]
    public IActionResult CompareNotes([FromQuery] int id1, [FromQuery] int id2)
    {
        var (p1, p2) = GetTwoPerfumes(id1, id2);
        if (p1 == null || p2 == null) return NotFound(ErrorMessagesConstants.PerfumeNotFoundMessage);

        double score = _noteStrategy.CalculateSimilarity(p1, p2);
        return Ok(new { Target = p1.Name, Candidate = p2.Name, NoteSimilarity = score });
    }

    [HttpGet("compare/accords")]
    public IActionResult CompareAccords([FromQuery] int id1, [FromQuery] int id2)
    {
        var (p1, p2) = GetTwoPerfumes(id1, id2);
        if (p1 == null || p2 == null) return NotFound(ErrorMessagesConstants.PerfumeNotFoundMessage);

        double score = _accordStrategy.CalculateSimilarity(p1, p2);
        return Ok(new { Target = p1.Name, Candidate = p2.Name, AccordSimilarity = score });
    }

    [HttpGet("compare/combined")]
    public IActionResult CompareCombined([FromQuery] int id1, [FromQuery] int id2, [FromQuery] SearchProfile profile = SearchProfile.Default)
    {
        var (p1, p2) = GetTwoPerfumes(id1, id2);
        if (p1 == null || p2 == null) return NotFound(ErrorMessagesConstants.PerfumeNotFoundMessage);

        var weights = ScentConstants.GetSimilarityWeights(profile);
        var combinedStrategy = new CombinedSimilarityStrategy(_accordStrategy, _noteStrategy, weights.accordWeight, weights.noteWeight);

        double score = combinedStrategy.CalculateSimilarity(p1, p2);
        return Ok(new { Target = p1.Name, Candidate = p2.Name, Profile = profile.ToString(), CombinedSimilarity = score });
    }

    [HttpGet("{id}/recommendations")]
    public IActionResult GetRecommendations(int id, [FromQuery] int count = 10, [FromQuery] SearchProfile profile = SearchProfile.Default)
    {
        var target = _repository.GetById(id);
        if (target == null) return NotFound(ErrorMessagesConstants.PerfumeNotFoundMessage);

        var catalog = _repository.GetAll();
        var recommendations = _engine.GetRecommendations(target, catalog, count, profile);

        return Ok(new
        {
            TargetPerfume = target.Name,
            ProfileUsed = profile.ToString(),
            Recommendations = recommendations
        });
    }

    private (Perfume? p1, Perfume? p2) GetTwoPerfumes(int id1, int id2)
    {
        return (_repository.GetById(id1), _repository.GetById(id2));
    }
}
