using System;
using System.Collections.Generic;

namespace STEMwise.Domain.Entities;

public class University : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public Guid CountryId { get; set; }
    public Country? Country { get; set; }
    public string City { get; set; } = string.Empty;
    public string? StateProvince { get; set; }
    public int? ScorecardId { get; set; }
    public int AnnualTuitionIntl { get; set; }
    public int AnnualLivingCost { get; set; }
    public string? RankingTier { get; set; }

    public ICollection<Program> Programs { get; set; } = new List<Program>();
}
