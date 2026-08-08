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
            // Appointment.Price column is added by a later migration (AddMemberships).
            // Omit adding the column here to avoid duplicate-column errors when the
            // database is created from all migrations in sequence.

            // Use idempotent SQL to create the table and indexes only if they do not already exist.
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TABLE IF EXISTS ""ServicePricingTiers"";");
        }
    }
}
