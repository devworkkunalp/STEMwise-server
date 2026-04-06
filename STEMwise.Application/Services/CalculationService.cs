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
        
        result.TotalInvestment = totalDirectCost + opportunityCost;
        result.OpportunityCost = opportunityCost;

        // 3. 10-Year Projections
        decimal cumulativeEarnings = 0;
        int breakEvenYear = -1;
        decimal npv = -result.TotalInvestment;

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
        result.BreakEvenYear = breakEvenYear;
        result.Currency = request.HomeCurrency;

        return result;
    }

    public Task<VisaResult> CalculateVisaProbabilityAsync(VisaRequest request)
    {
        int attempts = request.IsStem ? 3 : 1;
        double singleFailureRate = 1.0 - (double)request.BaseSelectionRate;
        double cumulativeFailure = Math.Pow(singleFailureRate, attempts);
        decimal successProb = (decimal)(1.0 - cumulativeFailure);

        string risk = "High";
        if (successProb > 0.7m) risk = "Low";
        else if (successProb > 0.4m) risk = "Medium";

        return Task.FromResult(new VisaResult
        {
            CumulativeSuccessProbability = Math.Round(successProb, 4),
            TotalAttempts = attempts,
            RiskLevel = risk
        });
    }

    public Task<LoanResult> CalculateLoanAmortizationAsync(LoanRequest request)
    {
        // P * (r(1+r)^n) / ((1+r)^n - 1)
        double principal = (double)request.Principal;
        double monthlyRate = (double)request.AnnualInterestRate / 12.0 / 100.0;
        int totalMonths = request.TenureYears * 12;

        double emi = principal * (monthlyRate * Math.Pow(1 + monthlyRate, totalMonths)) / (Math.Pow(1 + monthlyRate, totalMonths) - 1);
        
        decimal totalPayable = (decimal)(emi * totalMonths);

        return Task.FromResult(new LoanResult
        {
            MonthlyEMI = Math.Round((decimal)emi, 2),
            TotalAmountPayable = Math.Round(totalPayable, 2),
            TotalInterestPayable = Math.Round(totalPayable - request.Principal, 2)
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
