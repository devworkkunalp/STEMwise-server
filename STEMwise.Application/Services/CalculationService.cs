using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using STEMwise.Application.Interfaces;

namespace STEMwise.Application.Services;

public class CalculationService : ICalculationService
{
    private readonly IFrankfurterClient _frankfurterClient;
    private const decimal AnnualSalaryGrowthRate = 0.03m;
    private const decimal DiscountRate = 0.07m;

    public CalculationService(IFrankfurterClient frankfurterClient)
    {
        _frankfurterClient = frankfurterClient;
    }

    public async Task<ROIResult> CalculateROIAsync(ROIRequest request)
    {
        var result = new ROIResult();
        decimal exchangeRate = 1.0m;

        // 1. Currency Conversion (e.g., USD -> INR)
        if (request.StudyCurrency != request.HomeCurrency)
        {
            try
            {
                var rateResponse = await _frankfurterClient.GetLatestRatesAsync(request.StudyCurrency, new[] { request.HomeCurrency });
                if (rateResponse != null && rateResponse.Rates.TryGetValue(request.HomeCurrency, out var rate))
                {
                    exchangeRate = rate;
                }
            }
            catch (Exception ex)
            {
                // Log error or fallback to default (1.0)
                Console.WriteLine($"Currency conversion failed: {ex.Message}");
            }
        }

        // 2. Initial Investment (Study Phase)
        decimal annualCost = (request.AnnualTuition + request.AnnualLivingCost) * exchangeRate;
        decimal totalDirectCost = annualCost * request.DurationYears;
        decimal opportunityCost = (request.CurrentSalary * exchangeRate) * request.DurationYears;
        
        // Calculate estimated loan interest (Simplified for ROI purposes: Loan * Rate * Term * 0.55 factor)
        decimal loanInterest = (request.LoanAmount * exchangeRate) * (request.InterestRate / 100) * request.RepaymentTerm * 0.55m;
        
        result.TotalInvestment = Math.Round(totalDirectCost + opportunityCost + loanInterest, 2);
        result.OpportunityCost = Math.Round(opportunityCost, 2);

        // 3. 10-Year Projections
        decimal cumulativeEarnings = 0;
        int breakEvenYear = -1;
        decimal npv = -result.TotalInvestment;
        decimal totalNetEarnings = 0;

        // Baseline (Incremental Earnings comparison)
        decimal annualBaselineNet = (request.CurrentSalary * exchangeRate) * (1 - request.TaxRate);
        decimal firstYearWorkingSalary = request.FinalSalaryBenchmark * exchangeRate * (1 - request.TaxRate);
        result.IncrementalEarnings = Math.Round(firstYearWorkingSalary - annualBaselineNet, 2);

        for (int year = 1; year <= 10; year++)
        {
            decimal yearEarnings = 0;

            if (year > request.DurationYears)
            {
                // Working Phase
                int yearsWorking = year - (int)Math.Ceiling(request.DurationYears);
                decimal baseSalary = request.FinalSalaryBenchmark * exchangeRate;
                decimal grossSalary = baseSalary * (decimal)Math.Pow(1 + (double)AnnualSalaryGrowthRate, yearsWorking);
                yearEarnings = grossSalary * (1 - request.TaxRate);
            }

            cumulativeEarnings += yearEarnings;
            
            // NPV calculation: Net Cash Flow / (1+r)^t
            npv += yearEarnings / (decimal)Math.Pow(1 + (double)DiscountRate, year);

            if (breakEvenYear == -1 && cumulativeEarnings >= result.TotalInvestment)
            {
                breakEvenYear = year;
            }

            result.CashFlows.Add(new AnnualCashFlow 
            { 
                Year = year, 
                Earnings = Math.Round(yearEarnings, 2), 
                Cumulative = Math.Round(cumulativeEarnings, 2) 
            });
        }

        result.NetEarnings10Yr = Math.Round(cumulativeEarnings, 2);
        result.NPV = Math.Round(npv, 2);
        result.ROIPercentage = Math.Round((cumulativeEarnings - result.TotalInvestment) / result.TotalInvestment * 100, 2);
        result.BreakEvenYear = breakEvenYear > 0 ? breakEvenYear : 11; // 11 means not reached in 10 yrs
        result.Currency = request.HomeCurrency;

        // 4. ROI Score Calculation (0-100 normalized)
        // Logic: Payback <= 2 yrs is 100, Payback >= 10 yrs is 20.
        // We also factor in the 10yr ROI percentage.
        decimal playbackScore = Math.Max(0, 100 - ((decimal)(result.BreakEvenYear - 2) * 10));
        decimal roiPercentScore = Math.Min(100, result.ROIPercentage / 3.0m); // 300% ROI = 100 score
        
        result.ROIScore = (int)Math.Min(100, Math.Max(5, (playbackScore * 0.6m) + (roiPercentScore * 0.4m)));

        return result;
    }

