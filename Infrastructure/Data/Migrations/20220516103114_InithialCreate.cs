using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InithialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "BgtOwn");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "BgtEmployee");

            migrationBuilder.DropColumn(
                name: "Segment",
                table: "BgtEmployee");

            migrationBuilder.AddColumn<int>(
                name: "AuthId",
                table: "BgtOwn",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "BgtOwn",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompoCode",
                table: "BgtOwn",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoOfEmployee",
                table: "BgtEmployee",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CountDouble",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    Count = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountDouble", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CountLong",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    Count = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountLong", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CountDouble");

            migrationBuilder.DropTable(
                name: "CountLong");

            migrationBuilder.DropColumn(
                name: "AuthId",
                table: "BgtOwn");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "BgtOwn");

            migrationBuilder.DropColumn(
                name: "CompoCode",
                table: "BgtOwn");

            migrationBuilder.DropColumn(
                name: "NoOfEmployee",
                table: "BgtEmployee");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "BgtOwn",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "BgtEmployee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Segment",
                table: "BgtEmployee",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
