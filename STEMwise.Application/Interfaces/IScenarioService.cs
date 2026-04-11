using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace STEMwise.Application.Interfaces;

public interface IScenarioService
{
    Task<ScenarioModelResult> ModelScenarioAsync(ScenarioModelRequest request);
    Task<bool> SaveScenarioAsync(Guid profileId, ScenarioModelResult result);
    Task<List<ScenarioHistoryItem>> GetScenarioHistoryAsync(Guid profileId);
}

public class ScenarioModelRequest
{
    public Guid ProfileId { get; set; }
    public string ScenarioType { get; set; } = string.Empty;
}

public class ScenarioModelResult
{
    public string ScenarioType { get; set; } = string.Empty;
    public decimal BaseRoi { get; set; }
    public decimal AdjustedRoi { get; set; }
    public decimal ImpactScore { get; set; } // Delta ROI
    public string Narrative { get; set; } = string.Empty;
    public List<ScenarioMetric> Metrics { get; set; } = new();
    public List<PivotPathway> RecommendedPivots { get; set; } = new();
}

public class ScenarioMetric
{
    public string Label { get; set; } = string.Empty;
    public string BaseValue { get; set; } = string.Empty;
    public string AdjustedValue { get; set; } = string.Empty;
    public bool IsNegative { get; set; }
}

public class PivotPathway
{
    public string Name { get; set; } = string.Empty;
    public string ROI { get; set; } = string.Empty;
    public string RiskLevel { get; set; } = string.Empty;
}

public class ScenarioHistoryItem
{
    public DateTime Date { get; set; }
    public string ScenarioType { get; set; } = string.Empty;
    public int Impact { get; set; }
}
