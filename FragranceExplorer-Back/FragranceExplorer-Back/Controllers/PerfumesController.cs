using FragranceExplorer.BLL.Constants;
using FragranceExplorer.BLL.DTOs.PerfumeRecommendationsDtos;
using FragranceExplorer.BLL.DTOs.PerfumeSearchDtos;
using FragranceExplorer.BLL.Handlers;
using FragranceExplorer.DAL.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FragranceExplorer_Back.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PerfumesController : ControllerBase
{
    private readonly GetPerfumeRecommendationsHandler _recommendationsHandler;
    private readonly PerfumeSearchHandler _searchHandler;

    public PerfumesController(
        GetPerfumeRecommendationsHandler recommendationsHandler,
        PerfumeSearchHandler searchHandler)
    {
        _recommendationsHandler = recommendationsHandler;
        _searchHandler = searchHandler;
    }

    [HttpGet("{id:int}/recommendations")]
    [ProducesResponseType(typeof(PerfumeRecommendationsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PerfumeRecommendationsDto>> GetRecommendations(
        int id,
        [FromQuery] int count = 10,
        [FromQuery] SearchProfile profile = SearchProfile.Default,
        CancellationToken ct = default)
    {
        if (id <= 0 || count <= 0)
        {
            return BadRequest();
        }

        var query = new GetPerfumeRecommendationsQuery(id, count, profile);
        var result = await _recommendationsHandler.HandleAsync(query, ct);

        if (result is null)
        {
            return NotFound(ErrorMessagesConstants.PerfumeNotFoundMessage);
        }

        return Ok(result);
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(PerfumeSearchDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PerfumeSearchDto>> SearchPerfumes(
    [FromQuery] string? query,
    [FromQuery] int maxResults = ScentConstants.DefaultSearchResult,
    CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest(ErrorMessagesConstants.SearchQueryCannotBeEmpty);
        }

        if (maxResults <= 0)
        {
            return BadRequest();
        }

        var searchQuery = new PerfumeSearchQuery(query, maxResults);
        var result = await _searchHandler.HandleAsync(searchQuery, ct);

        return Ok(result);
    }
}
