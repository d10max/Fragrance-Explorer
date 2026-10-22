using FragranceExplorer.BLL.Models;
using FragranceExplorer.BLL.Strategies;
using FragranceExplorer.DAL.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FragranceExplorer_Back.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    // POST: api/testsimilarity/cosine
    [HttpPost("cosine")]
    public ActionResult<double> TestCosineSimilarity([FromBody] PerfumeComparisonRequestDto request)
    {
        if (request.Target == null || request.Candidate == null)
        {
            return BadRequest("Обидва парфуми (Target і Candidate) мають бути передані.");
        }

        var target = new Perfume(request.Target.Id, request.Target.Name, "Brand", GenderCategory.Unisex, 5.0);
        foreach (var accord in request.Target.Accords)
        {
            target.AddAccord(new PerfumeAccord(accord.Name, accord.Intensity));
        }

        // 2. Мапимо DTO в реальну доменну модель Candidate
        var candidate = new Perfume(request.Candidate.Id, request.Candidate.Name, "Brand", GenderCategory.Unisex, 5.0);
        foreach (var accord in request.Candidate.Accords)
        {
            candidate.AddAccord(new PerfumeAccord(accord.Name, accord.Intensity));
        }

        var strategy = new AccordCosineSimilarityStrategy();

        double score = strategy.CalculateSimilarity(target, candidate);

        return Ok(score);
    }
}

public class PerfumeComparisonRequestDto
{
    public PerfumeDto Target { get; set; }
    public PerfumeDto Candidate { get; set; }
}

public class PerfumeDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<AccordDto> Accords { get; set; } = new();
}

public class AccordDto
{
    public string Name { get; set; }
    public double Intensity { get; set; }
}
