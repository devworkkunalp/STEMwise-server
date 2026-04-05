using System;
using Microsoft.EntityFrameworkCore;
using STEMwise.Domain.Entities;

namespace STEMwise.Infrastructure.Data.Seed;

public static class DefaultDataSeed
{
    public static void SeedCountries(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>().HasData(
            new Country
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Code = "US",
                Name = "United States",
                CurrencyCode = "USD",
                FlagEmoji = "🇺🇸",
                PostStudyVisaName = "STEM OPT",
                PostStudyVisaMonths = 36,
                PrPathway = "H-1B \u2192 EB-2/3",
                PrDifficulty = "High",
                WorkVisaRisk = "High",
                LanguageBarrier = "None",
                IntlEmploymentRate = 85.5m
            },
            new Country
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Code = "GB",
                Name = "United Kingdom",
                CurrencyCode = "GBP",
                FlagEmoji = "🇬🇧",
                PostStudyVisaName = "Graduate Route",
                PostStudyVisaMonths = 24,
                PrPathway = "Skilled Worker \u2192 ILR",
                PrDifficulty = "Medium",
                WorkVisaRisk = "Medium",
                LanguageBarrier = "None",
                IntlEmploymentRate = 78.2m
            }
        );
    }
}
