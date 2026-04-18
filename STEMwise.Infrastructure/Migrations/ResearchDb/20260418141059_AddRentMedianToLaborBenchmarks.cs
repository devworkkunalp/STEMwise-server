using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STEMwise.Infrastructure.Migrations.ResearchDb
{
    /// <inheritdoc />
    public partial class AddRentMedianToLaborBenchmarks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RentMedian",
                table: "LaborBenchmarks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RentMedian",
                table: "LaborBenchmarks");
        }
    }
}
