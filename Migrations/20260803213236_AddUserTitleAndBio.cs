using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Family_and_Spa_Wellness.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTitleAndBio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            try
            {
                migrationBuilder.AddColumn<string>(
                    name: "Bio",
                    table: "Users",
                    type: "TEXT",
                    nullable: true);
            }
            catch { }

            try
            {
                migrationBuilder.AddColumn<string>(
                    name: "Title",
                    table: "Users",
                    type: "TEXT",
                    nullable: true);
            }
            catch { }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Users");
        }
    }
}
