using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using STEMwise.Application.Interfaces;
using STEMwise.Domain.Entities;

namespace STEMwise.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalaryController : ControllerBase
{
    private readonly ISalaryService _salaryService;

    public SalaryController(ISalaryService salaryService)
    {
        _salaryService = salaryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SalaryBenchmark>>> GetSalaries([FromQuery] string field, [FromQuery] string country, [FromQuery] string? metro = null)
    {
        var result = await _salaryService.GetSalariesByFieldAsync(field, country, metro);
        return Ok(result);
    }

    [HttpGet("h1b-stats")]
    public async Task<ActionResult<IEnumerable<H1BStatistic>>> GetH1BStats()
    {
        var stats = await _salaryService.GetH1BStatisticsAsync();
        return Ok(stats);
    }
}
