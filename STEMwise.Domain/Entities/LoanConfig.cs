using System;
using STEMwise.Domain.Enums;

namespace STEMwise.Domain.Entities;

public class LoanConfig : BaseEntity
{
    public Guid ProfileId { get; set; }
    public Profile? Profile { get; set; }
    public LoanType LoanType { get; set; }
    public string? LoanName { get; set; }
    public int Amount { get; set; }
    public decimal InterestRate { get; set; }
    public int RepaymentTermYears { get; set; }
    public bool IsActive { get; set; } = true;
}
