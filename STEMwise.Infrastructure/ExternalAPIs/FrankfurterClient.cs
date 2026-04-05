using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using STEMwise.Application.Interfaces;

namespace STEMwise.Infrastructure.ExternalAPIs;

public class FrankfurterClient : IFrankfurterClient
{
    private readonly HttpClient _httpClient;

    public FrankfurterClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.frankfurter.dev/v2/");
    }

    public async Task<FxRates> GetLatestRatesAsync(string baseCurrency = "USD", string[]? targetCurrencies = null)
    {
        var url = $"latest?base={baseCurrency}";
        if (targetCurrencies != null && targetCurrencies.Length > 0)
        {
            url += $"&symbols={string.Join(",", targetCurrencies)}";
        }

        var response = await _httpClient.GetFromJsonAsync<FxRates>(url);
        return response ?? new FxRates();
    }
}
