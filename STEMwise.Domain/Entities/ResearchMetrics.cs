using System;

namespace STEMwise.Domain.Entities;

public class UniversityMetric
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int UnitId { get; set; } // Primary identifier from College Scorecard API
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZIP { get; set; } = string.Empty;
    
    // Academic ROI Aggregates
    public int? MedianEarnings { get; set; }
    public int? MedianDebt { get; set; }
    public int? AnnualTuition { get; set; }
    public decimal? GraduationRate { get; set; }
    public decimal? AdmissionRate { get; set; }
    
    // Derived Research Hub Metrics
    public int? RoiScore { get; set; }
    public decimal? EmploymentRate { get; set; }
    public decimal? IntlStudentShare { get; set; } // H-1B Proxy
    
    public DateTime LastSynced { get; set; }
}

public class RegionalRent
{
    public int Id { get; set; }
    public string RegionName { get; set; } = string.Empty;
    public string MsaId { get; set; } = string.Empty; 
    public string State { get; set; } = string.Empty;
    
    public int EfficiencyRent { get; set; }
    public int OneBedRent { get; set; }
    public int TwoBedRent { get; set; }
    
    public DateTime EffectiveYear { get; set; }
    public DateTime LastSynced { get; set; }
}

public class VisaBenchmark
{
    public int Id { get; set; }
    public string RegionName { get; set; } = string.Empty;
    public string Specialization { get; set; } = "General";
    
    public int TotalPetitions { get; set; }
    public int Approvals { get; set; }
    public int Denials { get; set; }
    public decimal SuccessRate => TotalPetitions > 0 ? (decimal)Approvals / TotalPetitions : 0;
    
    // Performance Projections (Outcome Probabilities)
    public int OutcomeEmployedPct { get; set; }
    public int OutcomeH1BPct { get; set; }
    public int OutcomeReturnedPct { get; set; }

    public int FiscalYear { get; set; }
    public DateTime LastSynced { get; set; }
}

public class LaborBenchmark
{
    public int Id { get; set; }
    public string RegionName { get; set; } = string.Empty;
    public string Specialization { get; set; } = "General";
    
    public int JobCount { get; set; }
    public int AvgSalary { get; set; }
    public int MedianSalary { get; set; }
    
    // Distribution metrics for charts
    public int Percentile10Salary { get; set; }
    public int Percentile25Salary { get; set; }
    public int Percentile75Salary { get; set; }
    public int Percentile90Salary { get; set; }
    
    public int RentMedian { get; set; } // Regional rent integration for cost-of-living analytics
    
    public DateTime LastSynced { get; set; }
}

public class GlobalSectorBenchmark
{
    public int Id { get; set; }
    public string CountryName { get; set; } = string.Empty;
    public string Flag { get; set; } = string.Empty;
    public string Specialization { get; set; } = "General";
    
    public int MedianSalary { get; set; }
    public string PrMetric { get; set; } = string.Empty; // e.g. "Hard (10+ yr)"
    public string VisaEase { get; set; } = string.Empty; // e.g. "Smooth"
    public int RoiScore { get; set; }

    public DateTime LastSynced { get; set; }
}
