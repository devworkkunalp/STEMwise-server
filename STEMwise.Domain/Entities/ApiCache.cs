using System;

namespace STEMwise.Domain.Entities;

public class ApiCache : BaseEntity
{
    public string ApiSource { get; set; } = string.Empty;
    public string CacheKey { get; set; } = string.Empty;
    public string ResponseData { get; set; } = string.Empty;
    public DateTime FetchedAt { get; set; } = DateTime.UtcNow;
    public int TtlHours { get; set; }
}
