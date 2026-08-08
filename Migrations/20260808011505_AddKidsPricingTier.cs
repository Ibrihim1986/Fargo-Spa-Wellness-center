using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Family_and_Spa_Wellness.Migrations
{
    /// <inheritdoc />
    public partial class AddKidsPricingTier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxAge",
                table: "ServicePricingTiers",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MaxAge",
                table: "ServicePricingTiers");
        }
    }
}
