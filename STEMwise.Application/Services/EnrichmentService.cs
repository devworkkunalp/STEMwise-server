using System;
using System.Linq;
using System.Threading.Tasks;
using STEMwise.Application.Interfaces;

namespace STEMwise.Application.Services;

public class EnrichmentService : IEnrichmentService
{
    private readonly ICollegeScorecardClient _scorecardClient;
    private readonly ISalaryService _salaryService;
    private readonly IFrankfurterClient _frankfurterClient;
    private readonly ICalculationService _calculationService;
    private readonly IUniversityService _universityService;

    public EnrichmentService(
        ICollegeScorecardClient scorecardClient,
        ISalaryService salaryService,
        IFrankfurterClient frankfurterClient,
        ICalculationService calculationService,
        IUniversityService universityService)
    {
        _scorecardClient = scorecardClient;
        _salaryService = salaryService;
        _frankfurterClient = frankfurterClient;
        _calculationService = calculationService;
        _universityService = universityService;
    }

    public async Task<EnrichedProfileDto> EnrichProfileAsync(Guid userId, EnrichmentRequest request)
    {
        var result = new EnrichedProfileDto();

        // 1. School Data (College Scorecard)
        try 
        {
            var scorecardId = await _universityService.GetSchoolScorecardIdByNameAsync(request.UniversityName);
            if (scorecardId.HasValue)
            {
                var school = await _scorecardClient.GetSchoolByIdAsync(scorecardId.Value);
                if (school != null)
                {
                    result.School = new SchoolData
                    {
                        Name = school.Name,
                        Tuition = (decimal)(school.TuitionOutOfState ?? 0),
                        MedianEarnings10Yr = (decimal)(school.MedianEarnings10Yrs ?? 0),
                        CompletionRate = (double)(school.CompletionRate ?? 0)
                    };
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ENRICH ERROR] Scorecard extraction failed: {ex.Message}");
        }

        // 2. Labor Market Data (DOL / Salary Benchmarks)
        try
        {
            var salaries = await _salaryService.GetSalariesByFieldAsync(request.ProgramName, "US", request.TargetCity);
            var bench = salaries.FirstOrDefault();
            if (bench != null)
            {
                result.LaborMarket = new LaborMarketData
                {
                    BenchmarkSalary = (decimal)bench.AnnualSalary,
                    MetroArea = bench.MetroArea ?? request.TargetCity,
                    GrowthRate = 0.03m // Mocked for now
                };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ENRICH ERROR] Salary extraction failed: {ex.Message}");
        }

        // 3. Immigration Data (H-1B Statistics)
        try
        {
            var visaRequest = new VisaRequest 
            { 
                BaseSelectionRate = 0.25m, // Current historical average
                IsStem = request.ProgramName.Contains("Computer") || request.ProgramName.Contains("Science") || request.ProgramName.Contains("AI")
            };
            var visaResult = await _calculationService.CalculateVisaProbabilityAsync(visaRequest);
            
            result.Immigration = new ImmigrationData
            {
                H1BProbability = visaResult.CumulativeSuccessProbability,
                RiskLevel = visaResult.RiskLevel,
                MaxAttempts = visaResult.TotalAttempts
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ENRICH ERROR] Immigration calculation failed: {ex.Message}");
        }

        // 4. Financial Data (FX Rates)
        try
        {
            var currencyMap = new System.Collections.Generic.Dictionary<string, string>
            {
                { "India", "INR" },
                { "China", "CNY" },
                { "Brazil", "BRL" }
            };

            string targetCurrency = currencyMap.GetValueOrDefault(request.HomeCountry, "INR");
            var rates = await _frankfurterClient.GetLatestRatesAsync("USD", new[] { targetCurrency });
            
            result.Financials = new FinancialData
            {
                ExchangeRate = rates?.Rates.GetValueOrDefault(targetCurrency, 1.0m) ?? 1.0m,
                CurrencyCode = targetCurrency
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ENRICH ERROR] FX extraction failed: {ex.Message}");
        }

        return result;
    }
}
