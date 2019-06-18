using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eduria.Migrations
{
    public partial class Sprint4_Added_config : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsActive",
                table: "Exams",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Configs",
                columns: table => new
                {
                    ConfigId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ToMail = table.Column<string>(maxLength: 256, nullable: false),
                    UserMail = table.Column<string>(maxLength: 256, nullable: false),
                    Password = table.Column<string>(maxLength: 256, nullable: false),
                    FromMail = table.Column<string>(maxLength: 256, nullable: false),
                    SMTPPort = table.Column<int>(nullable: false),
                    Host = table.Column<string>(maxLength: 256, nullable: false),
                    EntryCreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configs", x => x.ConfigId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configs");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Exams");
        }
    }
}
