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
    public List<UniversitySelectionDto> SelectedUniversities { get; set; } = new();
}

public class UniversitySelectionDto
{
    public Guid UniversityId { get; set; }
    public EnrollmentStatus Status { get; set; } // Interested, Applied, Admitted
}
