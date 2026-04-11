using System;
using System.Threading.Tasks;
using STEMwise.Application.DTOs;

namespace STEMwise.Application.Interfaces;

public interface IEnrichmentService
{
    Task<EnrichedProfileDto> EnrichProfileAsync(Guid userId, EnrichmentRequest request);
}

public class EnrichmentRequest
{
    public string UniversityName { get; set; } = string.Empty;
    public string ProgramName { get; set; } = string.Empty;
    public string TargetRole { get; set; } = string.Empty;
    public string TargetCity { get; set; } = string.Empty;
    public string HomeCountry { get; set; } = "India";
}

public class EnrichedProfileDto
{
    public SchoolData School { get; set; } = new();
    public LaborMarketData LaborMarket { get; set; } = new();
    public ImmigrationData Immigration { get; set; } = new();
    public FinancialData Financials { get; set; } = new();
}

public class SchoolData
{
    public string Name { get; set; } = string.Empty;
    public decimal Tuition { get; set; }
    public decimal MedianEarnings10Yr { get; set; }
    public double CompletionRate { get; set; }
}

public class LaborMarketData
{
    public decimal BenchmarkSalary { get; set; }
    public string MetroArea { get; set; } = string.Empty;
    public decimal GrowthRate { get; set; }
}

public class ImmigrationData
{
    public decimal H1BProbability { get; set; }
    public string RiskLevel { get; set; } = string.Empty;
    public int MaxAttempts { get; set; }
}

public class FinancialData
{
    public decimal ExchangeRate { get; set; }
    public string CurrencyCode { get; set; } = string.Empty;
}
