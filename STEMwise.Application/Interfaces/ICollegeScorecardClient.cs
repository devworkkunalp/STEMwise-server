using System.Collections.Generic;
using System.Threading.Tasks;

namespace STEMwise.Application.Interfaces;

public class ScorecardSchool
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public int? TuitionOutOfState { get; set; }
    public decimal? CompletionRate { get; set; }
    public int? MedianEarnings10Yrs { get; set; }
}

public interface ICollegeScorecardClient
{
    Task<ScorecardSchool> GetSchoolByIdAsync(int scorecardId);
    Task<List<ScorecardSchool>> SearchSchoolsAsync(string name, string? state = null);
}
