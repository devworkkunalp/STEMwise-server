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
                // 20% Salary Cut + 6 Months Job Gap
                var recessionRequest = CloneRequest(baseRequest);
                recessionRequest.FinalSalaryBenchmark = (int)(baseRequest.FinalSalaryBenchmark * 0.8);
                var recessionResult = await _calculationService.CalculateROIAsync(recessionRequest);
                result.AdjustedRoi = Math.Max(5, recessionResult.ROIScore - 8); 
                result.Metrics.Add(new ScenarioMetric { Label = "Target Salary", BaseValue = $"${baseRequest.FinalSalaryBenchmark/1000}k", AdjustedValue = $"${recessionRequest.FinalSalaryBenchmark/1000}k", IsNegative = true });
                result.Metrics.Add(new ScenarioMetric { Label = "Job Search", BaseValue = "3 Months", AdjustedValue = "9 Months", IsNegative = true });
                break;

            case "H1B_DENIED":
                // Returning home after OPT
                result.AdjustedRoi = Math.Max(5, baseResult.ROIScore - 42); 
                result.Metrics.Add(new ScenarioMetric { Label = "Years in US", BaseValue = "10 Years", AdjustedValue = "3 Years (OPT)", IsNegative = true });
                result.Metrics.Add(new ScenarioMetric { Label = "Year 4-10 Earnings", BaseValue = "USD Full", AdjustedValue = "Local LPP", IsNegative = true });
                break;

            case "STUDY_DELAY":
                // Extra year of cost + 1 year delayed income
                var delayRequest = CloneRequest(baseRequest);
                delayRequest.DurationYears += 1;
                var delayResult = await _calculationService.CalculateROIAsync(delayRequest);
                result.AdjustedRoi = Math.Max(5, delayResult.ROIScore - 12);
                result.Metrics.Add(new ScenarioMetric { Label = "Program Duration", BaseValue = $"{baseRequest.DurationYears} Yrs", AdjustedValue = $"{delayRequest.DurationYears} Yrs", IsNegative = true });
                result.Metrics.Add(new ScenarioMetric { Label = "Total Investment", BaseValue = "Standard", AdjustedValue = "+$65k Est", IsNegative = true });
                break;

            case "JOB_GAP":
                // 3 month unemployment gap at start of OPT
                result.AdjustedRoi = Math.Max(5, baseResult.ROIScore - 15);
                result.Metrics.Add(new ScenarioMetric { Label = "OPT Start Date", BaseValue = "Month 1", AdjustedValue = "Month 4", IsNegative = true });
                result.Metrics.Add(new ScenarioMetric { Label = "Initial Savings", BaseValue = "$25k (1st year)", AdjustedValue = "$18k", IsNegative = true });
                break;

            case "LEVEL_3_PROMO":
                // Upside scenario
                result.AdjustedRoi = Math.Min(99, baseResult.ROIScore + 15);
                result.Metrics.Add(new ScenarioMetric { Label = "Visa Odds", BaseValue = "48%", AdjustedValue = "92% (Priority)", IsNegative = false });
                result.Metrics.Add(new ScenarioMetric { Label = "Path Stability", BaseValue = "Moderate", AdjustedValue = "High", IsNegative = false });
                break;
                
            default:
                result.AdjustedRoi = baseResult.ROIScore;
                break;
        }

        result.BaseRoi = baseResult.ROIScore; // Use score instead of percentage
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
