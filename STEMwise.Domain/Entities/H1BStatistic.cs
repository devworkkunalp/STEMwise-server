using System;
using STEMwise.Domain.Enums;

namespace STEMwise.Domain.Entities;

public class H1BStatistic : BaseEntity
{
    public int FiscalYear { get; set; }
    public WageLevel WageLevel { get; set; }
    public decimal SelectionRate { get; set; }
    public int? TotalRegistrations { get; set; }
    public int? TotalSelected { get; set; }
    public int? SalaryFloor { get; set; }
    public int? SalaryCeiling { get; set; }
}
