using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_992021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DivisionCode",
                table: "InvestmentRecComment");

            migrationBuilder.DropColumn(
                name: "DivisionName",
                table: "InvestmentRecComment");

            migrationBuilder.DropColumn(
                name: "PostingType",
                table: "InvestmentRecComment");

            migrationBuilder.DropColumn(
                name: "DivisionCode",
                table: "InvestmentInit");

            migrationBuilder.DropColumn(
                name: "DivisionName",
                table: "InvestmentInit");

            migrationBuilder.DropColumn(
                name: "PostingType",
                table: "InvestmentInit");

            migrationBuilder.DropColumn(
                name: "DivisionCode",
                table: "InvestmentAprComment");

            migrationBuilder.DropColumn(
                name: "DivisionName",
                table: "InvestmentAprComment");

            migrationBuilder.DropColumn(
                name: "PostingType",
                table: "InvestmentAprComment");

            migrationBuilder.DropColumn(
                name: "DivisionCode",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "DivisionName",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PostingType",
                table: "Employee");

            migrationBuilder.AddColumn<string>(
                name: "SBUName",
                table: "ProductInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SBUName",
                table: "MarketGroupDtl",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SBU",
                table: "InvestmentTargetedGroup",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SBUName",
                table: "InvestmentTargetedGroup",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarketGroupCode",
                table: "InvestmentRecComment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarketGroupName",
                table: "InvestmentRecComment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SBUName",
                table: "InvestmentRecComment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarketGroupCode",
                table: "InvestmentInit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarketGroupName",
                table: "InvestmentInit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SBUName",
                table: "InvestmentInit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarketGroupCode",
                table: "InvestmentAprComment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarketGroupName",
                table: "InvestmentAprComment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SBUName",
                table: "InvestmentAprComment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarketGroupCode",
                table: "Employee",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarketGroupName",
                table: "Employee",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RptDocCampWiseInvestment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    SBUName = table.Column<string>(maxLength: 50, nullable: true),
                    SBUCode = table.Column<string>(maxLength: 10, nullable: true),
                    MarketGroupCode = table.Column<string>(maxLength: 10, nullable: true),
                    MarketGroupName = table.Column<string>(maxLength: 70, nullable: true),
                    MarketCode = table.Column<string>(maxLength: 10, nullable: true),
                    MarketName = table.Column<string>(maxLength: 70, nullable: true),
                    TerritoryCode = table.Column<string>(maxLength: 10, nullable: true),
                    TerritoryName = table.Column<string>(maxLength: 70, nullable: true),
                    RegionCode = table.Column<string>(maxLength: 10, nullable: true),
                    RegionName = table.Column<string>(maxLength: 70, nullable: true),
                    DivisionCode = table.Column<string>(maxLength: 10, nullable: true),
                    DivisionName = table.Column<string>(maxLength: 50, nullable: true),
                    ZoneCode = table.Column<string>(maxLength: 10, nullable: true),
                    ZoneName = table.Column<string>(maxLength: 50, nullable: true),
                    DoctorId = table.Column<string>(maxLength: 10, nullable: true),
                    DoctorName = table.Column<string>(maxLength: 150, nullable: true),
                    DoctorCategory = table.Column<string>(maxLength: 30, nullable: true),
                    FromDate = table.Column<DateTime>(nullable: false),
                    ToDate = table.Column<DateTime>(nullable: false),
                    InvestmentPresent = table.Column<double>(nullable: false),
                    InvestmentPast = table.Column<double>(nullable: false),
                    Brand = table.Column<string>(maxLength: 50, nullable: true),
                    Campaign = table.Column<string>(maxLength: 50, nullable: true),
                    SubCampaign = table.Column<string>(maxLength: 50, nullable: true),
                    Commitment = table.Column<string>(maxLength: 50, nullable: true),
                    ActualShareBrand = table.Column<string>(maxLength: 10, nullable: true),
                    ActualShare = table.Column<string>(maxLength: 10, nullable: true),
                    CompetitorShare = table.Column<string>(maxLength: 10, nullable: true),
                    NoOfPresc = table.Column<string>(maxLength: 10, nullable: true),
                    NoOfPatient = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RptDocCampWiseInvestment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RptDocLocWiseInvestment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    SBUName = table.Column<string>(maxLength: 50, nullable: true),
                    SBUCode = table.Column<string>(maxLength: 10, nullable: true),
                    MarketGroupCode = table.Column<string>(maxLength: 10, nullable: true),
                    MarketGroupName = table.Column<string>(maxLength: 70, nullable: true),
                    MarketCode = table.Column<string>(maxLength: 10, nullable: true),
                    MarketName = table.Column<string>(maxLength: 70, nullable: true),
                    TerritoryCode = table.Column<string>(maxLength: 10, nullable: true),
                    TerritoryName = table.Column<string>(maxLength: 70, nullable: true),
                    RegionCode = table.Column<string>(maxLength: 10, nullable: true),
                    RegionName = table.Column<string>(maxLength: 70, nullable: true),
                    DivisionCode = table.Column<string>(maxLength: 10, nullable: true),
                    DivisionName = table.Column<string>(maxLength: 50, nullable: true),
                    ZoneCode = table.Column<string>(maxLength: 10, nullable: true),
                    ZoneName = table.Column<string>(maxLength: 50, nullable: true),
                    DoctorId = table.Column<string>(maxLength: 10, nullable: true),
                    DoctorName = table.Column<string>(maxLength: 150, nullable: true),
                    InstitutionId = table.Column<string>(maxLength: 10, nullable: true),
                    InstitutionName = table.Column<string>(maxLength: 150, nullable: true),
                    DonationType = table.Column<string>(maxLength: 20, nullable: true),
                    FromDate = table.Column<DateTime>(nullable: false),
                    ToDate = table.Column<DateTime>(nullable: false),
                    InvestedAmt = table.Column<double>(nullable: false),
                    Commitment = table.Column<string>(maxLength: 50, nullable: true),
                    ActualShare = table.Column<string>(maxLength: 10, nullable: true),
                    CompetitorShare = table.Column<string>(maxLength: 10, nullable: true),
                    NoOfPresc = table.Column<string>(maxLength: 10, nullable: true),
                    NoOfPatient = table.Column<string>(maxLength: 10, nullable: true),
                    Deviation = table.Column<string>(maxLength: 10, nullable: true),
                    LeaderNonLeader = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RptDocLocWiseInvestment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RptInsSocBcdsInvestment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    SBUName = table.Column<string>(maxLength: 50, nullable: true),
                    SBUCode = table.Column<string>(maxLength: 10, nullable: true),
                    MarketGroupCode = table.Column<string>(maxLength: 10, nullable: true),
                    MarketGroupName = table.Column<string>(maxLength: 70, nullable: true),
                    MarketCode = table.Column<string>(maxLength: 10, nullable: true),
                    MarketName = table.Column<string>(maxLength: 70, nullable: true),
                    TerritoryCode = table.Column<string>(maxLength: 10, nullable: true),
                    TerritoryName = table.Column<string>(maxLength: 70, nullable: true),
                    RegionCode = table.Column<string>(maxLength: 10, nullable: true),
                    RegionName = table.Column<string>(maxLength: 70, nullable: true),
                    DivisionCode = table.Column<string>(maxLength: 10, nullable: true),
                    DivisionName = table.Column<string>(maxLength: 50, nullable: true),
                    ZoneCode = table.Column<string>(maxLength: 10, nullable: true),
                    ZoneName = table.Column<string>(maxLength: 50, nullable: true),
                    InstitutionId = table.Column<string>(maxLength: 10, nullable: true),
                    InstitutionName = table.Column<string>(maxLength: 150, nullable: true),
                    SocietyId = table.Column<string>(maxLength: 10, nullable: true),
                    SocietyName = table.Column<string>(maxLength: 150, nullable: true),
                    BcdsId = table.Column<string>(maxLength: 10, nullable: true),
                    BcdsName = table.Column<string>(maxLength: 150, nullable: true),
                    DonationType = table.Column<string>(maxLength: 20, nullable: true),
                    FromDate = table.Column<DateTime>(nullable: false),
                    ToDate = table.Column<DateTime>(nullable: false),
                    InvestedAmt = table.Column<double>(nullable: false),
                    Commitment = table.Column<string>(maxLength: 50, nullable: true),
                    ActualShare = table.Column<string>(maxLength: 10, nullable: true),
                    CompetitorShare = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RptInsSocBcdsInvestment", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RptDocCampWiseInvestment");

            migrationBuilder.DropTable(
                name: "RptDocLocWiseInvestment");

            migrationBuilder.DropTable(
                name: "RptInsSocBcdsInvestment");

            migrationBuilder.DropColumn(
                name: "SBUName",
                table: "ProductInfo");

            migrationBuilder.DropColumn(
                name: "SBUName",
                table: "MarketGroupDtl");

            migrationBuilder.DropColumn(
                name: "SBU",
                table: "InvestmentTargetedGroup");

            migrationBuilder.DropColumn(
                name: "SBUName",
                table: "InvestmentTargetedGroup");

            migrationBuilder.DropColumn(
                name: "MarketGroupCode",
                table: "InvestmentRecComment");

            migrationBuilder.DropColumn(
                name: "MarketGroupName",
                table: "InvestmentRecComment");

            migrationBuilder.DropColumn(
                name: "SBUName",
                table: "InvestmentRecComment");

            migrationBuilder.DropColumn(
                name: "MarketGroupCode",
                table: "InvestmentInit");

            migrationBuilder.DropColumn(
                name: "MarketGroupName",
                table: "InvestmentInit");

            migrationBuilder.DropColumn(
                name: "SBUName",
                table: "InvestmentInit");

            migrationBuilder.DropColumn(
                name: "MarketGroupCode",
                table: "InvestmentAprComment");

            migrationBuilder.DropColumn(
                name: "MarketGroupName",
                table: "InvestmentAprComment");

            migrationBuilder.DropColumn(
                name: "SBUName",
                table: "InvestmentAprComment");

            migrationBuilder.DropColumn(
                name: "MarketGroupCode",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "MarketGroupName",
                table: "Employee");

            migrationBuilder.AddColumn<string>(
                name: "DivisionCode",
                table: "InvestmentRecComment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DivisionName",
                table: "InvestmentRecComment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostingType",
                table: "InvestmentRecComment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DivisionCode",
                table: "InvestmentInit",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DivisionName",
                table: "InvestmentInit",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostingType",
                table: "InvestmentInit",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DivisionCode",
                table: "InvestmentAprComment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DivisionName",
                table: "InvestmentAprComment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostingType",
                table: "InvestmentAprComment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DivisionCode",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DivisionName",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostingType",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
