using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using STEMwise.Application.Interfaces;
using STEMwise.Domain.Entities;

namespace STEMwise.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountryController : ControllerBase
{
    private readonly ICountryService _countryService;

    public CountryController(ICountryService countryService)
    {
        _countryService = countryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Country>>> GetAll()
    {
        var countries = await _countryService.GetAllCountriesAsync();
        return Ok(countries);
    }

    [HttpGet("{code}")]
    public async Task<ActionResult<Country>> GetByCode(string code)
    {
        var country = await _countryService.GetCountryByCodeAsync(code.ToUpper());
        if (country == null) return NotFound();
        return Ok(country);
    }
}
