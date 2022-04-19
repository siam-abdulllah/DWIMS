using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Migrate071221 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DonationType",
                table: "BudgetCeiling");

            migrationBuilder.AddColumn<string>(
                name: "DonationShortName",
                table: "Donation",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DonationId",
                table: "BudgetCeiling",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RptInvestmentSummary",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    ReferenceNo = table.Column<string>(nullable: true),
                    DonationTypeName = table.Column<string>(nullable: true),
                    DonationTo = table.Column<string>(nullable: true),
                    ProposedAmount = table.Column<long>(nullable: false),
                    FromDate = table.Column<DateTimeOffset>(nullable: false),
                    ToDate = table.Column<DateTimeOffset>(nullable: false),
                    InvStatus = table.Column<string>(nullable: true),
                    EmployeeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RptInvestmentSummary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RptSBUWiseExpSummart",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    SBUName = table.Column<string>(nullable: true),
                    SBU = table.Column<string>(nullable: true),
                    DonationTypeName = table.Column<string>(nullable: true),
                    DonationId = table.Column<int>(nullable: false),
                    Expense = table.Column<long>(nullable: false),
                    Budget = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RptSBUWiseExpSummart", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RptInvestmentSummary");

            migrationBuilder.DropTable(
                name: "RptSBUWiseExpSummart");

            migrationBuilder.DropColumn(
                name: "DonationShortName",
                table: "Donation");

            migrationBuilder.DropColumn(
                name: "DonationId",
                table: "BudgetCeiling");

            migrationBuilder.AddColumn<string>(
                name: "DonationType",
                table: "BudgetCeiling",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
