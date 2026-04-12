using System;
using System.Text.Json;
using STEMwise.Domain.Enums;

namespace STEMwise.Domain.Entities;

public class ROIReport : BaseEntity
{
    public Guid ProfileId { get; set; }
    public Profile? Profile { get; set; }
    public int RoiScore { get; set; }
    public ROILabel RoiLabel { get; set; }
    public int TotalCostUsd { get; set; }
    public decimal TotalCostHome { get; set; }
    public decimal PaybackPeriodYears { get; set; }
    public decimal H1BProbability { get; set; }
    public WageLevel? H1BWageLevel { get; set; }
    public int OptAnnualSalary { get; set; }
    public int BreakevenSalary { get; set; }
    public decimal EarningsPremiumPct { get; set; }
    public decimal DebtToIncomeRatio { get; set; }
    public string[] OptimizationTips { get; set; } = Array.Empty<string>();
    public string? InputSnapshot { get; set; }
}
