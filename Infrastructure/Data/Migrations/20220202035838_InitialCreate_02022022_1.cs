using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_02022022_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvStatusCount",
                table: "RptInvestmentSummary",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "ApprovalCeiling",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "ApprovalCeiling",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MedDispSearch",
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
                    ProposedAmount = table.Column<double>(nullable: false),
                    MarketName = table.Column<string>(nullable: true),
                    EmployeeName = table.Column<string>(nullable: true),
                    ApprovedBy = table.Column<string>(nullable: true),
                    DepotName = table.Column<string>(nullable: true),
                    ApprovedDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedDispSearch", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RptMedDisp",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    ReferenceNo = table.Column<string>(nullable: true),
                    DonationTypeName = table.Column<string>(nullable: true),
                    EmployeeName = table.Column<string>(nullable: true),
                    ApprovedBy = table.Column<string>(nullable: true),
                    ApprovedDate = table.Column<DateTimeOffset>(nullable: false),
                    MarketName = table.Column<string>(nullable: true),
                    PaymentRefNo = table.Column<string>(nullable: true),
                    PaymentDate = table.Column<DateTimeOffset>(nullable: true),
                    DispatchAmt = table.Column<double>(nullable: true),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RptMedDisp", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedDispSearch");

            migrationBuilder.DropTable(
                name: "RptMedDisp");

            migrationBuilder.DropColumn(
                name: "InvStatusCount",
                table: "RptInvestmentSummary");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "ApprovalCeiling");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "ApprovalCeiling");
        }
    }
}
