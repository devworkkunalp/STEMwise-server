using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using STEMwise.Application.Interfaces;
using STEMwise.Domain.Entities;
using STEMwise.Infrastructure.Data;

namespace STEMwise.Infrastructure.Services;

public class CountryService : ICountryService
{
    private readonly AppDbContext _context;

    public CountryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Country>> GetAllCountriesAsync()
    {
        return await _context.Countries.ToListAsync();
    }

    public async Task<Country?> GetCountryByCodeAsync(string code)
    {
        return await _context.Countries.FirstOrDefaultAsync(c => c.Code == code);
    }
}
