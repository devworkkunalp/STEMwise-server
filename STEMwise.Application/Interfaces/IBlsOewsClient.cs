using System.Collections.Generic;
using System.Threading.Tasks;

namespace STEMwise.Application.Interfaces;

public class WageData
{
    public string Year { get; set; } = string.Empty;
    public string Period { get; set; } = string.Empty;
    public string PeriodName { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public Dictionary<string, string> Footnotes { get; set; } = new();
}

public interface IBlsOewsClient
{
    Task<List<WageData>> GetWagesByOccupationAsync(string socCode, string areaCode);
}
