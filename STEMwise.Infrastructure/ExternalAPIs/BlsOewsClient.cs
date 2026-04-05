using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using STEMwise.Application.Interfaces;

namespace STEMwise.Infrastructure.ExternalAPIs;

public class BlsOewsClient : IBlsOewsClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public BlsOewsClient(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.bls.gov/publicAPI/v2/timeseries/data/");
        _apiKey = config["ApiKeys:BLS"] ?? throw new ArgumentNullException("BLS API Key missing");
    }

    public async Task<List<WageData>> GetWagesByOccupationAsync(string socCode, string areaCode)
    {
        string processedSoc = socCode.Replace("-", "");
        string seriesId = $"OEUM{areaCode}000000{processedSoc}04";

        var requestBody = new
        {
            seriesid = new[] { seriesId },
            registrationkey = _apiKey
        };

        var response = await _httpClient.PostAsJsonAsync("", requestBody);
        var result = await response.Content.ReadFromJsonAsync<BlsResponse>();

        if (result?.Status == "REQUEST_SUCCEEDED" && result.Results.Series.Count > 0)
        {
            return result.Results.Series[0].Data;
        }

        return new List<WageData>();
    }

    private class BlsResponse
    {
        public string Status { get; set; } = string.Empty;
        public BlsResults Results { get; set; } = new();
    }

    private class BlsResults
    {
        public List<BlsSeries> Series { get; set; } = new();
    }

    private class BlsSeries
    {
        public string SeriesID { get; set; } = string.Empty;
        public List<WageData> Data { get; set; } = new();
    }
}
