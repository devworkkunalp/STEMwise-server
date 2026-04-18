using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STEMwise.Application.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace STEMwise.API.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class ResearchController : ControllerBase
{
    private readonly IResearchService _researchService;

    public ResearchController(IResearchService researchService)
    {
        _researchService = researchService;
    }

    [HttpGet("universities")]
    public async Task<IActionResult> GetUniversities([FromQuery] string? sector)
    {
        var data = await _researchService.GetRankedUniversitiesAsync(sector);
        return Ok(data);
    }

    [HttpGet("housing")]
    public async Task<IActionResult> GetHousingRents()
    {
        var data = await _researchService.GetRegionalRentsAsync();
        return Ok(data);
    }

    [HttpGet("visa-benchmarks")]
    public async Task<IActionResult> GetVisaBenchmarks()
    {
        var data = await _researchService.GetVisaBenchmarksAsync();
        return Ok(data);
    }

    [HttpGet("labor-benchmarks")]
    public async Task<IActionResult> GetLaborBenchmarks()
    {
        var data = await _researchService.GetLaborBenchmarksAsync();
        return Ok(data);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetResearchSummary()
    {
        // Combined summary for the Research Hub main dashboard
        var summary = new
        {
            TopSchools = await _researchService.GetRankedUniversitiesAsync(null),
            HubRents = await _researchService.GetRegionalRentsAsync(),
            VisaTrends = await _researchService.GetVisaBenchmarksAsync()
        };
        
        return Ok(summary);
    }

    [HttpGet("global")]
    public async Task<IActionResult> GetGlobalRankings()
    {
        var data = await _researchService.GetGlobalRankingsAsync();
        return Ok(data);
    }

    [HttpGet("global-alternatives")]
    public async Task<IActionResult> GetGlobalSectorAlternatives([FromQuery] string? specialization)
    {
        var data = await _researchService.GetGlobalSectorBenchmarksAsync(specialization);
        return Ok(data);
    }
}
