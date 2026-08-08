using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Family_and_Spa_Wellness.Migrations
{
    /// <inheritdoc />
    public partial class AddAppointmentStatusTestimonialServiceAndAvailability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            try
            {
                migrationBuilder.AddColumn<int>(
                    name: "ServiceId",
                    table: "Testimonials",
                    type: "INTEGER",
                    nullable: true);
            }
            catch { }

            try
            {
                migrationBuilder.AddColumn<string>(
                    name: "Status",
                    table: "Appointments",
                    type: "TEXT",
                    nullable: false,
                    defaultValue: "");
            }
            catch { }

            migrationBuilder.CreateTable(
                name: "ProviderAvailabilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProviderId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Hour = table.Column<int>(type: "INTEGER", nullable: false),
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderAvailabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProviderAvailabilities_Users_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Testimonials_ServiceId",
                table: "Testimonials",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderAvailabilities_ProviderId",
                table: "ProviderAvailabilities",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Testimonials_Services_ServiceId",
                table: "Testimonials",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Testimonials_Services_ServiceId",
                table: "Testimonials");

            migrationBuilder.DropTable(
                name: "ProviderAvailabilities");

            migrationBuilder.DropIndex(
                name: "IX_Testimonials_ServiceId",
                table: "Testimonials");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Testimonials");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Appointments");
        }
    }
}
