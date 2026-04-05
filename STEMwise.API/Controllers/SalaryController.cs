using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEMwise.Application.Interfaces;
using STEMwise.Domain.Entities;

namespace STEMwise.API.Controllers;

/// <summary>
/// Provides salary benchmarking and H-1B immigration statistics.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SalaryController : ControllerBase
{
    private readonly ISalaryService _salaryService;

    public SalaryController(ISalaryService salaryService)
    {
        _salaryService = salaryService;
    }

    /// <summary>
    /// Retrieves curated salary benchmarks based on field and location.
    /// </summary>
    /// <remarks>
    /// **Key Points:**
    /// - Supports filtering by Metro Area for more granular US data.
    /// - Returns wage levels (1-4) relevant for H-1B selection odds.
    /// </remarks>
    /// <param name="field">The STEM field of study.</param>
    /// <param name="country">Target country code.</param>
    /// <param name="metro">Optional metro area name.</param>
    /// <returns>A list of salary benchmarks.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SalaryBenchmark>>> GetSalaries([FromQuery] string field, [FromQuery] string country, [FromQuery] string? metro = null)
    {
        var result = await _salaryService.GetSalariesByFieldAsync(field, country, metro);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves historical H-1B selection rates and wage thresholds.
    /// </summary>
    /// <remarks>
    /// **Key Points:**
    /// - Data used to calculate the 'H1B Probability' in ROI reports.
    /// - Organized by fiscal year.
    /// </remarks>
    /// <returns>A list of historical H-1B statistics.</returns>
    [HttpGet("h1b-stats")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<H1BStatistic>>> GetH1BStats()
    {
        var stats = await _salaryService.GetH1BStatisticsAsync();
        return Ok(stats);
    }
}
