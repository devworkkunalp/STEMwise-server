using System;

namespace STEMwise.Domain.Entities;

public class UserUniversity : BaseEntity
{
    public Guid ProfileId { get; set; }
    public Profile? Profile { get; set; }
    public Guid UniversityId { get; set; }
    public University? University { get; set; }
    public Guid? ProgramId { get; set; }
    public Program? Program { get; set; }
    public int? CustomTuition { get; set; }
    public int? CustomLivingCost { get; set; }
    public bool IsPrimary { get; set; } = false;
}
