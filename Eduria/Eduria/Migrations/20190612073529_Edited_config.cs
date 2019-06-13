using Microsoft.EntityFrameworkCore.Migrations;

namespace Eduria.Migrations
{
    public partial class Edited_config : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserMail",
                table: "Configs",
                newName: "Subject");

            migrationBuilder.RenameColumn(
                name: "ToMail",
                table: "Configs",
                newName: "Body");

            migrationBuilder.RenameColumn(
                name: "EntryCreatedAt",
                table: "Configs",
                newName: "EntryChangedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Subject",
                table: "Configs",
                newName: "UserMail");

            migrationBuilder.RenameColumn(
                name: "EntryChangedAt",
                table: "Configs",
                newName: "EntryCreatedAt");

            migrationBuilder.RenameColumn(
                name: "Body",
                table: "Configs",
                newName: "ToMail");
        }
    }
}
