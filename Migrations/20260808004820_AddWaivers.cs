using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Family_and_Spa_Wellness.Migrations
{
    /// <inheritdoc />
    public partial class AddWaivers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequiresWaiverType",
                table: "Services",
                type: "TEXT",
                nullable: true);

            // Create Waivers table if it does not already exist (idempotent)
            migrationBuilder.Sql(@"CREATE TABLE IF NOT EXISTS ""Waivers"" (
                ""Id"" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                ""ClientId"" INTEGER NOT NULL,
                ""WaiverType"" TEXT NOT NULL,
                ""IsSigned"" INTEGER NOT NULL,
                ""SignedAt"" TEXT NULL,
                FOREIGN KEY (""ClientId"") REFERENCES ""Users"" (""Id"") ON DELETE RESTRICT
            );");

            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ""IX_Waivers_ClientId"" ON ""Waivers"" (""ClientId"");");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Waivers");

            migrationBuilder.DropColumn(
                name: "RequiresWaiverType",
                table: "Services");
        }
    }
}
