using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEMwise.Application.Interfaces;

namespace STEMwise.API.Controllers;

/// <summary>
/// Orchestrates the Data Enrichment Pipeline from multiple external sources.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EnrichmentController : ControllerBase
{
    private readonly IEnrichmentService _enrichmentService;

    public EnrichmentController(IEnrichmentService enrichmentService)
    {
        _enrichmentService = enrichmentService;
    }

    /// <summary>
    /// Executes the full data enrichment pipeline (Scorecard + DOL + FX + H1B).
    /// </summary>
    /// <param name="request">Parameters for enrichment.</param>
    /// <returns>A unified enriched profile result.</returns>
    [HttpPost("profile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<EnrichedProfileDto>> EnrichProfile([FromBody] EnrichmentRequest request)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized();
        }

        var result = await _enrichmentService.EnrichProfileAsync(userId, request);
        return Ok(result);
    }
}
