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
    public decimal LoanAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int RepaymentTerm { get; set; } = 10; // Default to 10 years
    public string HomeCurrency { get; set; } = "INR";
    public string StudyCurrency { get; set; } = "USD";
    public decimal TaxRate { get; set; } = 0.25m; // 25% default
}

public class ROIResult
{
    public decimal TotalInvestment { get; set; }
    public decimal TotalDirectCost { get; set; } // Tuition + Rent + Misc
    public decimal LoanInterest { get; set; } // Estimated total interest
    public decimal NetEarnings10Yr { get; set; }
    public decimal ROIPercentage { get; set; }
    public decimal NPV { get; set; }
    public decimal BreakEvenYear { get; set; }
    public decimal OpportunityCost { get; set; }
    public int ROIScore { get; set; } // 0-100 normalized
    public decimal IncrementalEarnings { get; set; } // Annual Delta
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
    public decimal Salary { get; set; }
    public string City { get; set; } = string.Empty;
    public string FieldOfStudy { get; set; } = string.Empty;
    public bool IsStem { get; set; } // Auto-attempts calculation
}

public class VisaResult
{
    public decimal CumulativeSuccessProbability { get; set; }
    public int TotalAttempts { get; set; }
    public string RiskLevel { get; set; } = string.Empty;
    public int WageLevel { get; set; } // I-IV categorization
    public List<WageLevelData> ProbabilityMatrix { get; set; } = new();
    public string OptimizationTip { get; set; } = string.Empty;
}

public class WageLevelData
{
    public string Level { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public string Label { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsUserLevel { get; set; }
}

public class LoanRequest
{
    public decimal Principal { get; set; }
    public decimal AnnualInterestRate { get; set; }
    public int TenureYears { get; set; }
    public int GracePeriodMonths { get; set; }
    public bool IsInterestCapitalized { get; set; }
    public decimal TotalEstimatedCost { get; set; } // For Gap Detection
}

public class LoanResult
{
    public decimal MonthlyEMI { get; set; }
    public decimal TotalInterestPayable { get; set; }
    public decimal TotalAmountPayable { get; set; }
    public decimal TotalInterestAtRepayment { get; set; } // Accrued during grace
    public decimal FundingGap { get; set; }
    public bool IsGapPresent { get; set; }
    public List<AmortizationMonth> AmortizationSchedule { get; set; } = new();
}

public class AmortizationMonth
{
    public int Month { get; set; }
    public decimal PrincipalPaid { get; set; }
    public decimal InterestPaid { get; set; }
    public decimal RemainingBalance { get; set; }
}
