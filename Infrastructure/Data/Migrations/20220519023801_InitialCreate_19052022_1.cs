using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_19052022_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MarketInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    SBU = table.Column<string>(nullable: true),
                    SBUName = table.Column<string>(nullable: true),
                    MarketCode = table.Column<string>(nullable: true),
                    MarketName = table.Column<string>(nullable: true),
                    Serial = table.Column<int>(nullable: false),
                    TagCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegionInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    SBU = table.Column<string>(nullable: true),
                    SBUName = table.Column<string>(nullable: true),
                    RegionCode = table.Column<string>(nullable: true),
                    RegionName = table.Column<string>(nullable: true),
                    Serial = table.Column<int>(nullable: false),
                    TagCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TerritoryInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    SBU = table.Column<string>(nullable: true),
                    SBUName = table.Column<string>(nullable: true),
                    TerritoryCode = table.Column<string>(nullable: true),
                    TerritoryName = table.Column<string>(nullable: true),
                    Serial = table.Column<int>(nullable: false),
                    TagCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerritoryInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ZoneInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    SBU = table.Column<string>(nullable: true),
                    SBUName = table.Column<string>(nullable: true),
                    ZoneCode = table.Column<string>(nullable: true),
                    ZoneName = table.Column<string>(nullable: true),
                    Serial = table.Column<int>(nullable: false),
                    TagCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZoneInfo", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarketInfo");

            migrationBuilder.DropTable(
                name: "RegionInfo");

            migrationBuilder.DropTable(
                name: "TerritoryInfo");

            migrationBuilder.DropTable(
                name: "ZoneInfo");
        }
    }
}
