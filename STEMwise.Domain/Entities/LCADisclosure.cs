using System;
using STEMwise.Domain.Enums;

namespace STEMwise.Domain.Entities;

public class LCADisclosure : BaseEntity
{
    public string EmployerName { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string? SocCode { get; set; }
    public string WorksiteCity { get; set; } = string.Empty;
    public string WorksiteState { get; set; } = string.Empty;
    public int WageOffered { get; set; }
    public WageLevel WageLevel { get; set; }
    public string CaseStatus { get; set; } = string.Empty;
    public DateTime? DecisionDate { get; set; }
}
