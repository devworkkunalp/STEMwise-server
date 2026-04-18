using System;

namespace STEMwise.Domain.Entities;

public class GlobalUniversityMetric
{
    public int Id { get; set; }
    public string CountryCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int? AnnualTuition { get; set; }
    public int? MedianSalary { get; set; }
    public string Currency { get; set; } = "USD";
    public decimal? EmploymentRate { get; set; }
    public decimal? VisaSuccessRate { get; set; }
    public int? RoiScore { get; set; }
    public DateTime LastSynced { get; set; }
}
