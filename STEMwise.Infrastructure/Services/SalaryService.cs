using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using STEMwise.Application.Interfaces;
using STEMwise.Domain.Entities;
using STEMwise.Infrastructure.Data;

namespace STEMwise.Infrastructure.Services;

public class SalaryService : ISalaryService
{
    private readonly AppDbContext _context;

    public SalaryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SalaryBenchmark>> GetSalariesByFieldAsync(string stemField, string countryCode, string? metroArea = null)
    {
        var query = _context.SalaryBenchmarks
            .Include(s => s.Country)
            .Where(s => s.StemField == stemField && s.Country!.Code == countryCode);

        if (!string.IsNullOrEmpty(metroArea))
        {
            query = query.Where(s => s.MetroArea == metroArea);
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<H1BStatistic>> GetH1BStatisticsAsync()
    {
        return await _context.H1BStatistics
            .OrderByDescending(h => h.FiscalYear)
            .ToListAsync();
    }
}
