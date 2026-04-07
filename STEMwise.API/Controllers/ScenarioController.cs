using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STEMwise.Domain.Entities;
using STEMwise.Infrastructure.Data;

namespace STEMwise.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ScenarioController : ControllerBase
{
    private readonly AppDbContext _context;

    public ScenarioController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Saves a modeled "What-If" scenario for the user's profile.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<SavedScenario>> SaveScenario([FromBody] SavedScenario scenario)
    {
        if (scenario == null) return BadRequest();
        
        scenario.Id = Guid.NewGuid();
        _context.SavedScenarios.Add(scenario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetScenarios), new { profileId = scenario.ProfileId }, scenario);
    }

    /// <summary>
    /// Retrieves all saved scenarios for a specific profile.
    /// </summary>
    [HttpGet("{profileId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SavedScenario>>> GetScenarios(Guid profileId)
    {
        var scenarios = await _context.SavedScenarios
            .Where(s => s.ProfileId == profileId)
            .OrderByDescending(s => s.Id) // Roughly chronological if no createdAt
            .ToListAsync();

        return Ok(scenarios);
    }

    /// <summary>
    /// Deletes a saved scenario.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteScenario(Guid id)
    {
        var scenario = await _context.SavedScenarios.FindAsync(id);
        if (scenario == null) return NotFound();

        _context.SavedScenarios.Remove(scenario);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
