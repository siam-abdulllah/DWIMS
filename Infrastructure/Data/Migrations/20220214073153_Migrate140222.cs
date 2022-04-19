using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Migrate140222 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Confirmation",
                table: "RptInvestmentSummary",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "DId",
                table: "RptInvestmentSummary",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepotName",
                table: "RptInvestmentSummary",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "RptInvestmentSummary",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProposeFor",
                table: "RptInvestmentSummary",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SBUName",
                table: "RptInvestmentSummary",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "RptDepotLetterSearch",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedDate",
                table: "RptDepotLetterSearch",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DId",
                table: "RptDepotLetterSearch",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DonationTo",
                table: "RptDepotLetterSearch",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "RptDepotLetter",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BudgetCeilingForCampaign",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    CampaignBudget = table.Column<double>(nullable: false),
                    TotalExpense = table.Column<double>(nullable: false),
                    TotalRemaining = table.Column<double>(nullable: false),
                    DonationId = table.Column<int>(nullable: false),
                    SBU = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetCeilingForCampaign", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChangeDepot",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InvestmentInitId = table.Column<int>(nullable: false),
                    ChangeDate = table.Column<DateTimeOffset>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: true),
                    Remarks = table.Column<double>(nullable: false),
                    OldDepotCode = table.Column<string>(nullable: true),
                    DepotCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeDepot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangeDepot_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeDepot_InvestmentInit_InvestmentInitId",
                        column: x => x.InvestmentInitId,
                        principalTable: "InvestmentInit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChangeDepotSearch",
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
                    ProposeFor = table.Column<string>(nullable: true),
                    DoctorName = table.Column<string>(nullable: true),
                    ProposedAmount = table.Column<double>(nullable: false),
                    MarketName = table.Column<string>(nullable: true),
                    EmployeeName = table.Column<string>(nullable: true),
                    DepotCode = table.Column<string>(nullable: true),
                    DepotName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeDepotSearch", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentInitForApr",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    ReferenceNo = table.Column<string>(nullable: true),
                    ProposeFor = table.Column<string>(nullable: true),
                    DonationId = table.Column<int>(nullable: false),
                    DonationTo = table.Column<string>(nullable: true),
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
                    RemainingSBU = table.Column<string>(nullable: true),
                    Confirmation = table.Column<bool>(nullable: false),
                    SubmissionDate = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentInitForApr", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentInitForApr_Donation_DonationId",
                        column: x => x.DonationId,
                        principalTable: "Donation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentInitForApr_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentRcvPending",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    ReferenceNo = table.Column<string>(nullable: true),
                    ProposeFor = table.Column<string>(nullable: true),
                    DonationId = table.Column<int>(nullable: false),
                    DonationTo = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: false),
                    SBU = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
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
                    Confirmation = table.Column<bool>(nullable: false),
                    SubmissionDate = table.Column<DateTimeOffset>(nullable: true),
                    ApprovedBy = table.Column<string>(nullable: true),
                    ApprovedDate = table.Column<DateTimeOffset>(nullable: true),
                    DepotName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentRcvPending", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentRcvPending_Donation_DonationId",
                        column: x => x.DonationId,
                        principalTable: "Donation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentRcvPending_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChangeDepot_EmployeeId",
                table: "ChangeDepot",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeDepot_InvestmentInitId",
                table: "ChangeDepot",
                column: "InvestmentInitId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentInitForApr_DonationId",
                table: "InvestmentInitForApr",
                column: "DonationId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentInitForApr_EmployeeId",
                table: "InvestmentInitForApr",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentRcvPending_DonationId",
                table: "InvestmentRcvPending",
                column: "DonationId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentRcvPending_EmployeeId",
                table: "InvestmentRcvPending",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetCeilingForCampaign");

            migrationBuilder.DropTable(
                name: "ChangeDepot");

            migrationBuilder.DropTable(
                name: "ChangeDepotSearch");

            migrationBuilder.DropTable(
                name: "InvestmentInitForApr");

            migrationBuilder.DropTable(
                name: "InvestmentRcvPending");

            migrationBuilder.DropColumn(
                name: "Confirmation",
                table: "RptInvestmentSummary");

            migrationBuilder.DropColumn(
                name: "DId",
                table: "RptInvestmentSummary");

            migrationBuilder.DropColumn(
                name: "DepotName",
                table: "RptInvestmentSummary");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "RptInvestmentSummary");

            migrationBuilder.DropColumn(
                name: "ProposeFor",
                table: "RptInvestmentSummary");

            migrationBuilder.DropColumn(
                name: "SBUName",
                table: "RptInvestmentSummary");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "RptDepotLetterSearch");

            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                table: "RptDepotLetterSearch");

            migrationBuilder.DropColumn(
                name: "DId",
                table: "RptDepotLetterSearch");

            migrationBuilder.DropColumn(
                name: "DonationTo",
                table: "RptDepotLetterSearch");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "RptDepotLetter");
        }
    }
}
