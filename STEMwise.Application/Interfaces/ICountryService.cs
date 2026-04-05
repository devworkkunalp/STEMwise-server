using System.Collections.Generic;
using System.Threading.Tasks;
using STEMwise.Domain.Entities;

namespace STEMwise.Application.Interfaces;

public interface ICountryService
{
    Task<IEnumerable<Country>> GetAllCountriesAsync();
    Task<Country?> GetCountryByCodeAsync(string code);
}
