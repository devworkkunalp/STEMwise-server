using System.Collections.Generic;
using System.Threading.Tasks;

namespace STEMwise.Application.Interfaces;

public interface ICalculationService
{
    Task<ROIResult> CalculateROIAsync(ROIRequest request);
    Task<VisaResult> CalculateVisaProbabilityAsync(VisaRequest request);
    Task<LoanResult> CalculateLoanAmortizationAsync(LoanRequest request);
    Task<List<ROIResult>> CompareROIAsync(IEnumerable<ROIRequest> requests);
}

public class ROIRequest
{
    public int AnnualTuition { get; set; }
    public int AnnualLivingCost { get; set; }
    public decimal DurationYears { get; set; }
    public int FinalSalaryBenchmark { get; set; }
    public int CurrentSalary { get; set; } // Opportunity cost
    public string HomeCurrency { get; set; } = "INR";
    public string StudyCurrency { get; set; } = "USD";
    public decimal TaxRate { get; set; } = 0.25m; // 25% default
}

public class ROIResult
{
    public decimal TotalInvestment { get; set; }
    public decimal NetEarnings10Yr { get; set; }
    public decimal ROIPercentage { get; set; }
    public decimal NPV { get; set; }
    public int BreakEvenYear { get; set; }
    public decimal OpportunityCost { get; set; }
    public string Currency { get; set; } = "INR";
    public List<AnnualCashFlow> CashFlows { get; set; } = new();
}

public class AnnualCashFlow
{
    public int Year { get; set; }
    public decimal Earnings { get; set; }
    public decimal Cumulative { get; set; }
}

public class VisaRequest
{
    public decimal BaseSelectionRate { get; set; } // Current H-1B lottery rate
    public bool IsStem { get; set; } // If STEM, attempts = 3, else 1
}

public class VisaResult
{
    public decimal CumulativeSuccessProbability { get; set; }
    public int TotalAttempts { get; set; }
    public string RiskLevel { get; set; } = string.Empty;
}

public class LoanRequest
{
    public decimal Principal { get; set; }
    public decimal AnnualInterestRate { get; set; }
    public int TenureYears { get; set; }
    public int GracePeriodMonths { get; set; }
}

public class LoanResult
{
    public decimal MonthlyEMI { get; set; }
    public decimal TotalInterestPayable { get; set; }
    public decimal TotalAmountPayable { get; set; }
}
