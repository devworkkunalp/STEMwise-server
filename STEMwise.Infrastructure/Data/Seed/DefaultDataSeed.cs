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

    public static void SeedEmployers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employer>().HasData(
            new Employer
            {
                Id = Guid.NewGuid(),
                Name = "Google",
                H1BFilingsTotal = 12500,
                AvgSponsoredSalary = 165000,
                SponsorScore = 98,
                TopCities = new List<string> { "Mountain View", "San Francisco", "Austin", "New York" },
                PrimaryStemFields = new List<string> { "Software Engineering", "Data Science", "AI" },
                CreatedAt = DateTime.UtcNow
            },
            new Employer
            {
                Id = Guid.NewGuid(),
                Name = "Amazon",
                H1BFilingsTotal = 35000,
                AvgSponsoredSalary = 155000,
                SponsorScore = 95,
                TopCities = new List<string> { "Seattle", "Austin", "Arlington", "San Francisco" },
                PrimaryStemFields = new List<string> { "Cloud Computing", "Software Engineering", "Product Management" },
                CreatedAt = DateTime.UtcNow
            },
            new Employer
            {
                Id = Guid.NewGuid(),
                Name = "Meta",
                H1BFilingsTotal = 8500,
                AvgSponsoredSalary = 175000,
                SponsorScore = 92,
                TopCities = new List<string> { "Menlo Park", "San Francisco", "Seattle", "Austin" },
                PrimaryStemFields = new List<string> { "Product Engineering", "Data Engineering" },
                CreatedAt = DateTime.UtcNow
            },
            new Employer
            {
                Id = Guid.NewGuid(),
                Name = "Tesla",
                H1BFilingsTotal = 2100,
                AvgSponsoredSalary = 145000,
                SponsorScore = 88,
                TopCities = new List<string> { "Austin", "Fremont", "Palo Alto" },
                PrimaryStemFields = new List<string> { "Mechanical Engineering", "Software Engineering", "AI" },
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}
