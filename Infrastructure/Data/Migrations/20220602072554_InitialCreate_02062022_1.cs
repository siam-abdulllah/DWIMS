using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_02062022_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductCode",
                table: "MedicineDispatchDtl",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SBU",
                table: "MedicineDispatchDtl",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DonationTypeAllocated",
                table: "BgtEmployeeLocationWiseSBUExp",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TotalAllocated",
                table: "BgtEmployeeLocationWiseSBUExp",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalLoc",
                table: "BgtEmployeeLocationWiseSBUExp",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AuthExpense",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    Expense = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthExpense", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BgtCampaign",
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
                    Amount = table.Column<long>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    EnteredBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BgtCampaign", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BgtOwnTotal",
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
                    AuthId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    SBU = table.Column<string>(nullable: true),
                    CompoCode = table.Column<string>(nullable: true),
                    DonationId = table.Column<int>(nullable: false),
                    Amount = table.Column<long>(nullable: false),
                    AmtLimit = table.Column<long>(nullable: false),
                    Segment = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    EnteredBy = table.Column<int>(nullable: false),
                    TotalAmount = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BgtOwnTotal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DonWiseExpByEmp",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    DonationId = table.Column<int>(nullable: false),
                    Count = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonWiseExpByEmp", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthExpense");

            migrationBuilder.DropTable(
                name: "BgtCampaign");

            migrationBuilder.DropTable(
                name: "BgtOwnTotal");

            migrationBuilder.DropTable(
                name: "DonWiseExpByEmp");

            migrationBuilder.DropColumn(
                name: "ProductCode",
                table: "MedicineDispatchDtl");

            migrationBuilder.DropColumn(
                name: "SBU",
                table: "MedicineDispatchDtl");

            migrationBuilder.DropColumn(
                name: "DonationTypeAllocated",
                table: "BgtEmployeeLocationWiseSBUExp");

            migrationBuilder.DropColumn(
                name: "TotalAllocated",
                table: "BgtEmployeeLocationWiseSBUExp");

            migrationBuilder.DropColumn(
                name: "TotalLoc",
                table: "BgtEmployeeLocationWiseSBUExp");
        }
    }
}
