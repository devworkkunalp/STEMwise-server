using System.Collections.Generic;
using System.Threading.Tasks;
using STEMwise.Domain.Entities;

namespace STEMwise.Application.Interfaces;

public interface ISalaryService
{
    Task<IEnumerable<SalaryBenchmark>> GetSalariesByFieldAsync(string stemField, string countryCode, string? metroArea = null);
    Task<IEnumerable<H1BStatistic>> GetH1BStatisticsAsync();
    Task<IEnumerable<Employer>> GetTopSponsorsAsync(string? metroArea = null);
}
