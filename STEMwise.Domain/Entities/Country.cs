using System.Collections.Generic;

namespace STEMwise.Domain.Entities;

public class Country : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string CurrencyCode { get; set; } = string.Empty;
    public string FlagEmoji { get; set; } = string.Empty;
    public string? PostStudyVisaName { get; set; }
    public int? PostStudyVisaMonths { get; set; }
    public string? PrPathway { get; set; }
    public string? PrDifficulty { get; set; }
    public string? WorkVisaRisk { get; set; }
    public string? LanguageBarrier { get; set; }
    public decimal? IntlEmploymentRate { get; set; }

    public ICollection<University> Universities { get; set; } = new List<University>();
}
