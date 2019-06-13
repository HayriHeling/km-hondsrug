using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eduria.Migrations
{
    public partial class Sprint4changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeLineHasTimeTables");

            migrationBuilder.DropTable(
                name: "TimeLineHasUsers");

            migrationBuilder.DropTable(
                name: "TimeLines");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "TimeTableInformations",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TimeTableInformations");

            migrationBuilder.CreateTable(
                name: "TimeLineHasTimeTables",
                columns: table => new
                {
                    TimeLineHasTimeTableId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TimeLineId = table.Column<int>(nullable: false),
                    TimeTableId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeLineHasTimeTables", x => x.TimeLineHasTimeTableId);
                });

            migrationBuilder.CreateTable(
                name: "TimeLineHasUsers",
                columns: table => new
                {
                    TimeLineHasUserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TimeLineId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeLineHasUsers", x => x.TimeLineHasUserId);
                });

            migrationBuilder.CreateTable(
                name: "TimeLines",
                columns: table => new
                {
                    TimeLineId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeLines", x => x.TimeLineId);
                });
        }
    }
}
