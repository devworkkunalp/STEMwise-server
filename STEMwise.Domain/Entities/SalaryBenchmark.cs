using System;
using STEMwise.Domain.Enums;

namespace STEMwise.Domain.Entities;

public class SalaryBenchmark : BaseEntity
{
    public Guid CountryId { get; set; }
    public Country? Country { get; set; }
    public string StemField { get; set; } = string.Empty;
    public string? MetroArea { get; set; }
    public WageLevel WageLevel { get; set; }
    public int AnnualSalary { get; set; }
    public int? Percentile25 { get; set; }
    public int? Percentile50 { get; set; }
    public int? Percentile75 { get; set; }
    public string? Source { get; set; }
    public string? SocCode { get; set; }
}
