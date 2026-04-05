using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using STEMwise.Application.Interfaces;

namespace STEMwise.Infrastructure.ExternalAPIs;

public class CollegeScorecardClient : ICollegeScorecardClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public CollegeScorecardClient(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.data.gov/ed/collegescorecard/v1/");
        _apiKey = config["ApiKeys:CollegeScorecard"] ?? throw new ArgumentNullException("Scorecard API Key missing");
    }

    public async Task<ScorecardSchool> GetSchoolByIdAsync(int scorecardId)
    {
        var response = await _httpClient.GetFromJsonAsync<ScorecardResponse>(
            $"schools?api_key={_apiKey}&id={scorecardId}&fields=id,school.name,school.city,school.state,latest.cost.tuition.out_of_state,latest.completion.rate_suppressed.overall,latest.earnings.10_yrs_after_entry.median");

        return response?.Results?.FirstOrDefault() ?? new ScorecardSchool();
    }

    public async Task<List<ScorecardSchool>> SearchSchoolsAsync(string name, string? state = null)
    {
        var url = $"schools?api_key={_apiKey}&school.name={Uri.EscapeDataString(name)}&fields=id,school.name,school.city,school.state,latest.cost.tuition.out_of_state,latest.completion.rate_suppressed.overall,latest.earnings.10_yrs_after_entry.median&per_page=10";
        if (!string.IsNullOrEmpty(state)) url += $"&school.state={state}";

        var response = await _httpClient.GetFromJsonAsync<ScorecardResponse>(url);
        return response?.Results ?? new List<ScorecardSchool>();
    }

    private class ScorecardResponse
    {
        public List<ScorecardSchool> Results { get; set; } = new();
    }
}
