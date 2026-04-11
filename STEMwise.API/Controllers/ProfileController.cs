using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEMwise.Application.DTOs;
using STEMwise.Application.Interfaces;
using STEMwise.Domain.Entities;
using STEMwise.Domain.Enums;

namespace STEMwise.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    /// <summary>
    /// Gets the current authenticated user's profile.
    /// </summary>
    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Profile>> GetMyProfile()
    {
        try
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                ?? User.FindFirst("sub")?.Value; // Fallback to 'sub' claim

            if (string.IsNullOrEmpty(userIdString))
            {
                Console.WriteLine("[AUTH WARNING] No NameIdentifier or sub claim found.");
                return Unauthorized();
            }

            if (!Guid.TryParse(userIdString, out var userId))
            {
                Console.WriteLine($"[AUTH ERROR] Could not parse userId as Guid: {userIdString}");
                return BadRequest("Invalid user identity format.");
            }

            var profile = await _profileService.GetProfileByUserIdAsync(userId);
            if (profile == null)
            {
                return NoContent();
            }

            return Ok(profile);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[PROFILE ERROR] Unexpected error in GetMyProfile: {ex.Message}");
            return StatusCode(500, new { message = "An internal error occurred retrieving your profile.", details = ex.Message });
        }
    }

    /// <summary>
    /// Creates or updates the user profile during or after onboarding.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Profile>> UpsertProfile([FromBody] ProfileDto profileDto)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized();
        }

        var profile = await _profileService.UpsertProfileAsync(userId, profileDto);
        return Ok(profile);
    }
}




