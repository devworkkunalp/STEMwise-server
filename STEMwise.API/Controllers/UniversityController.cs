using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEMwise.Application.Interfaces;

namespace STEMwise.API.Controllers;

/// <summary>
/// Manages university data and live academic metrics.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UniversityController : ControllerBase
{
    private readonly IUniversityService _universityService;
    private readonly ICollegeScorecardClient _scorecardClient;

    public UniversityController(IUniversityService universityService, ICollegeScorecardClient scorecardClient)
    {
        _universityService = universityService;
        _scorecardClient = scorecardClient;
    }

    /// <summary>
    /// Searches for universities with optional country filtering.
    /// </summary>
    /// <remarks>
    /// **Key Points:**
    /// - Performs fuzzy name matching on the university database.
    /// - Returns ranking tier and regional identifiers.
    /// </remarks>
    /// <param name="q">Search query string.</param>
    /// <param name="country">Optional 2-letter country code filter.</param>
    /// <returns>A list of matching universities.</returns>
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UniversitySearchResult>>> Search([FromQuery] string q, [FromQuery] string? country = null)
    {
        var results = await _universityService.SearchUniversitiesAsync(q, country);
        return Ok(results);
    }

    /// <summary>
    /// Fetches live financial and employment data for a university from the US College Scorecard API.
    /// </summary>
    /// <remarks>
    /// **Key Points:**
    /// - Requires the internal university ID to map to the Scorecard UnitID.
    /// - Returns median earnings (10yr), tuition, and completion rates.
    /// </remarks>
    /// <param name="id">Internal University GUID.</param>
    /// <returns>Live data from the College Scorecard.</returns>
    [HttpGet("{id}/scorecard")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ScorecardSchool>> GetScorecard(Guid id)
    {
        var scorecardId = await _universityService.GetSchoolScorecardIdAsync(id);
        if (scorecardId == null) return NotFound("University not found or no Scorecard ID mapped.");

        var school = await _scorecardClient.GetSchoolByIdAsync(scorecardId.Value);
        return Ok(school);
    }
}
