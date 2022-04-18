using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_18042022_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
           

           

           
            

            migrationBuilder.CreateTable(
                name: "DocLocMap",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    DoctorCode = table.Column<int>(nullable: false),
                    DoctorName = table.Column<string>(nullable: true),
                    Designation = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    MarketCode = table.Column<string>(nullable: true),
                    MarketName = table.Column<string>(nullable: true),
                    SBU = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocLocMap", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentRapid",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InvestmentInitId = table.Column<int>(nullable: false),
                    ReferenceNo = table.Column<string>(nullable: true),
                    ProposeFor = table.Column<string>(nullable: true),
                    SBU = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    PropsalDate = table.Column<DateTime>(nullable: false),
                    DonationTo = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    ProposedAmount = table.Column<double>(nullable: false),
                    PaymentMethod = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    ChequeTitle = table.Column<string>(nullable: true),
                    DepotName = table.Column<string>(nullable: true),
                    DepotCode = table.Column<string>(nullable: true),
                    InitiatorId = table.Column<int>(nullable: false),
                    SubCampaignId = table.Column<int>(nullable: false),
                    SubCampaignName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentRapid", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentRapidAppr",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InvestmentInitId = table.Column<int>(nullable: false),
                    InvestmentRapidId = table.Column<int>(nullable: false),
                    ApprovedBy = table.Column<int>(nullable: false),
                    ApprovalRemarks = table.Column<string>(nullable: true),
                    ApprovedStatus = table.Column<string>(nullable: true),
                    ApprovalAuthId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentRapidAppr", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RptCampaignSummary",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    CampaignName = table.Column<string>(nullable: true),
                    ReferenceNo = table.Column<string>(nullable: true),
                    DonationTypeName = table.Column<string>(nullable: true),
                    SBUName = table.Column<string>(nullable: true),
                    FromDate = table.Column<string>(nullable: true),
                    ToDate = table.Column<string>(nullable: true),
                    InstitutionId = table.Column<int>(nullable: true),
                    InstitutionName = table.Column<string>(nullable: true),
                    DoctorId = table.Column<int>(nullable: true),
                    DoctorName = table.Column<string>(nullable: true),
                    RecStatus = table.Column<string>(nullable: true),
                    Total = table.Column<double>(nullable: true),
                    ProposedAmount = table.Column<double>(nullable: true),
                    MarketCode = table.Column<string>(nullable: true),
                    MarketName = table.Column<string>(nullable: true),
                    TerritoryCode = table.Column<string>(nullable: true),
                    TerritoryName = table.Column<string>(nullable: true),
                    RegionCode = table.Column<string>(nullable: true),
                    RegionName = table.Column<string>(nullable: true),
                    ZoneCode = table.Column<string>(nullable: true),
                    ZoneName = table.Column<string>(nullable: true),
                    PaymentFreq = table.Column<string>(nullable: true),
                    DepotName = table.Column<string>(nullable: true),
                    ApprovedBy = table.Column<string>(nullable: true),
                    PaymentRefNo = table.Column<string>(nullable: true),
                    SAPRefNo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RptCampaignSummary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RptEmpWiseExp",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    ApprovalAuthorityId = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    EmployeeName = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: false),
                    DonationId = table.Column<int>(nullable: false),
                    DonationTypeName = table.Column<string>(nullable: true),
                    Month = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Budget = table.Column<double>(nullable: true),
                    Expense = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RptEmpWiseExp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RptSBUWiseExp",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    SBUName = table.Column<string>(nullable: true),
                    Amount = table.Column<long>(nullable: false),
                    Expense = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RptSBUWiseExp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RptSummary",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    ReferenceNo = table.Column<string>(nullable: true),
                    DId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    EmployeeName = table.Column<string>(nullable: true),
                    SBUName = table.Column<string>(nullable: true),
                    MarketName = table.Column<string>(nullable: true),
                    TerritoryName = table.Column<string>(nullable: true),
                    RegionName = table.Column<string>(nullable: true),
                    ZoneName = table.Column<string>(nullable: true),
                    ProposedAmount = table.Column<double>(nullable: true),
                    InvStatus = table.Column<string>(nullable: true),
                    ApprovedBy = table.Column<string>(nullable: true),
                    DonationTypeName = table.Column<string>(nullable: true),
                    DonationTo = table.Column<string>(nullable: true),
                    FromDate = table.Column<string>(nullable: true),
                    ToDate = table.Column<string>(nullable: true),
                    PaymentMethod = table.Column<string>(nullable: true),
                    PaymentFreq = table.Column<string>(nullable: true),
                    DepotName = table.Column<string>(nullable: true),
                    ReceiveStatus = table.Column<string>(nullable: true),
                    ReceiveBy = table.Column<string>(nullable: true),
                    ProposeFor = table.Column<string>(nullable: true),
                    Confirmation = table.Column<bool>(nullable: false),
                    PaymentRefNo = table.Column<string>(nullable: true),
                    SAPRefNo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RptSummary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemSummary",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    Phase = table.Column<string>(nullable: true),
                    SBUA = table.Column<int>(nullable: true),
                    SBUB = table.Column<int>(nullable: true),
                    SBUC = table.Column<int>(nullable: true),
                    SBUD = table.Column<int>(nullable: true),
                    SBUE = table.Column<int>(nullable: true),
                    SBUN = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSummary", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropTable(
                name: "DocLocMap");

            migrationBuilder.DropTable(
                name: "InvestmentRapid");

            migrationBuilder.DropTable(
                name: "InvestmentRapidAppr");

            migrationBuilder.DropTable(
                name: "RptCampaignSummary");

            migrationBuilder.DropTable(
                name: "RptEmpWiseExp");

            migrationBuilder.DropTable(
                name: "RptSBUWiseExp");

            migrationBuilder.DropTable(
                name: "RptSummary");

            migrationBuilder.DropTable(
                name: "SystemSummary");


           
        }
    }
}
