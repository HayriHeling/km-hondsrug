using Microsoft.EntityFrameworkCore.Migrations;

namespace Eduria.Migrations
{
    public partial class SmallupdatetoName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TimeTableInformations",
                maxLength: 2147483647,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Exams",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "Configs",
                maxLength: 2147483647,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 256);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TimeTableInformations",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 2147483647);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Exams",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<string>(
                name: "Body",
                table: "Configs",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 2147483647);
        }
    }
}
