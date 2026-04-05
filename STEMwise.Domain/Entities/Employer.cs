using System;
using System.Collections.Generic;

namespace STEMwise.Domain.Entities;

public class Employer : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int? H1BFilingsTotal { get; set; }
    public int? AvgSponsoredSalary { get; set; }
    public List<string> PrimaryStemFields { get; set; } = new List<string>();
    public List<string> TopCities { get; set; } = new List<string>();
    public int? SponsorScore { get; set; }
}
