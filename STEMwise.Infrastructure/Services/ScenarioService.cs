using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using STEMwise.Application.Interfaces;
using STEMwise.Domain.Entities;
using STEMwise.Infrastructure.Data;

namespace STEMwise.Infrastructure.Services;

public class ScenarioService : IScenarioService
{
    private readonly AppDbContext _context;
    private readonly ICalculationService _calculationService;
    private readonly IProfileService _profileService;

    public ScenarioService(AppDbContext context, ICalculationService calculationService, IProfileService profileService)
    {
        _context = context;
        _calculationService = calculationService;
        _profileService = profileService;
    }

    public async Task<ScenarioModelResult> ModelScenarioAsync(ScenarioModelRequest request)
    {
        var profile = await _profileService.GetProfileByIdAsync(request.ProfileId);
        if (profile == null) throw new Exception("Profile not found");

        // Base Request
        var baseRequest = new ROIRequest
        {
            AnnualTuition = (int)profile.AnnualTuition,
            AnnualLivingCost = (int)profile.AnnualLivingCost,
            DurationYears = profile.ProgramDurationYears,
            FinalSalaryBenchmark = (int)profile.TargetSalary,
            CurrentSalary = 0,
            LoanAmount = profile.LoanAmount,
            InterestRate = profile.LoanInterestRate,
            HomeCurrency = string.IsNullOrEmpty(profile.HomeCurrency) ? "INR" : profile.HomeCurrency,
            TaxRate = 0.25m
        };

        var baseResult = await _calculationService.CalculateROIAsync(baseRequest);
        var result = new ScenarioModelResult
        {
            ScenarioType = request.ScenarioType,
            BaseRoi = baseResult.ROIPercentage,
            Narrative = GetNarrative(request.ScenarioType)
        };

        switch (request.ScenarioType)
        {
            case "RECESSION":
                // 20% Salary Cut + 6 Months Job Gap (Opportunity Cost of zero earnings)
                var recessionRequest = CloneRequest(baseRequest);
                recessionRequest.FinalSalaryBenchmark = (int)(baseRequest.FinalSalaryBenchmark * 0.8);
                // We simulate job gap by adding 0.5 to duration (unearned time) or just deducting 6 months earnings
                var recessionResult = await _calculationService.CalculateROIAsync(recessionRequest);
                // Manual correction for the 6-month gap impact on cumulative earnings
                result.AdjustedRoi = recessionResult.ROIPercentage - 8; // Approximation for the job gap
                result.Metrics.Add(new ScenarioMetric { Label = "Target Salary", BaseValue = $"${baseRequest.FinalSalaryBenchmark/1000}k", AdjustedValue = $"${recessionRequest.FinalSalaryBenchmark/1000}k", IsNegative = true });
                result.Metrics.Add(new ScenarioMetric { Label = "Job Search", BaseValue = "3 Months", AdjustedValue = "9 Months", IsNegative = true });
                break;

            case "H1B_DENIED":
                // 3 years US salary -> 7 years Home salary
                int homeSalaryLevel = GetHomeSalaryBenchmark(profile.Nationality);
                var deniedResult = Math.Max(0, baseResult.ROIPercentage - 40); // Standard heavy impact
                result.AdjustedRoi = deniedResult;
                result.Metrics.Add(new ScenarioMetric { Label = "Years in US", BaseValue = "10 Years", AdjustedValue = "3 Years (OPT)", IsNegative = true });
                result.Metrics.Add(new ScenarioMetric { Label = "Year 4-10 Earnings", BaseValue = $"${baseRequest.FinalSalaryBenchmark/1000}k/yr", AdjustedValue = $"₹{homeSalaryLevel/100000} LPA", IsNegative = true });
                break;

            case "CURRENCY_CRASH":
                // 20% FX spike (assuming INR or similar)
                result.AdjustedRoi = Math.Max(0, baseResult.ROIPercentage - 12);
                result.Metrics.Add(new ScenarioMetric { Label = "Exchange Rate", BaseValue = "₹83.5/$", AdjustedValue = "₹100.2/$", IsNegative = true });
                result.Metrics.Add(new ScenarioMetric { Label = "Loan Burden", BaseValue = "Standard", AdjustedValue = "+20% Cost", IsNegative = true });
                break;

            case "LEVEL_3_PROMO":
                // Boost for Level 3 prioritization
                result.AdjustedRoi = Math.Min(100, baseResult.ROIPercentage + 15);
                result.Metrics.Add(new ScenarioMetric { Label = "Visa Odds", BaseValue = "48%", AdjustedValue = "92% (Priority)", IsNegative = false });
                result.Metrics.Add(new ScenarioMetric { Label = "Path Stability", BaseValue = "Moderate", AdjustedValue = "High", IsNegative = false });
                break;
                
            default:
                result.AdjustedRoi = baseResult.ROIPercentage;
                break;
        }

        result.ImpactScore = result.AdjustedRoi - result.BaseRoi;
        result.RecommendedPivots = GetPivots(request.ScenarioType, profile.Nationality);

        return result;
    }

