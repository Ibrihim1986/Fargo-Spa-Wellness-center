using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Family_and_Spa_Wellness.Migrations
{
    /// <inheritdoc />
    public partial class AddServicePricingTiers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Appointments",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "ServicePricingTiers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProviderId = table.Column<int>(type: "INTEGER", nullable: true),
                    DurationMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePricingTiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicePricingTiers_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServicePricingTiers_Users_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServicePricingTiers_ProviderId",
                table: "ServicePricingTiers",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePricingTiers_ServiceId",
                table: "ServicePricingTiers",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServicePricingTiers");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Appointments");
        }
    }
}
