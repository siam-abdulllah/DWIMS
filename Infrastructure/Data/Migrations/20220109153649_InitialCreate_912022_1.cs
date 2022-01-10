using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_912022_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DepotCode",
                table: "InvestmentTargetedGroup",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepotName",
                table: "InvestmentTargetedGroup",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarketGroupCode",
                table: "InvestmentTargetedGroup",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarketGroupName",
                table: "InvestmentTargetedGroup",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegionCode",
                table: "InvestmentTargetedGroup",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegionName",
                table: "InvestmentTargetedGroup",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TerritoryCode",
                table: "InvestmentTargetedGroup",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TerritoryName",
                table: "InvestmentTargetedGroup",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZoneCode",
                table: "InvestmentTargetedGroup",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZoneName",
                table: "InvestmentTargetedGroup",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RptDepotLetter",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    ReferenceNo = table.Column<string>(nullable: true),
                    DonationTypeName = table.Column<string>(nullable: true),
                    DoctorName = table.Column<string>(nullable: true),
                    ProposedAmount = table.Column<long>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    DocId = table.Column<int>(nullable: false),
                    EmployeeName = table.Column<string>(nullable: true),
                    EmpId = table.Column<int>(nullable: false),
                    DesignationName = table.Column<string>(nullable: true),
                    MarketName = table.Column<string>(nullable: true),
                    DepotName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RptDepotLetter", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RptDepotLetter");

            migrationBuilder.DropColumn(
                name: "DepotCode",
                table: "InvestmentTargetedGroup");

            migrationBuilder.DropColumn(
                name: "DepotName",
                table: "InvestmentTargetedGroup");

            migrationBuilder.DropColumn(
                name: "MarketGroupCode",
                table: "InvestmentTargetedGroup");

            migrationBuilder.DropColumn(
                name: "MarketGroupName",
                table: "InvestmentTargetedGroup");

            migrationBuilder.DropColumn(
                name: "RegionCode",
                table: "InvestmentTargetedGroup");

            migrationBuilder.DropColumn(
                name: "RegionName",
                table: "InvestmentTargetedGroup");

            migrationBuilder.DropColumn(
                name: "TerritoryCode",
                table: "InvestmentTargetedGroup");

            migrationBuilder.DropColumn(
                name: "TerritoryName",
                table: "InvestmentTargetedGroup");

            migrationBuilder.DropColumn(
                name: "ZoneCode",
                table: "InvestmentTargetedGroup");

            migrationBuilder.DropColumn(
                name: "ZoneName",
                table: "InvestmentTargetedGroup");
        }
    }
}
