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
        
        if (result.TotalInvestment != 0)
        {
            result.ROIPercentage = Math.Round((cumulativeEarnings - result.TotalInvestment) / result.TotalInvestment * 100, 2);
        }
        else
        {
            result.ROIPercentage = 0;
        }

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

    public async Task<VisaResult> CalculateVisaProbabilityAsync(VisaRequest request)
    {
        // 1. Determine Wage Level based on City Tiers (Feb 2026 Forecast)
        var highTierCities = new[] { "San Francisco", "San Jose", "New York", "Seattle", "Palo Alto" };
        var medTierCities = new[] { "Austin", "Chicago", "Dallas", "Boston", "Atlanta", "Los Angeles" };

        int wageLevel = 1;
        if (highTierCities.Any(c => request.City.Contains(c, StringComparison.OrdinalIgnoreCase)))
        {
            if (request.Salary >= 155000) wageLevel = 4;
            else if (request.Salary >= 125000) wageLevel = 3;
            else if (request.Salary >= 95000) wageLevel = 2;
        }
        else if (medTierCities.Any(c => request.City.Contains(c, StringComparison.OrdinalIgnoreCase)))
        {
            if (request.Salary >= 135000) wageLevel = 4;
            else if (request.Salary >= 105000) wageLevel = 3;
            else if (request.Salary >= 75000) wageLevel = 2;
        }
        else
        {
            if (request.Salary >= 125000) wageLevel = 4;
            else if (request.Salary >= 95000) wageLevel = 3;
            else if (request.Salary >= 65000) wageLevel = 2;
        }

        // 2. Map Probabilities (Feb 2026 Model)
        var levels = new Dictionary<int, decimal>
        {
            { 1, 0.15m },
            { 2, 0.48m },
            { 3, 0.61m },
            { 4, 0.78m }
        };

        decimal baseRate = levels.GetValueOrDefault(wageLevel, 0.15m);
        int attempts = request.IsStem ? 3 : 1;
        decimal cumulativeProb = 1 - (decimal)Math.Pow((double)(1 - baseRate), attempts);

        // 3. Generate Probability Matrix for Frontend
        var matrix = new List<WageLevelData>
        {
            new WageLevelData { Level = "Level I", Rate = 15, Label = "Entry Level", Description = "Lowest selection probability. High wage-based risk.", IsUserLevel = wageLevel == 1 },
            new WageLevelData { Level = "Level II", Rate = 48, Label = "Qualified", Description = "Standard selection rate for most new grads in Med/High COL cities.", IsUserLevel = wageLevel == 2 },
            new WageLevelData { Level = "Level III", Rate = 61, Label = "Experienced", Description = "Significantly higher selection odds via wage-based selection.", IsUserLevel = wageLevel == 3 },
            new WageLevelData { Level = "Level IV", Rate = 78, Label = "Fully Competent", Description = "Highest priority. Near certain selection in current climate.", IsUserLevel = wageLevel == 4 }
        };

        // 4. Determine Risk Level & Optimization Tip
        string riskLevel = cumulativeProb > 0.85m ? "Low" : (cumulativeProb > 0.5m ? "Medium" : "High");
        
        string optimizationTip = "";
        if (wageLevel < 3 && medTierCities.Any(c => request.City.Contains(c, StringComparison.OrdinalIgnoreCase)))
        {
            optimizationTip = $"Moving to a lower COLA city while maintaining this salary could jump you to Level III (61% individual odds).";
        }
        else if (wageLevel < 2)
        {
            optimizationTip = $"Your current salary is classified as Level I. Targeting roles with >$95k salary in your current city would triple your primary selection odds.";
        }
        else
        {
            optimizationTip = "You are in a strong wage bracket. Focus on employer stability and Eb-2/3 initiation timelines early.";
        }

        return new VisaResult
        {
            CumulativeSuccessProbability = Math.Round(cumulativeProb, 4),
            TotalAttempts = attempts,
            RiskLevel = riskLevel,
            WageLevel = wageLevel,
            ProbabilityMatrix = matrix,
            OptimizationTip = optimizationTip
        };
    }

    public Task<LoanResult> CalculateLoanAmortizationAsync(LoanRequest request)
    {
        double principal = (double)request.Principal;
        double annualRate = (double)request.AnnualInterestRate / 100.0;
        double monthlyRate = annualRate / 12.0;
        int graceMonths = request.GracePeriodMonths;
        int repaymentMonths = request.TenureYears * 12;

        // 1. Interest during Grace Period (Simple Interest)
        double totalGraceInterest = principal * (annualRate * (graceMonths / 12.0));
        
        // 2. Adjust Principal if Capitalized
        double principalAtRepayment = request.IsInterestCapitalized ? (principal + totalGraceInterest) : principal;
        
        // 3. Calculate EMI (Standard Amortization Formula)
        double emi = 0;
        if (repaymentMonths > 0)
        {
            if (monthlyRate > 0)
            {
                double factor = Math.Pow(1 + monthlyRate, repaymentMonths);
                emi = principalAtRepayment * (monthlyRate * factor) / (factor - 1);
            }
            else
            {
                emi = principalAtRepayment / repaymentMonths;
            }
        }

        // 4. Generate Schedule
        var schedule = new List<AmortizationMonth>();
        double currentBalance = principalAtRepayment;
        double totalInterestPaidDuringRepayment = 0;

        for (int m = 1; m <= repaymentMonths; m++)
        {
            double interestM = currentBalance * monthlyRate;
            double principalM = emi - interestM;
            currentBalance = Math.Max(0, currentBalance - principalM);
            totalInterestPaidDuringRepayment += interestM;

            // Sample every 6 months to keep DTO size manageable
            if (m % 6 == 0 || m == 1 || m == repaymentMonths)
            {
                schedule.Add(new AmortizationMonth
                {
                    Month = m,
                    PrincipalPaid = (decimal)Math.Round(principalM, 2),
                    InterestPaid = (decimal)Math.Round(interestM, 2),
                    RemainingBalance = (decimal)Math.Round(currentBalance, 2)
                });
            }
        }

        // 5. Gap Detection
        decimal gap = Math.Max(0, request.TotalEstimatedCost - request.Principal);

        return Task.FromResult(new LoanResult
        {
            MonthlyEMI = Math.Round((decimal)emi, 2),
            TotalInterestAtRepayment = (decimal)Math.Round(totalGraceInterest, 2),
            TotalInterestPayable = (decimal)Math.Round(totalInterestPaidDuringRepayment + (request.IsInterestCapitalized ? totalGraceInterest : totalGraceInterest), 2),
            TotalAmountPayable = (decimal)Math.Round(principal + totalInterestPaidDuringRepayment + totalGraceInterest, 2),
            FundingGap = gap,
            IsGapPresent = gap > 10,
            AmortizationSchedule = schedule
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
