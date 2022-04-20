using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Migrate200422_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BgtEmployee",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    CompId = table.Column<int>(nullable: false),
                    DeptId = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    SBU = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: false),
                    AuthId = table.Column<int>(nullable: false),
                    Amount = table.Column<long>(nullable: false),
                    Segment = table.Column<string>(nullable: true),
                    PermEdit = table.Column<bool>(nullable: false),
                    PermView = table.Column<bool>(nullable: false),
                    PermAmt = table.Column<bool>(nullable: false),
                    PermDonation = table.Column<bool>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    EnteredBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BgtEmployee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BgtOwn",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    CompId = table.Column<int>(nullable: false),
                    DeptId = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false),
                    SBU = table.Column<string>(nullable: true),
                    DonationId = table.Column<int>(nullable: false),
                    Amount = table.Column<long>(nullable: false),
                    AmtLimit = table.Column<long>(nullable: false),
                    Segment = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    EnteredBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BgtOwn", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BgtSBUTotal",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    CompId = table.Column<int>(nullable: false),
                    DeptId = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    SBU = table.Column<string>(nullable: true),
                    SBUAmount = table.Column<long>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    EnteredBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BgtSBUTotal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BgtYearlyTotal",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    CompId = table.Column<int>(nullable: false),
                    DeptId = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    TotalAmount = table.Column<long>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    EnteredBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BgtYearlyTotal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmpSbuMapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    CompId = table.Column<int>(nullable: false),
                    DeptId = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false),
                    SBU = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpSbuMapping", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BgtEmployee");

            migrationBuilder.DropTable(
                name: "BgtOwn");

            migrationBuilder.DropTable(
                name: "BgtSBUTotal");

            migrationBuilder.DropTable(
                name: "BgtYearlyTotal");

            migrationBuilder.DropTable(
                name: "EmpSbuMapping");
        }
    }
}
