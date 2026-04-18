using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using STEMwise.Application.Interfaces;
using STEMwise.Domain.Entities;
using STEMwise.Infrastructure.Data;

namespace STEMwise.Infrastructure.Services;

public class ResearchService : IResearchService
{
    private readonly ResearchDbContext _context;
    private readonly IMemoryCache _cache;
    private const string CacheKeyPrefix = "ResearchData_";

    public ResearchService(ResearchDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<IEnumerable<UniversityMetric>> GetRankedUniversitiesAsync(string? sector)
    {
        var cacheKey = $"{CacheKeyPrefix}Universities_{sector ?? "All"}";
        
        if (!_cache.TryGetValue(cacheKey, out List<UniversityMetric>? universities))
        {
            universities = await _context.UniversityMetrics
                .OrderByDescending(u => u.RoiScore)
                .Take(200)
                .ToListAsync();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(1));

            _cache.Set(cacheKey, universities, cacheOptions);
        }

        return universities ?? new List<UniversityMetric>();
    }

    public async Task<IEnumerable<RegionalRent>> GetRegionalRentsAsync()
    {
        var cacheKey = $"{CacheKeyPrefix}Rents";

        if (!_cache.TryGetValue(cacheKey, out List<RegionalRent>? rents))
        {
            rents = await _context.RegionalRents.ToListAsync();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(1));

            _cache.Set(cacheKey, rents, cacheOptions);
        }

        return rents ?? new List<RegionalRent>();
    }

    public async Task<IEnumerable<VisaBenchmark>> GetVisaBenchmarksAsync()
    {
        var cacheKey = $"{CacheKeyPrefix}Visa";

        if (!_cache.TryGetValue(cacheKey, out List<VisaBenchmark>? visa))
        {
            visa = await _context.VisaBenchmarks.ToListAsync();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(1));

            _cache.Set(cacheKey, visa, cacheOptions);
        }

        return visa ?? new List<VisaBenchmark>();
    }

    public async Task<IEnumerable<LaborBenchmark>> GetLaborBenchmarksAsync()
    {
        var cacheKey = $"{CacheKeyPrefix}Labor";

        if (!_cache.TryGetValue(cacheKey, out List<LaborBenchmark>? labor))
        {
            labor = await _context.LaborBenchmarks.ToListAsync();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(1));

            _cache.Set(cacheKey, labor, cacheOptions);
        }

        return labor ?? new List<LaborBenchmark>();
    }

    public async Task<IEnumerable<GlobalUniversityMetric>> GetGlobalRankingsAsync()
    {
        var cacheKey = $"{CacheKeyPrefix}Global";

        if (!_cache.TryGetValue(cacheKey, out List<GlobalUniversityMetric>? globalData))
        {
            globalData = await _context.GlobalUniversityMetrics
                .OrderByDescending(g => g.RoiScore)
                .ToListAsync();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(1));

            _cache.Set(cacheKey, globalData, cacheOptions);
        }

        return globalData ?? new List<GlobalUniversityMetric>();
    }

    public async Task<IEnumerable<GlobalSectorBenchmark>> GetGlobalSectorBenchmarksAsync(string? specialization)
    {
        // Normalize specialization for robust mapping (Matches exact strings in GlobalSectorBenchmarks table)
        var normalizedSpec = specialization?.ToLower() switch {
            "cs" => "Computer Science / AI",
            "computer science / ai" => "Computer Science / AI",
            "cyber" => "Cybersecurity",
            "cybersecurity" => "Cybersecurity",
            "data" => "Data Science / Analytics",
            "data science / analytics" => "Data Science / Analytics",
            "electrical" => "Electrical Engineering",
            "biomedical" => "Biomedical Sciences",
            "mechanical" => "Mechanical Engineering",
            _ => specialization
        };

        var cacheKey = $"{CacheKeyPrefix}GlobalSectors_{normalizedSpec ?? "All"}";

        if (!_cache.TryGetValue(cacheKey, out List<GlobalSectorBenchmark>? globalSectors))
        {
            var query = _context.GlobalSectorBenchmarks.AsQueryable();
            
            if (!string.IsNullOrEmpty(normalizedSpec))
            {
                query = query.Where(g => g.Specialization == normalizedSpec);
            }

            globalSectors = await query.ToListAsync();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(1));

            _cache.Set(cacheKey, globalSectors, cacheOptions);
        }

        return globalSectors ?? new List<GlobalSectorBenchmark>();
    }
}
