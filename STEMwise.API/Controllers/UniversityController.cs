using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using STEMwise.Application.Interfaces;

namespace STEMwise.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UniversityController : ControllerBase
{
    private readonly IUniversityService _universityService;
    private readonly ICollegeScorecardClient _scorecardClient;

    public UniversityController(IUniversityService universityService, ICollegeScorecardClient scorecardClient)
    {
        _universityService = universityService;
        _scorecardClient = scorecardClient;
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<UniversitySearchResult>>> Search([FromQuery] string q, [FromQuery] string? country = null)
    {
        var results = await _universityService.SearchUniversitiesAsync(q, country);
        return Ok(results);
    }

    [HttpGet("{id}/scorecard")]
    public async Task<ActionResult<ScorecardSchool>> GetScorecard(Guid id)
    {
        var scorecardId = await _universityService.GetSchoolScorecardIdAsync(id);
        if (scorecardId == null) return NotFound("University not found or no Scorecard ID mapped.");

        var school = await _scorecardClient.GetSchoolByIdAsync(scorecardId.Value);
        return Ok(school);
    }
}
