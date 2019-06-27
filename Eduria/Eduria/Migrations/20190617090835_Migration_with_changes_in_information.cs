using Microsoft.EntityFrameworkCore.Migrations;

namespace Eduria.Migrations
{
    public partial class Migration_with_changes_in_information : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AudioSource",
                table: "TimeTableInformations");

            migrationBuilder.DropColumn(
                name: "VideoSource",
                table: "TimeTableInformations");

            migrationBuilder.AddColumn<int>(
                name: "BeforeChrist",
                table: "TimeTableInformations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "TimeTableInformations",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeforeChrist",
                table: "TimeTableInformations");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "TimeTableInformations");

            migrationBuilder.AddColumn<string>(
                name: "AudioSource",
                table: "TimeTableInformations",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoSource",
                table: "TimeTableInformations",
                maxLength: 256,
                nullable: true);
        }
    }
}
