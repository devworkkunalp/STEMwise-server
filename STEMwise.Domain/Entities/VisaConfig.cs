using System;
using STEMwise.Domain.Enums;

namespace STEMwise.Domain.Entities;

public class VisaConfig : BaseEntity
{
    public Guid ProfileId { get; set; }
    public Profile? Profile { get; set; }
    public VisaPath VisaPath { get; set; }
    public string? TargetRole { get; set; }
    public string? TargetEmployerTier { get; set; }
    public string? TargetCity { get; set; }
    public int? ExpectedSalary { get; set; }
}