    public Task<VisaResult> CalculateVisaProbabilityAsync(VisaRequest request)
    {
        int attempts = request.IsStem ? 3 : 1;
        
        // 1. Determine Wage Level (Simplified Feb 2026 Model)
        // High Tier: SF, New York, Seattle, San Jose
        // Med Tier: Austin, Chicago, Dallas, Boston
        string city = request.City?.ToLower() ?? "";
        bool isHighTier = city.Contains("san francisco") || city.Contains("new york") || city.Contains("seattle") || city.Contains("san jose");
        bool isMedTier = city.Contains("austin") || city.Contains("chicago") || city.Contains("dallas") || city.Contains("boston");

        int level = 1;
        if (isHighTier) {
            if (request.Salary >= 165000) level = 4;
            else if (request.Salary >= 135000) level = 3;
            else if (request.Salary >= 105000) level = 2;
        } else if (isMedTier) {
            if (request.Salary >= 140000) level = 4;
            else if (request.Salary >= 115000) level = 3;
            else if (request.Salary >= 90000) level = 2;
        } else {
            if (request.Salary >= 120000) level = 4;
            else if (request.Salary >= 100000) level = 3;
            else if (request.Salary >= 75000) level = 2;
        }

        // 2. Base Selection Rates per Level (Feb 2026 Forecasts)
        double baseRate = level switch {
            4 => 0.85,
            3 => 0.58,
            2 => 0.32,
            _ => 0.12
        };

        // 3. Cumulative Probability across attempts
        double singleFailureRate = 1.0 - baseRate;
        double cumulativeFailure = Math.Pow(singleFailureRate, attempts);
        decimal successProb = (decimal)(1.0 - cumulativeFailure);

        string risk = "High";
        if (successProb > 0.75m) risk = "Low";
        else if (successProb > 0.45m) risk = "Medium";

        // 4. Optimization Tip
        string tip = level < 3 && isHighTier 
            ? $"Relocating to a Tier 2 city (e.g., Austin) with your ${request.Salary:N0} salary would likely push you to Level III, increasing your selection odds to 58% per attempt."
            : level < 2 ? "Targeting a role with a higher base salary or moving to a lower COL city is recommended to escape Level I lottery risk."
            : "Your profile is competitive. Focus on top sponsorship-ready employers.";

        return Task.FromResult(new VisaResult
        {
            CumulativeSuccessProbability = Math.Round(successProb, 4),
            TotalAttempts = attempts,
            RiskLevel = risk,
            WageLevel = level,
            OptimizationTip = tip
        });
    }

    public Task<LoanResult> CalculateLoanAmortizationAsync(LoanRequest request)
    {
        double principal = (double)request.Principal;
        double annualRate = (double)request.AnnualInterestRate / 100.0;
        double monthlyRate = annualRate / 12.0;

        // 1. Initial interest calculation during grace period (Simple interest)
        // Assume interest is accrued but not compounded during grace period
        double gracePeriodInterest = principal * (annualRate * (request.GracePeriodMonths / 12.0));
        
        // 2. Amortization after grace period
        int repaymentMonths = request.TenureYears * 12;
        
        double emi = principal * (monthlyRate * Math.Pow(1 + monthlyRate, repaymentMonths)) / (Math.Pow(1 + monthlyRate, repaymentMonths) - 1);
        
        decimal totalAmortized = (decimal)(emi * repaymentMonths);
        decimal totalInterest = (decimal)gracePeriodInterest + (totalAmortized - request.Principal);

        return Task.FromResult(new LoanResult
        {
            MonthlyEMI = Math.Round((decimal)emi, 2),
            TotalAmountPayable = Math.Round((decimal)principal + totalInterest, 2),
            TotalInterestPayable = Math.Round(totalInterest, 2)
        });
    }

    public async Task<List<ROIResult>> CompareROIAsync(IEnumerable<ROIRequest> requests)
    {
        var results = new List<ROIResult>();
        if (requests == null) return results;

        foreach (var request in requests)
        {
            results.Add(await CalculateROIAsync(request));
        }
        return results;
    }
}
