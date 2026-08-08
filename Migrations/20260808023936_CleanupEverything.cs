using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Family_and_Spa_Wellness.Migrations
{
    /// <inheritdoc />
    public partial class CleanupEverything : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // This migration's operations were already applied in earlier migrations or are redundant.
            // Keep Up() intentionally empty to avoid executing duplicate DDL.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovalRequests");

            migrationBuilder.DropTable(
                name: "Dependents");

            migrationBuilder.DropTable(
                name: "Fees");

            migrationBuilder.DropTable(
                name: "GiftCards");

            migrationBuilder.DropTable(
                name: "GuardianConsents");

            migrationBuilder.DropTable(
                name: "Memberships");

            migrationBuilder.DropTable(
                name: "SavedCards");

            migrationBuilder.DropTable(
                name: "Transactions");

            // Waivers related columns omitted from Down because they are managed in earlier migrations.
            // No-op in Down for Waivers columns to avoid attempting to drop columns that were added in earlier migrations.

            migrationBuilder.DropColumn(
                name: "NotificationChannel",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MinimumAge",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "RequiresWaiver",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "DependentId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "ProviderNotes",
                table: "Appointments");

            // Original IsSigned column handling omitted to avoid reintroducing duplicate/readonly properties.
        }
    }
}
