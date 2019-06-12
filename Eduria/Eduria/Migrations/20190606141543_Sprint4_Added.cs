using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eduria.Migrations
{
    public partial class Sprint4_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source",
                table: "TimeTables");

            migrationBuilder.DropColumn(
                name: "MediaLink",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Period",
                table: "AnalyticDatas");

            migrationBuilder.RenameColumn(
                name: "MediaType",
                table: "Questions",
                newName: "MediaSourceId");

            migrationBuilder.RenameColumn(
                name: "Year",
                table: "AnalyticDatas",
                newName: "PeriodId");

            migrationBuilder.AddColumn<int>(
                name: "MediaSourceId",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MediaSourceId",
                table: "TimeTables",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MediaSources",
                columns: table => new
                {
                    MediaSourceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Source = table.Column<string>(maxLength: 256, nullable: false),
                    MediaType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaSources", x => x.MediaSourceId);
                });

            migrationBuilder.CreateTable(
                name: "Periods",
                columns: table => new
                {
                    PeriodId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PeriodNum = table.Column<int>(nullable: false),
                    PeriodStart = table.Column<DateTime>(nullable: false),
                    PeriodEnd = table.Column<DateTime>(nullable: false),
                    SchoolYearStart = table.Column<int>(nullable: false),
                    SchoolYearEnd = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periods", x => x.PeriodId);
                });

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

            migrationBuilder.CreateTable(
                name: "TimeTableInfoHasMediaSrcs",
                columns: table => new
                {
                    TimeTableInfoHasMediaSrcId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TimeTableInformationId = table.Column<int>(nullable: false),
                    MediaSourceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeTableInfoHasMediaSrcs", x => x.TimeTableInfoHasMediaSrcId);
                });

            migrationBuilder.CreateTable(
                name: "TimeTableInformations",
                columns: table => new
                {
                    TimeTableInformationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TimeTableId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 256, nullable: true),
                    AudioSource = table.Column<string>(maxLength: 256, nullable: true),
                    VideoSource = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeTableInformations", x => x.TimeTableInformationId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaSources");

            migrationBuilder.DropTable(
                name: "Periods");

            migrationBuilder.DropTable(
                name: "TimeLineHasTimeTables");

            migrationBuilder.DropTable(
                name: "TimeLineHasUsers");

            migrationBuilder.DropTable(
                name: "TimeLines");

            migrationBuilder.DropTable(
                name: "TimeTableInfoHasMediaSrcs");

            migrationBuilder.DropTable(
                name: "TimeTableInformations");

            migrationBuilder.DropColumn(
                name: "MediaSourceId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MediaSourceId",
                table: "TimeTables");

            migrationBuilder.RenameColumn(
                name: "MediaSourceId",
                table: "Questions",
                newName: "MediaType");

            migrationBuilder.RenameColumn(
                name: "PeriodId",
                table: "AnalyticDatas",
                newName: "Year");

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "TimeTables",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MediaLink",
                table: "Questions",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Period",
                table: "AnalyticDatas",
                nullable: false,
                defaultValue: 0);
        }
    }
}
