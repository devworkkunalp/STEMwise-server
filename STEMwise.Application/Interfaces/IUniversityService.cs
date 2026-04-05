using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace STEMwise.Application.Interfaces;

public class UniversitySearchResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string CountryName { get; set; } = string.Empty;
    public string RankingTier { get; set; } = string.Empty;
}

public interface IUniversityService
{
    Task<IEnumerable<UniversitySearchResult>> SearchUniversitiesAsync(string query, string? countryCode = null);
    Task<int?> GetSchoolScorecardIdAsync(Guid universityId);
}