    public async Task<bool> SaveScenarioAsync(Guid profileId, ScenarioModelResult result)
    {
        var scenario = new SavedScenario
        {
            ProfileId = profileId,
            ScenarioType = result.ScenarioType,
            BaseRoi = (int)result.BaseRoi,
            AdjustedRoi = (int)result.AdjustedRoi,
            ImpactDelta = (int)result.ImpactScore,
            CreatedAt = DateTime.UtcNow
        };

        _context.SavedScenarios.Add(scenario);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<List<ScenarioHistoryItem>> GetScenarioHistoryAsync(Guid profileId)
    {
        return await _context.SavedScenarios
            .Where(s => s.ProfileId == profileId)
            .OrderByDescending(s => s.CreatedAt)
            .Select(s => new ScenarioHistoryItem
            {
                Id = s.Id,
                CreatedAt = s.CreatedAt,
                ScenarioType = s.ScenarioType,
                ImpactScore = s.ImpactDelta,
                AdjustedRoi = s.AdjustedRoi
            })
            .ToListAsync();
    }

    private ROIRequest CloneRequest(ROIRequest req) => new ROIRequest
    {
        AnnualTuition = req.AnnualTuition,
        AnnualLivingCost = req.AnnualLivingCost,
        DurationYears = req.DurationYears,
        FinalSalaryBenchmark = req.FinalSalaryBenchmark,
        CurrentSalary = req.CurrentSalary,
        LoanAmount = req.LoanAmount,
        InterestRate = req.InterestRate,
        HomeCurrency = req.HomeCurrency,
        TaxRate = req.TaxRate
    };

    private string GetNarrative(string type) => type switch
    {
        "RECESSION" => "A 20% salary cut and 6-month job search delay significantly increases your break-even time and debt ratio.",
        "H1B_DENIED" => "Returning home after OPT cuts your 10-year USD savings potential by over 60%. Impact is severe if debt is high.",
        "CURRENCY_CRASH" => "Local currency devaluation increases the effective principal of your US-denominated loan.",
        "LEVEL_3_PROMO" => "High-wage prioritization makes your visa outcome nearly certain, securing your long-term earnings path.",
        _ => "Scenario modeled based on current global trends."
    };

    private int GetHomeSalaryBenchmark(string? nationality) => nationality == "India" ? 2000000 : 35000;

    private List<PivotPathway> GetPivots(string type, string? nationality)
    {
        var pivots = new List<PivotPathway>();
        if (type == "H1B_DENIED" || type == "RECESSION")
        {
            pivots.Add(new PivotPathway { Name = "Canada Express Entry", ROI = "72%", RiskLevel = "Low" });
            pivots.Add(new PivotPathway { Name = "Germany Blue Card", ROI = "65%", RiskLevel = "Low" });
        }
        else
        {
            pivots.Add(new PivotPathway { Name = "O-1 Talent Visa", ROI = "88%", RiskLevel = "High Bar" });
        }
        return pivots;
    }
}
