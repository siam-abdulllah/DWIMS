using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_29122021_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "RptInvestmentSummary",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarketName",
                table: "RptInvestmentSummary",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiveBy",
                table: "RptInvestmentSummary",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiveStatus",
                table: "RptInvestmentSummary",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepotCode",
                table: "Employee",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepotName",
                table: "Employee",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InvestmentRecDepot",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InvestmentInitId = table.Column<int>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: false),
                    DepotCode = table.Column<string>(nullable: true),
                    DepotName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentRecDepot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentRecDepot_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentRecDepot_InvestmentInit_InvestmentInitId",
                        column: x => x.InvestmentInitId,
                        principalTable: "InvestmentInit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentTargetGroupSQL",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InvestmentInitId = table.Column<int>(nullable: false),
                    SBU = table.Column<string>(nullable: true),
                    SBUName = table.Column<string>(nullable: true),
                    MarketCode = table.Column<string>(nullable: true),
                    MarketName = table.Column<string>(nullable: true),
                    MarketGroupName = table.Column<string>(nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    RecStatus = table.Column<string>(nullable: true),
                    ApprovalAuthorityName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentTargetGroupSQL", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LastFiveInvestmentInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    DonationShortName = table.Column<string>(nullable: true),
                    InvestmentAmount = table.Column<long>(nullable: true),
                    ComtSharePrcntAll = table.Column<string>(nullable: true),
                    ComtSharePrcnt = table.Column<string>(nullable: true),
                    PrescribedSharePrcnt = table.Column<double>(nullable: true),
                    PrescribedSharePrcntAll = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LastFiveInvestmentInfo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentRecDepot_EmployeeId",
                table: "InvestmentRecDepot",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentRecDepot_InvestmentInitId",
                table: "InvestmentRecDepot",
                column: "InvestmentInitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvestmentRecDepot");

            migrationBuilder.DropTable(
                name: "InvestmentTargetGroupSQL");

            migrationBuilder.DropTable(
                name: "LastFiveInvestmentInfo");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "RptInvestmentSummary");

            migrationBuilder.DropColumn(
                name: "MarketName",
                table: "RptInvestmentSummary");

            migrationBuilder.DropColumn(
                name: "ReceiveBy",
                table: "RptInvestmentSummary");

            migrationBuilder.DropColumn(
                name: "ReceiveStatus",
                table: "RptInvestmentSummary");

            migrationBuilder.DropColumn(
                name: "DepotCode",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "DepotName",
                table: "Employee");
        }
    }
}
