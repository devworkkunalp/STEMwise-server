using System.Collections.Generic;
using System.Threading.Tasks;
using STEMwise.Domain.Entities;

namespace STEMwise.Application.Interfaces;

public interface IResearchService
{
    Task<IEnumerable<UniversityMetric>> GetRankedUniversitiesAsync(string? sector);
    Task<IEnumerable<RegionalRent>> GetRegionalRentsAsync();
    Task<IEnumerable<VisaBenchmark>> GetVisaBenchmarksAsync();
    Task<IEnumerable<LaborBenchmark>> GetLaborBenchmarksAsync();
}
