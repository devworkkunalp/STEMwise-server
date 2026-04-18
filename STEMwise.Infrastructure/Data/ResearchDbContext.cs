using Microsoft.EntityFrameworkCore;
using STEMwise.Domain.Entities;

namespace STEMwise.Infrastructure.Data;

public class ResearchDbContext : DbContext
{
    public ResearchDbContext(DbContextOptions<ResearchDbContext> options)
        : base(options)
    {
    }

    public DbSet<UniversityMetric> UniversityMetrics { get; set; } = null!;
    public DbSet<RegionalRent> RegionalRents { get; set; } = null!;
    public DbSet<VisaBenchmark> VisaBenchmarks { get; set; } = null!;
    public DbSet<LaborBenchmark> LaborBenchmarks { get; set; } = null!;
    public DbSet<GlobalUniversityMetric> GlobalUniversityMetrics { get; set; } = null!;
    public DbSet<GlobalSectorBenchmark> GlobalSectorBenchmarks { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Map to existing tables created by the Orchestrator
        modelBuilder.Entity<UniversityMetric>().ToTable("UniversityMetrics");
        modelBuilder.Entity<RegionalRent>().ToTable("RegionalRents");
        
        modelBuilder.Entity<VisaBenchmark>().ToTable("VisaBenchmarks");
        modelBuilder.Entity<VisaBenchmark>().HasIndex(v => new { v.RegionName, v.Specialization }).IsUnique();

        modelBuilder.Entity<LaborBenchmark>().ToTable("LaborBenchmarks");
        modelBuilder.Entity<LaborBenchmark>().HasIndex(l => new { l.RegionName, l.Specialization }).IsUnique();

        modelBuilder.Entity<GlobalUniversityMetric>().ToTable("GlobalUniversityMetrics");
        
        modelBuilder.Entity<GlobalSectorBenchmark>().ToTable("GlobalSectorBenchmarks");
        modelBuilder.Entity<GlobalSectorBenchmark>().HasIndex(g => new { g.CountryName, g.Specialization }).IsUnique();
    }
}
