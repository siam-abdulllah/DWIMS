using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Migrate0712212 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvestmentRecv",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InvestmentInitId = table.Column<int>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: false),
                    SBU = table.Column<string>(nullable: true),
                    SBUName = table.Column<string>(nullable: true),
                    ZoneCode = table.Column<string>(nullable: true),
                    ZoneName = table.Column<string>(nullable: true),
                    RegionCode = table.Column<string>(nullable: true),
                    RegionName = table.Column<string>(nullable: true),
                    TerritoryCode = table.Column<string>(nullable: true),
                    TerritoryName = table.Column<string>(nullable: true),
                    MarketCode = table.Column<string>(nullable: true),
                    MarketName = table.Column<string>(nullable: true),
                    MarketGroupCode = table.Column<string>(nullable: true),
                    MarketGroupName = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(nullable: true),
                    ChequeTitle = table.Column<string>(nullable: true),
                    PaymentMethod = table.Column<string>(nullable: true),
                    CommitmentAllSBU = table.Column<string>(nullable: true),
                    CommitmentOwnSBU = table.Column<string>(nullable: true),
                    FromDate = table.Column<DateTimeOffset>(nullable: false),
                    ToDate = table.Column<DateTimeOffset>(nullable: false),
                    TotalMonth = table.Column<int>(nullable: false),
                    ProposedAmount = table.Column<long>(nullable: false),
                    Purpose = table.Column<string>(nullable: true),
                    ReceiveStatus = table.Column<string>(nullable: true),
                    Priority = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentRecv", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentRecv_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentRecv_InvestmentInit_InvestmentInitId",
                        column: x => x.InvestmentInitId,
                        principalTable: "InvestmentInit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentRecv_EmployeeId",
                table: "InvestmentRecv",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentRecv_InvestmentInitId",
                table: "InvestmentRecv",
                column: "InvestmentInitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvestmentRecv");
        }
    }
}
