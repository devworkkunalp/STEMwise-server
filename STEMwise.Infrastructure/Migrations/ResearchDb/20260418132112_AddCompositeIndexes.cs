using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STEMwise.Infrastructure.Migrations.ResearchDb
{
    /// <inheritdoc />
    public partial class AddCompositeIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Specialization",
                table: "VisaBenchmarks",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "RegionName",
                table: "VisaBenchmarks",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Specialization",
                table: "LaborBenchmarks",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "RegionName",
                table: "LaborBenchmarks",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Specialization",
                table: "GlobalSectorBenchmarks",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CountryName",
                table: "GlobalSectorBenchmarks",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // Drop old non-composite unique indexes
            migrationBuilder.DropIndex(
                name: "IX_LaborBenchmarks_RegionName",
                table: "LaborBenchmarks");

            migrationBuilder.DropIndex(
                name: "IX_VisaBenchmarks_RegionName",
                table: "VisaBenchmarks");

            migrationBuilder.CreateIndex(
                name: "IX_VisaBenchmarks_RegionName_Specialization",
                table: "VisaBenchmarks",
                columns: new[] { "RegionName", "Specialization" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LaborBenchmarks_RegionName_Specialization",
                table: "LaborBenchmarks",
                columns: new[] { "RegionName", "Specialization" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GlobalSectorBenchmarks_CountryName_Specialization",
                table: "GlobalSectorBenchmarks",
                columns: new[] { "CountryName", "Specialization" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VisaBenchmarks_RegionName_Specialization",
                table: "VisaBenchmarks");

            migrationBuilder.DropIndex(
                name: "IX_LaborBenchmarks_RegionName_Specialization",
                table: "LaborBenchmarks");

            migrationBuilder.DropIndex(
                name: "IX_GlobalSectorBenchmarks_CountryName_Specialization",
                table: "GlobalSectorBenchmarks");

            migrationBuilder.AlterColumn<string>(
                name: "Specialization",
                table: "VisaBenchmarks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "RegionName",
                table: "VisaBenchmarks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Specialization",
                table: "LaborBenchmarks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "RegionName",
                table: "LaborBenchmarks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Specialization",
                table: "GlobalSectorBenchmarks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CountryName",
                table: "GlobalSectorBenchmarks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
