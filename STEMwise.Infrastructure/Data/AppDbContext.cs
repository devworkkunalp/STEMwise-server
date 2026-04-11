using Microsoft.EntityFrameworkCore;
using STEMwise.Domain.Entities;

namespace STEMwise.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Country> Countries { get; set; } = null!;
    public DbSet<University> Universities { get; set; } = null!;
    public DbSet<Program> Programs { get; set; } = null!;
    public DbSet<SalaryBenchmark> SalaryBenchmarks { get; set; } = null!;
    public DbSet<H1BStatistic> H1BStatistics { get; set; } = null!;
    public DbSet<Employer> Employers { get; set; } = null!;
    public DbSet<LCADisclosure> LCADisclosures { get; set; } = null!;
    public DbSet<Profile> Profiles { get; set; } = null!;
    public DbSet<UserUniversity> UserUniversities { get; set; } = null!;
    public DbSet<LoanConfig> LoanConfigs { get; set; } = null!;
    public DbSet<VisaConfig> VisaConfigs { get; set; } = null!;
    public DbSet<ROIReport> ROIReports { get; set; } = null!;
    public DbSet<SavedScenario> SavedScenarios { get; set; } = null!;
    public DbSet<FxRateCache> FxRateCaches { get; set; } = null!;
    public DbSet<ApiCache> ApiCaches { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Country>().HasIndex(c => c.Code).IsUnique();
        modelBuilder.Entity<FxRateCache>().HasIndex(f => new { f.BaseCurrency, f.TargetCurrency }).IsUnique();
        
        Seed.DefaultDataSeed.SeedCountries(modelBuilder);
        Seed.DefaultDataSeed.SeedEmployers(modelBuilder);
    }
}
