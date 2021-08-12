using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_1282021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SBU",
                table: "DoctorMarket",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "DoctorMarket",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReportInvestmentInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    MarketCode = table.Column<string>(nullable: true),
                    TerritoryCode = table.Column<string>(nullable: true),
                    RegionCode = table.Column<string>(nullable: true),
                    ZoneCode = table.Column<string>(nullable: true),
                    DivisionCode = table.Column<string>(nullable: true),
                    NationalCode = table.Column<string>(nullable: true),
                    SocietyId = table.Column<string>(nullable: true),
                    DoctorId = table.Column<string>(nullable: true),
                    InstituteId = table.Column<string>(nullable: true),
                    DonationType = table.Column<string>(nullable: true),
                    ExpenseDetail = table.Column<string>(nullable: true),
                    InvestmentAmount = table.Column<string>(nullable: true),
                    FromDate = table.Column<string>(nullable: true),
                    ToDate = table.Column<string>(nullable: true),
                    PrescribedSharePrcnt = table.Column<string>(nullable: true),
                    PrescShareFromDate = table.Column<string>(nullable: true),
                    PrescShareToDate = table.Column<string>(nullable: true),
                    ComtSharePrcnt = table.Column<string>(nullable: true),
                    DonationDuration = table.Column<string>(nullable: true),
                    DonationFromDate = table.Column<string>(nullable: true),
                    DonationToDate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportInvestmentInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportProductInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    ProductCode = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    SBU = table.Column<string>(nullable: true),
                    MarketCode = table.Column<string>(nullable: true),
                    TerritoryCode = table.Column<string>(nullable: true),
                    RegionCode = table.Column<string>(nullable: true),
                    ZoneCode = table.Column<string>(nullable: true),
                    DivisionCode = table.Column<string>(nullable: true),
                    NationalCode = table.Column<string>(nullable: true),
                    SocietyId = table.Column<string>(nullable: true),
                    DoctorId = table.Column<string>(nullable: true),
                    InstituteId = table.Column<string>(nullable: true),
                    DonationType = table.Column<string>(nullable: true),
                    ExpenseDetail = table.Column<string>(nullable: true),
                    InvestmentAmount = table.Column<string>(nullable: true),
                    FromDate = table.Column<string>(nullable: true),
                    ToDate = table.Column<string>(nullable: true),
                    PrescribedSharePrcnt = table.Column<string>(nullable: true),
                    PrescShareFromDate = table.Column<string>(nullable: true),
                    PrescShareToDate = table.Column<string>(nullable: true),
                    ComtSharePrcnt = table.Column<string>(nullable: true),
                    DonationDuration = table.Column<string>(nullable: true),
                    DonationFromDate = table.Column<string>(nullable: true),
                    DonationToDate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportProductInfo", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportInvestmentInfo");

            migrationBuilder.DropTable(
                name: "ReportProductInfo");

            migrationBuilder.DropColumn(
                name: "SBU",
                table: "DoctorMarket");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "DoctorMarket");
        }
    }
}
