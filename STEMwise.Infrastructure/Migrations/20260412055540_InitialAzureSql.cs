using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace STEMwise.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialAzureSql : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiCaches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApiSource = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CacheKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FetchedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TtlHours = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiCaches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlagEmoji = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostStudyVisaName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostStudyVisaMonths = table.Column<int>(type: "int", nullable: true),
                    PrPathway = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrDifficulty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkVisaRisk = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageBarrier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntlEmploymentRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    H1BFilingsTotal = table.Column<int>(type: "int", nullable: true),
                    AvgSponsoredSalary = table.Column<int>(type: "int", nullable: true),
                    PrimaryStemFields = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TopCities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SponsorScore = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FxRateCaches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BaseCurrency = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TargetCurrency = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FetchedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FxRateCaches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "H1BStatistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FiscalYear = table.Column<int>(type: "int", nullable: false),
                    WageLevel = table.Column<int>(type: "int", nullable: false),
                    SelectionRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalRegistrations = table.Column<int>(type: "int", nullable: true),
                    TotalSelected = table.Column<int>(type: "int", nullable: true),
                    SalaryFloor = table.Column<int>(type: "int", nullable: true),
                    SalaryCeiling = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_H1BStatistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LCADisclosures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SocCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorksiteCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorksiteState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WageOffered = table.Column<int>(type: "int", nullable: false),
                    WageLevel = table.Column<int>(type: "int", nullable: false),
                    CaseStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DecisionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LCADisclosures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StemField = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DegreeLevel = table.Column<int>(type: "int", nullable: false),
                    IntakeTerm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetCity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetUniversity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DegreeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnnualTuition = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AnnualLivingCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProgramDurationYears = table.Column<int>(type: "int", nullable: false),
                    LoanAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LoanInterestRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalaryBenchmarks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StemField = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MetroArea = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WageLevel = table.Column<int>(type: "int", nullable: false),
                    AnnualSalary = table.Column<int>(type: "int", nullable: false),
                    Percentile25 = table.Column<int>(type: "int", nullable: true),
                    Percentile50 = table.Column<int>(type: "int", nullable: true),
                    Percentile75 = table.Column<int>(type: "int", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SocCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryBenchmarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryBenchmarks_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Universities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StateProvince = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScorecardId = table.Column<int>(type: "int", nullable: true),
                    AnnualTuitionIntl = table.Column<int>(type: "int", nullable: false),
                    AnnualLivingCost = table.Column<int>(type: "int", nullable: false),
                    RankingTier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Universities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Universities_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoanConfigs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoanType = table.Column<int>(type: "int", nullable: false),
                    LoanName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    InterestRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RepaymentTermYears = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanConfigs_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ROIReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoiScore = table.Column<int>(type: "int", nullable: false),
                    RoiLabel = table.Column<int>(type: "int", nullable: false),
                    TotalCostUsd = table.Column<int>(type: "int", nullable: false),
                    TotalCostHome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaybackPeriodYears = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    H1BProbability = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    H1BWageLevel = table.Column<int>(type: "int", nullable: true),
                    OptAnnualSalary = table.Column<int>(type: "int", nullable: false),
                    BreakevenSalary = table.Column<int>(type: "int", nullable: false),
                    EarningsPremiumPct = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DebtToIncomeRatio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OptimizationTips = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InputSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROIReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ROIReports_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SavedScenarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScenarioType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScenarioParams = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseRoi = table.Column<int>(type: "int", nullable: false),
                    AdjustedRoi = table.Column<int>(type: "int", nullable: false),
                    ImpactDelta = table.Column<int>(type: "int", nullable: false),
                    AlternativePaths = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedScenarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedScenarios_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VisaConfigs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VisaPath = table.Column<int>(type: "int", nullable: false),
                    TargetRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetEmployerTier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetCity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpectedSalary = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisaConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisaConfigs_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Programs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UniversityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StemField = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DegreeLevel = table.Column<int>(type: "int", nullable: false),
                    DurationYears = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StemOptEligible = table.Column<bool>(type: "bit", nullable: false),
                    AvgStartingSalary = table.Column<int>(type: "int", nullable: true),
                    EmploymentRate1Yr = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Programs_Universities_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "Universities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserUniversities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UniversityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProgramId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomTuition = table.Column<int>(type: "int", nullable: true),
                    CustomLivingCost = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserUniversities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserUniversities_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserUniversities_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserUniversities_Universities_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "Universities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Code", "CreatedAt", "CurrencyCode", "FlagEmoji", "IntlEmploymentRate", "LanguageBarrier", "Name", "PostStudyVisaMonths", "PostStudyVisaName", "PrDifficulty", "PrPathway", "WorkVisaRisk" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "US", new DateTime(2026, 4, 12, 5, 55, 38, 722, DateTimeKind.Utc).AddTicks(7843), "USD", "🇺🇸", 85.5m, "None", "United States", 36, "STEM OPT", "High", "H-1B → EB-2/3", "High" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "GB", new DateTime(2026, 4, 12, 5, 55, 38, 723, DateTimeKind.Utc).AddTicks(7244), "GBP", "🇬🇧", 78.2m, "None", "United Kingdom", 24, "Graduate Route", "Medium", "Skilled Worker → ILR", "Medium" }
                });

            migrationBuilder.InsertData(
                table: "Employers",
                columns: new[] { "Id", "AvgSponsoredSalary", "CreatedAt", "H1BFilingsTotal", "Name", "PrimaryStemFields", "SponsorScore", "TopCities" },
                values: new object[,]
                {
                    { new Guid("426887e3-d5c5-40ff-8f56-336fb3f1a1bc"), 155000, new DateTime(2026, 4, 12, 5, 55, 38, 726, DateTimeKind.Utc).AddTicks(6969), 35000, "Amazon", "[\"Cloud Computing\",\"Software Engineering\",\"Product Management\"]", 95, "[\"Seattle\",\"Austin\",\"Arlington\",\"San Francisco\"]" },
                    { new Guid("42f0c79c-37ed-4260-bff6-9686c4d34665"), 145000, new DateTime(2026, 4, 12, 5, 55, 38, 726, DateTimeKind.Utc).AddTicks(7004), 2100, "Tesla", "[\"Mechanical Engineering\",\"Software Engineering\",\"AI\"]", 88, "[\"Austin\",\"Fremont\",\"Palo Alto\"]" },
                    { new Guid("5af3f54b-832b-47b2-b2ab-a35814f69c99"), 165000, new DateTime(2026, 4, 12, 5, 55, 38, 726, DateTimeKind.Utc).AddTicks(6175), 12500, "Google", "[\"Software Engineering\",\"Data Science\",\"AI\"]", 98, "[\"Mountain View\",\"San Francisco\",\"Austin\",\"New York\"]" },
                    { new Guid("d187a922-f9c8-4b31-aaee-dc81be7d276c"), 175000, new DateTime(2026, 4, 12, 5, 55, 38, 726, DateTimeKind.Utc).AddTicks(6993), 8500, "Meta", "[\"Product Engineering\",\"Data Engineering\"]", 92, "[\"Menlo Park\",\"San Francisco\",\"Seattle\",\"Austin\"]" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Code",
                table: "Countries",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FxRateCaches_BaseCurrency_TargetCurrency",
                table: "FxRateCaches",
                columns: new[] { "BaseCurrency", "TargetCurrency" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoanConfigs_ProfileId",
                table: "LoanConfigs",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_UniversityId",
                table: "Programs",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_ROIReports_ProfileId",
                table: "ROIReports",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryBenchmarks_CountryId",
                table: "SalaryBenchmarks",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedScenarios_ProfileId",
                table: "SavedScenarios",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Universities_CountryId",
                table: "Universities",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserUniversities_ProfileId",
                table: "UserUniversities",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserUniversities_ProgramId",
                table: "UserUniversities",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_UserUniversities_UniversityId",
                table: "UserUniversities",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_VisaConfigs_ProfileId",
                table: "VisaConfigs",
                column: "ProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiCaches");

            migrationBuilder.DropTable(
                name: "Employers");

            migrationBuilder.DropTable(
                name: "FxRateCaches");

            migrationBuilder.DropTable(
                name: "H1BStatistics");

            migrationBuilder.DropTable(
                name: "LCADisclosures");

            migrationBuilder.DropTable(
                name: "LoanConfigs");

            migrationBuilder.DropTable(
                name: "ROIReports");

            migrationBuilder.DropTable(
                name: "SalaryBenchmarks");

            migrationBuilder.DropTable(
                name: "SavedScenarios");

            migrationBuilder.DropTable(
                name: "UserUniversities");

            migrationBuilder.DropTable(
                name: "VisaConfigs");

            migrationBuilder.DropTable(
                name: "Programs");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Universities");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
