using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class EmpSbuMappingTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SBUName",
                table: "EmpSbuMapping",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PipeLineExpense",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    Pipeline = table.Column<double>(nullable: false),
                    SBUName = table.Column<string>(nullable: true),
                    SBUCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PipeLineExpense", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RptInvestmentSummaryInd",
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
                    Name = table.Column<string>(nullable: true),
                    ProposedAmount = table.Column<double>(nullable: true),
                    RecommendedAmount = table.Column<double>(nullable: true),
                    ApprovedAmount = table.Column<double>(nullable: true),
                    FromDate = table.Column<DateTimeOffset>(nullable: true),
                    ToDate = table.Column<DateTimeOffset>(nullable: true),
                    InvStatus = table.Column<string>(nullable: true),
                    InvStatusCount = table.Column<int>(nullable: false),
                    EmployeeName = table.Column<string>(nullable: true),
                    ReceiveStatus = table.Column<string>(nullable: true),
                    ReceiveBy = table.Column<string>(nullable: true),
                    ApprovedBy = table.Column<string>(nullable: true),
                    MarketName = table.Column<string>(nullable: true),
                    DepotName = table.Column<string>(nullable: true),
                    SBUName = table.Column<string>(nullable: true),
                    ProposeFor = table.Column<string>(nullable: true),
                    PaymentMethod = table.Column<string>(nullable: true),
                    Confirmation = table.Column<bool>(nullable: false),
                    DId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RptInvestmentSummaryInd", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TotalExpense",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    Expense = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalExpense", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PipeLineExpense");

            migrationBuilder.DropTable(
                name: "RptInvestmentSummaryInd");

            migrationBuilder.DropTable(
                name: "TotalExpense");

            migrationBuilder.DropColumn(
                name: "SBUName",
                table: "EmpSbuMapping");
        }
    }
}
