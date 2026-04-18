using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STEMwise.Infrastructure.Migrations.ResearchDb
{
    /// <inheritdoc />
    public partial class AddSpecializationToResearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Specialization",
                table: "LaborBenchmarks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "General");

            migrationBuilder.AddColumn<string>(
                name: "Specialization",
                table: "VisaBenchmarks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "General");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GlobalUniversityMetrics");

            migrationBuilder.DropTable(
                name: "LaborBenchmarks");

            migrationBuilder.DropTable(
                name: "RegionalRents");

            migrationBuilder.DropTable(
                name: "UniversityMetrics");

            migrationBuilder.DropTable(
                name: "VisaBenchmarks");
        }
    }
}
