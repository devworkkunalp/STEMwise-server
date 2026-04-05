using System;
using System.Text.Json;

namespace STEMwise.Domain.Entities;

public class SavedScenario : BaseEntity
{
    public Guid ProfileId { get; set; }
    public Profile? Profile { get; set; }
    public string ScenarioType { get; set; } = string.Empty;
    public JsonDocument? ScenarioParams { get; set; }
    public int BaseRoi { get; set; }
    public int AdjustedRoi { get; set; }
    public int ImpactDelta { get; set; }
    public JsonDocument? AlternativePaths { get; set; }
}
