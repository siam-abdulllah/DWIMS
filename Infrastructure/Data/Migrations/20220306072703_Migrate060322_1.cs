using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Migrate060322_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentRefNo",
                table: "RptMedDisp");

            migrationBuilder.DropColumn(
                name: "ReferenceNo",
                table: "RptDepotLetterSearch");

            migrationBuilder.DropColumn(
                name: "ReferenceNo",
                table: "MedDispSearch");

            migrationBuilder.AlterColumn<double>(
                name: "Expense",
                table: "RptSBUWiseExpSummart",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "PayRefNo",
                table: "RptMedDisp",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SAPRefNo",
                table: "RptMedDisp",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayRefNo",
                table: "InvestmentRecv",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayRefNo",
                table: "InvestmentRcvPending",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RptChequePrintSearch",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InvestmentInitId = table.Column<int>(nullable: false),
                    PayRefNo = table.Column<string>(nullable: true),
                    DonationTypeName = table.Column<string>(nullable: true),
                    DonationTo = table.Column<string>(nullable: true),
                    ProposeFor = table.Column<string>(nullable: true),
                    DId = table.Column<int>(nullable: false),
                    DoctorName = table.Column<string>(nullable: true),
                    SBUName = table.Column<string>(nullable: true),
                    ChequeTitle = table.Column<string>(nullable: true),
                    ProposedAmount = table.Column<double>(nullable: false),
                    MarketName = table.Column<string>(nullable: true),
                    EmployeeName = table.Column<string>(nullable: true),
                    ApprovedBy = table.Column<string>(nullable: true),
                    ApprovedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RptChequePrintSearch", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RptYearlyBudget",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    DonationTypeName = table.Column<string>(nullable: true),
                    DonationId = table.Column<int>(nullable: false),
                    SBUName = table.Column<string>(nullable: true),
                    Amount = table.Column<long>(nullable: false),
                    Expense = table.Column<double>(nullable: true),
                    January = table.Column<double>(nullable: true),
                    February = table.Column<double>(nullable: true),
                    March = table.Column<double>(nullable: true),
                    April = table.Column<double>(nullable: true),
                    May = table.Column<double>(nullable: true),
                    June = table.Column<double>(nullable: true),
                    July = table.Column<double>(nullable: true),
                    August = table.Column<double>(nullable: true),
                    September = table.Column<double>(nullable: true),
                    October = table.Column<double>(nullable: true),
                    November = table.Column<double>(nullable: true),
                    December = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RptYearlyBudget", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RptChequePrintSearch");

            migrationBuilder.DropTable(
                name: "RptYearlyBudget");

            migrationBuilder.DropColumn(
                name: "PayRefNo",
                table: "RptMedDisp");

            migrationBuilder.DropColumn(
                name: "SAPRefNo",
                table: "RptMedDisp");

            migrationBuilder.DropColumn(
                name: "PayRefNo",
                table: "InvestmentRecv");

            migrationBuilder.DropColumn(
                name: "PayRefNo",
                table: "InvestmentRcvPending");

            migrationBuilder.AlterColumn<long>(
                name: "Expense",
                table: "RptSBUWiseExpSummart",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<string>(
                name: "PaymentRefNo",
                table: "RptMedDisp",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNo",
                table: "RptDepotLetterSearch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNo",
                table: "MedDispSearch",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
