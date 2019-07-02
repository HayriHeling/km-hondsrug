using Microsoft.EntityFrameworkCore.Migrations;

namespace Eduria.Migrations
{
    public partial class Bugfixv1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TimeTables",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeTableDesignId",
                table: "TimeTables",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "TimeTables");

            migrationBuilder.DropColumn(
                name: "TimeTableDesignId",
                table: "TimeTables");
        }
    }
}
