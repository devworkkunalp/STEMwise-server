using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STEMwise.Application.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace STEMwise.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ScenarioController : ControllerBase
{
    private readonly IScenarioService _scenarioService;
    private readonly IProfileService _profileService;

    public ScenarioController(IScenarioService scenarioService, IProfileService profileService)
    {
        _scenarioService = scenarioService;
        _profileService = profileService;
    }

    [HttpPost("model")]
    public async Task<IActionResult> ModelScenario([FromBody] ScenarioModelRequest request)
    {
        Console.WriteLine($"[SCENARIO] Modeling request received for Profile: {request.ProfileId}, Type: {request.ScenarioType}");
        try
        {
            var result = await _scenarioService.ModelScenarioAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[SCENARIO ERROR] {ex.Message} \n {ex.StackTrace}");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveScenario([FromBody] ScenarioModelResult result)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId)) return Unauthorized();

        var profile = await _profileService.GetProfileByUserIdAsync(userId);
        if (profile == null) return NotFound("Profile not found");

        try
        {
            var success = await _scenarioService.SaveScenarioAsync(profile.Id, result);
            return Ok(new { success });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId)) return Unauthorized();

        var profile = await _profileService.GetProfileByUserIdAsync(userId);
        if (profile == null) return NotFound("Profile not found");

        var history = await _scenarioService.GetScenarioHistoryAsync(profile.Id);
        return Ok(history);
    }
}
