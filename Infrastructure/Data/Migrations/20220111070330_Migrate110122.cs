using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Migrate110122 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DepotPrintTrack",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InvestmentInitId = table.Column<int>(nullable: true),
                    DepotId = table.Column<string>(nullable: true),
                    DepotName = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: true),
                    LastPrintTime = table.Column<DateTimeOffset>(nullable: true),
                    PrintCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepotPrintTrack", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepotPrintTrack_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepotPrintTrack_InvestmentInit_InvestmentInitId",
                        column: x => x.InvestmentInitId,
                        principalTable: "InvestmentInit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeLocation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    MarketCode = table.Column<string>(nullable: true),
                    MarketName = table.Column<string>(nullable: true),
                    TerritoryCode = table.Column<string>(nullable: true),
                    TerritoryName = table.Column<string>(nullable: true),
                    RegionCode = table.Column<string>(nullable: true),
                    RegionName = table.Column<string>(nullable: true),
                    ZoneCode = table.Column<string>(nullable: true),
                    ZoneName = table.Column<string>(nullable: true),
                    EmployeeName = table.Column<string>(nullable: true),
                    Priority = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeLocation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RptDepotLetterSearch",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    ReferenceNo = table.Column<string>(nullable: true),
                    DonationTypeName = table.Column<string>(nullable: true),
                    ProposeFor = table.Column<string>(nullable: true),
                    DoctorName = table.Column<string>(nullable: true),
                    ProposedAmount = table.Column<long>(nullable: false),
                    MarketName = table.Column<string>(nullable: true),
                    EmployeeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RptDepotLetterSearch", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepotPrintTrack_EmployeeId",
                table: "DepotPrintTrack",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_DepotPrintTrack_InvestmentInitId",
                table: "DepotPrintTrack",
                column: "InvestmentInitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepotPrintTrack");

            migrationBuilder.DropTable(
                name: "EmployeeLocation");

            migrationBuilder.DropTable(
                name: "RptDepotLetterSearch");
        }
    }
}
