using System;
using STEMwise.Domain.Enums;

namespace STEMwise.Domain.Entities;

public class Program : BaseEntity
{
    public Guid UniversityId { get; set; }
    public University? University { get; set; }
    public string Name { get; set; } = string.Empty;
    public string StemField { get; set; } = string.Empty;
    public DegreeLevel DegreeLevel { get; set; }
    public decimal DurationYears { get; set; }
    public string? CipCode { get; set; }
    public bool StemOptEligible { get; set; } = true;
    public int? AvgStartingSalary { get; set; }
    public decimal? EmploymentRate1Yr { get; set; }
}
