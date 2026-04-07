using System;
using System.Collections.Generic;
using STEMwise.Domain.Enums;

namespace STEMwise.Application.DTOs;

public class ProfileDto
{
    public string? DisplayName { get; set; }
    public string Nationality { get; set; } = string.Empty;
    public string HomeCurrency { get; set; } = string.Empty;
    public string StemField { get; set; } = string.Empty;
    public DegreeLevel DegreeLevel { get; set; }
    public string? IntakeTerm { get; set; }
    
    // Core Profile Settings for ROI
    public string? TargetUniversity { get; set; }
    public string? DegreeName { get; set; }
    public decimal AnnualTuition { get; set; }
    public decimal AnnualLivingCost { get; set; }
    public int ProgramDurationYears { get; set; }
    public decimal LoanAmount { get; set; }
    public decimal LoanInterestRate { get; set; }

    public List<UniversitySelectionDto> SelectedUniversities { get; set; } = new();
}

public class UniversitySelectionDto
{
    public Guid UniversityId { get; set; }
    public EnrollmentStatus Status { get; set; } // Interested, Applied, Admitted
}
