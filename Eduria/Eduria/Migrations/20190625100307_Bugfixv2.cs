using Microsoft.EntityFrameworkCore.Migrations;

namespace Eduria.Migrations
{
    public partial class Bugfixv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "TimeTableInformations",
                maxLength: 2147483647,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "TimeTableInformations",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2147483647,
                oldNullable: true);
        }
    }
}
