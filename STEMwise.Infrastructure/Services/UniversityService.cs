using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using STEMwise.Application.Interfaces;
using STEMwise.Infrastructure.Data;

namespace STEMwise.Infrastructure.Services;

public class UniversityService : IUniversityService
{
    private readonly AppDbContext _context;

    public UniversityService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UniversitySearchResult>> SearchUniversitiesAsync(string query, string? countryCode = null)
    {
        var dbQuery = _context.Universities
            .Include(u => u.Country)
            .AsQueryable();

        if (!string.IsNullOrEmpty(query))
        {
            dbQuery = dbQuery.Where(u => u.Name.Contains(query));
        }

        if (!string.IsNullOrEmpty(countryCode))
        {
            dbQuery = dbQuery.Where(u => u.Country!.Code == countryCode);
        }

        return await dbQuery
            .Select(u => new UniversitySearchResult
            {
                Id = u.Id,
                Name = u.Name,
                City = u.City,
                CountryName = u.Country != null ? u.Country.Name : string.Empty,
                RankingTier = u.RankingTier ?? string.Empty
            })
            .ToListAsync();
    }

    public async Task<int?> GetSchoolScorecardIdAsync(Guid universityId)
    {
        var university = await _context.Universities.FindAsync(universityId);
        return university?.ScorecardId;
    }
}
