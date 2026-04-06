using System.Collections.Generic;
using System.Threading.Tasks;

namespace STEMwise.Application.Interfaces;

public class FxRates
{
    [System.Text.Json.Serialization.JsonPropertyName("base")]
    public string BaseCurrency { get; set; } = "USD";

    [System.Text.Json.Serialization.JsonPropertyName("date")]
    public string Date { get; set; } = string.Empty;

    [System.Text.Json.Serialization.JsonPropertyName("rates")]
    public Dictionary<string, decimal> Rates { get; set; } = new();
}

public interface IFrankfurterClient
{
    Task<FxRates> GetLatestRatesAsync(string baseCurrency = "USD", string[]? targetCurrencies = null);
}
