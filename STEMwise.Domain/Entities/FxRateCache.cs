using System;

namespace STEMwise.Domain.Entities;

public class FxRateCache : BaseEntity
{
    public string BaseCurrency { get; set; } = "USD";
    public string TargetCurrency { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public DateTime FetchedAt { get; set; } = DateTime.UtcNow;
}
