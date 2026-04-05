using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STEMwise.Application.Interfaces;
using STEMwise.Domain.Entities;

namespace STEMwise.API.Controllers;

/// <summary>
/// Provides access to country-specific immigration and economic data.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CountryController : ControllerBase
{
    private readonly ICountryService _countryService;

    public CountryController(ICountryService countryService)
    {
        _countryService = countryService;
    }

    /// <summary>
    /// Retrieves all supported countries and their visa/PR profiles.
    /// </summary>
    /// <remarks>
    /// **Key Points:**
    /// - Includes PR difficulty and work visa risk ratings.
    /// - Returns base currency and flag emoji for UI rendering.
    /// </remarks>
    /// <returns>A list of countries.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Country>>> GetAll()
    {
        var countries = await _countryService.GetAllCountriesAsync();
        return Ok(countries);
    }

    /// <summary>
    /// Retrieves a specific country by its ISO 3166-1 alpha-2 code (e.g., 'US', 'GB').
    /// </summary>
    /// <param name="code">The 2-letter country code.</param>
    /// <returns>The country data if found.</returns>
    [HttpGet("{code}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Country>> GetByCode(string code)
    {
        var country = await _countryService.GetCountryByCodeAsync(code.ToUpper());
        if (country == null) return NotFound();
        return Ok(country);
    }
}
