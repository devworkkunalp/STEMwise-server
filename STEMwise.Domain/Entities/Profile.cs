using System;
using System.Collections.Generic;
using STEMwise.Domain.Enums;

namespace STEMwise.Domain.Entities;

public class Profile : BaseEntity
{
    public Guid UserId { get; set; } // Supabase Auth User ID
    public string? DisplayName { get; set; }
    public string Nationality { get; set; } = string.Empty; // Country Code
    public string HomeCurrency { get; set; } = string.Empty;
    public string StemField { get; set; } = string.Empty;
    public DegreeLevel DegreeLevel { get; set; }
    public string? IntakeTerm { get; set; }
    
    // Career & Simulation Targets (TS 03/04/05 Support)
    public string? TargetCity { get; set; }
    public decimal TargetSalary { get; set; }
    public string? Specialization { get; set; }
    
    // Core ROI Simulation Baseline
    public string? TargetUniversity { get; set; }
    public string? DegreeName { get; set; }
    public decimal AnnualTuition { get; set; }
    public decimal AnnualLivingCost { get; set; }
    public int ProgramDurationYears { get; set; }
    public decimal LoanAmount { get; set; }
    public decimal LoanInterestRate { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<UserUniversity> UserUniversities { get; set; } = new List<UserUniversity>();
    public ICollection<LoanConfig> LoanConfigs { get; set; } = new List<LoanConfig>();
    public ICollection<VisaConfig> VisaConfigs { get; set; } = new List<VisaConfig>();
}
