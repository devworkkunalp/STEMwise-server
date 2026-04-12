using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace STEMwise.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentitySupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employers",
                keyColumn: "Id",
                keyValue: new Guid("426887e3-d5c5-40ff-8f56-336fb3f1a1bc"));

            migrationBuilder.DeleteData(
                table: "Employers",
                keyColumn: "Id",
                keyValue: new Guid("42f0c79c-37ed-4260-bff6-9686c4d34665"));

            migrationBuilder.DeleteData(
                table: "Employers",
                keyColumn: "Id",
                keyValue: new Guid("5af3f54b-832b-47b2-b2ab-a35814f69c99"));

            migrationBuilder.DeleteData(
                table: "Employers",
                keyColumn: "Id",
                keyValue: new Guid("d187a922-f9c8-4b31-aaee-dc81be7d276c"));

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 12, 6, 50, 5, 115, DateTimeKind.Utc).AddTicks(4547));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 12, 6, 50, 5, 115, DateTimeKind.Utc).AddTicks(9767));

            migrationBuilder.InsertData(
                table: "Employers",
                columns: new[] { "Id", "AvgSponsoredSalary", "CreatedAt", "H1BFilingsTotal", "Name", "PrimaryStemFields", "SponsorScore", "TopCities" },
                values: new object[,]
                {
                    { new Guid("1a2aa907-4425-447b-b9ff-53191c575afe"), 165000, new DateTime(2026, 4, 12, 6, 50, 5, 117, DateTimeKind.Utc).AddTicks(5271), 12500, "Google", "[\"Software Engineering\",\"Data Science\",\"AI\"]", 98, "[\"Mountain View\",\"San Francisco\",\"Austin\",\"New York\"]" },
                    { new Guid("378e275a-5b33-4cbe-86c4-0d215de3e41e"), 175000, new DateTime(2026, 4, 12, 6, 50, 5, 117, DateTimeKind.Utc).AddTicks(5682), 8500, "Meta", "[\"Product Engineering\",\"Data Engineering\"]", 92, "[\"Menlo Park\",\"San Francisco\",\"Seattle\",\"Austin\"]" },
                    { new Guid("76226072-8fa6-47d9-8abf-a548a13f4931"), 145000, new DateTime(2026, 4, 12, 6, 50, 5, 117, DateTimeKind.Utc).AddTicks(5689), 2100, "Tesla", "[\"Mechanical Engineering\",\"Software Engineering\",\"AI\"]", 88, "[\"Austin\",\"Fremont\",\"Palo Alto\"]" },
                    { new Guid("baf2c848-83c1-481e-bf69-85c71ac24994"), 155000, new DateTime(2026, 4, 12, 6, 50, 5, 117, DateTimeKind.Utc).AddTicks(5663), 35000, "Amazon", "[\"Cloud Computing\",\"Software Engineering\",\"Product Management\"]", 95, "[\"Seattle\",\"Austin\",\"Arlington\",\"San Francisco\"]" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "Employers",
                keyColumn: "Id",
                keyValue: new Guid("1a2aa907-4425-447b-b9ff-53191c575afe"));

            migrationBuilder.DeleteData(
                table: "Employers",
                keyColumn: "Id",
                keyValue: new Guid("378e275a-5b33-4cbe-86c4-0d215de3e41e"));

            migrationBuilder.DeleteData(
                table: "Employers",
                keyColumn: "Id",
                keyValue: new Guid("76226072-8fa6-47d9-8abf-a548a13f4931"));

            migrationBuilder.DeleteData(
                table: "Employers",
                keyColumn: "Id",
                keyValue: new Guid("baf2c848-83c1-481e-bf69-85c71ac24994"));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 12, 5, 55, 38, 722, DateTimeKind.Utc).AddTicks(7843));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "CreatedAt",
                value: new DateTime(2026, 4, 12, 5, 55, 38, 723, DateTimeKind.Utc).AddTicks(7244));

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
        }
    }
}
