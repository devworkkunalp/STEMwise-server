using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STEMwise.Infrastructure.Migrations.ResearchDb
{
    /// <inheritdoc />
    public partial class ExpandResearchAnalytics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OutcomeEmployedPct",
                table: "VisaBenchmarks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OutcomeH1BPct",
                table: "VisaBenchmarks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OutcomeReturnedPct",
                table: "VisaBenchmarks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Percentile10Salary",
                table: "LaborBenchmarks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Percentile25Salary",
                table: "LaborBenchmarks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Percentile90Salary",
                table: "LaborBenchmarks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GlobalSectorBenchmarks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Flag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MedianSalary = table.Column<int>(type: "int", nullable: false),
                    PrMetric = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VisaEase = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoiScore = table.Column<int>(type: "int", nullable: false),
                    LastSynced = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalSectorBenchmarks", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GlobalSectorBenchmarks");

            migrationBuilder.DropColumn(
                name: "OutcomeEmployedPct",
                table: "VisaBenchmarks");

            migrationBuilder.DropColumn(
                name: "OutcomeH1BPct",
                table: "VisaBenchmarks");

            migrationBuilder.DropColumn(
                name: "OutcomeReturnedPct",
                table: "VisaBenchmarks");

            migrationBuilder.DropColumn(
                name: "Percentile10Salary",
                table: "LaborBenchmarks");

            migrationBuilder.DropColumn(
                name: "Percentile25Salary",
                table: "LaborBenchmarks");

            migrationBuilder.DropColumn(
                name: "Percentile90Salary",
                table: "LaborBenchmarks");
        }
    }
}
