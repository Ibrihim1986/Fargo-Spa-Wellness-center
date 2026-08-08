using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Family_and_Spa_Wellness.Migrations
{
    /// <inheritdoc />
    public partial class AddMemberships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            try
            {
                migrationBuilder.AddColumn<decimal>(
                    name: "Price",
                    table: "Appointments",
                    type: "TEXT",
                    nullable: false,
                    defaultValue: 0m);
            }
            catch { }

            migrationBuilder.CreateTable(
                name: "ApprovalRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EntityType = table.Column<string>(type: "TEXT", nullable: false),
                    Action = table.Column<string>(type: "TEXT", nullable: false),
                    EntityId = table.Column<int>(type: "INTEGER", nullable: true),
                    Payload = table.Column<string>(type: "TEXT", nullable: false),
                    RequestedById = table.Column<int>(type: "INTEGER", nullable: false),
                    RequestedByEmail = table.Column<string>(type: "TEXT", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    ReviewedById = table.Column<int>(type: "INTEGER", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ReviewComment = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Memberships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlanName = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastRenewedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Memberships_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Create ServicePricingTiers table and indexes if they don't already exist
            migrationBuilder.Sql(@"CREATE TABLE IF NOT EXISTS ""ServicePricingTiers"" (
                ""Id"" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                ""ServiceId"" INTEGER NOT NULL,
                ""ProviderId"" INTEGER NULL,
                ""DurationMinutes"" INTEGER NOT NULL,
                ""Price"" TEXT NOT NULL,
                ""IsActive"" INTEGER NOT NULL,
                FOREIGN KEY (""ServiceId"") REFERENCES ""Services"" (""Id"") ON DELETE CASCADE,
                FOREIGN KEY (""ProviderId"") REFERENCES ""Users"" (""Id"")
            );");

            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ""IX_ServicePricingTiers_ProviderId"" ON ""ServicePricingTiers"" (""ProviderId"");");
            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ""IX_ServicePricingTiers_ServiceId"" ON ""ServicePricingTiers"" (""ServiceId"");");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_UserId",
                table: "Memberships",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovalRequests");

            migrationBuilder.DropTable(
                name: "Memberships");

            migrationBuilder.DropTable(
                name: "ServicePricingTiers");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Appointments");
        }
    }
}
