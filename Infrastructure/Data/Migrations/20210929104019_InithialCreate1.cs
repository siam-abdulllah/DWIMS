using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InithialCreate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvestmentDetailTracker",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InvestmentInitId = table.Column<int>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: false),
                    DonationType = table.Column<string>(nullable: true),
                    Month = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    FromDate = table.Column<DateTimeOffset>(nullable: false),
                    ToDate = table.Column<DateTimeOffset>(nullable: false),
                    ApprovedAmount = table.Column<long>(nullable: false),
                    PaidStatus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentDetailTracker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentDetailTracker_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentDetailTracker_InvestmentInit_InvestmentInitId",
                        column: x => x.InvestmentInitId,
                        principalTable: "InvestmentInit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentDetailTracker_EmployeeId",
                table: "InvestmentDetailTracker",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentDetailTracker_InvestmentInitId",
                table: "InvestmentDetailTracker",
                column: "InvestmentInitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvestmentDetailTracker");
        }
    }
}
