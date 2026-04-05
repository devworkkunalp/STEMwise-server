using System.Collections.Generic;
using System.Threading.Tasks;

namespace STEMwise.Application.Interfaces;

public class FxRates
{
    public string BaseCurrency { get; set; } = "USD";
    public string Date { get; set; } = string.Empty;
    public Dictionary<string, decimal> Rates { get; set; } = new();
}

public interface IFrankfurterClient
{
    Task<FxRates> GetLatestRatesAsync(string baseCurrency = "USD", string[]? targetCurrencies = null);
}
