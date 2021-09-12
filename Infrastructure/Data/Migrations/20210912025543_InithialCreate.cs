using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InithialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApprovalAuthority",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    ApprovalAuthorityName = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Priority = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalAuthority", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bcds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    BcdsName = table.Column<string>(nullable: true),
                    BcdsAddress = table.Column<string>(nullable: true),
                    NoOfMember = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bcds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BrandInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    BrandName = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CampaignMst",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    CampaignNo = table.Column<string>(nullable: true),
                    CampaignName = table.Column<string>(nullable: true),
                    SBU = table.Column<string>(nullable: true),
                    BrandCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignMst", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DoctorInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    DoctorCode = table.Column<string>(nullable: true),
                    DoctorName = table.Column<string>(nullable: true),
                    PatientPerDay = table.Column<int>(nullable: false),
                    AvgPrescValue = table.Column<int>(nullable: false),
                    Degree = table.Column<string>(nullable: true),
                    Designation = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DoctorMarket",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    DoctorCode = table.Column<int>(nullable: false),
                    MarketCode = table.Column<string>(nullable: true),
                    MarketName = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    SBU = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorMarket", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Donation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    DonationTypeName = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    EmployeeSAPCode = table.Column<string>(nullable: true),
                    EmployeeName = table.Column<string>(nullable: true),
                    DepartmentId = table.Column<int>(nullable: true),
                    DepartmentName = table.Column<string>(nullable: true),
                    DesignationId = table.Column<int>(nullable: true),
                    DesignationName = table.Column<string>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    JoiningDate = table.Column<DateTime>(nullable: true),
                    JoiningPlace = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
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
                    MarketGroupName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstitutionInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InstitutionCode = table.Column<string>(nullable: true),
                    InstitutionName = table.Column<string>(nullable: true),
                    InstitutionType = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    NoOfBeds = table.Column<int>(nullable: false),
                    AvgNoAdmtPati = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstitutionMarket",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InstitutionCode = table.Column<int>(nullable: false),
                    MarketCode = table.Column<string>(nullable: true),
                    MarketName = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    SBU = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionMarket", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InvesetmentTypeName = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Priority = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    PostTitle = table.Column<string>(maxLength: 100, nullable: false),
                    PostDescription = table.Column<string>(maxLength: 1000, nullable: true),
                    PostedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    ProductCode = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    BrandCode = table.Column<string>(nullable: true),
                    BrandName = table.Column<string>(nullable: true),
                    SBU = table.Column<string>(nullable: true),
                    SBUName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportConfig",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    ReportName = table.Column<string>(nullable: true),
                    ReportCode = table.Column<string>(nullable: true),
                    ReportFunc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RptDocCampWiseInvestment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    SBUName = table.Column<string>(maxLength: 50, nullable: true),
                    SBUCode = table.Column<string>(maxLength: 10, nullable: true),
                    MarketGroupCode = table.Column<string>(maxLength: 10, nullable: true),
                    MarketGroupName = table.Column<string>(maxLength: 70, nullable: true),
                    MarketCode = table.Column<string>(maxLength: 10, nullable: true),
                    MarketName = table.Column<string>(maxLength: 70, nullable: true),
                    TerritoryCode = table.Column<string>(maxLength: 10, nullable: true),
                    TerritoryName = table.Column<string>(maxLength: 70, nullable: true),
                    RegionCode = table.Column<string>(maxLength: 10, nullable: true),
                    RegionName = table.Column<string>(maxLength: 70, nullable: true),
                    DivisionCode = table.Column<string>(maxLength: 10, nullable: true),
                    DivisionName = table.Column<string>(maxLength: 50, nullable: true),
                    ZoneCode = table.Column<string>(maxLength: 10, nullable: true),
                    ZoneName = table.Column<string>(maxLength: 50, nullable: true),
                    DoctorId = table.Column<string>(maxLength: 10, nullable: true),
                    DoctorName = table.Column<string>(maxLength: 150, nullable: true),
                    InstitutionId = table.Column<string>(maxLength: 10, nullable: true),
                    InstitutionName = table.Column<string>(maxLength: 150, nullable: true),
                    DoctorCategory = table.Column<string>(maxLength: 30, nullable: true),
                    FromDate = table.Column<DateTime>(nullable: false),
                    ToDate = table.Column<DateTime>(nullable: false),
                    InvestmentPresent = table.Column<double>(nullable: false),
                    InvestmentPast = table.Column<double>(nullable: false),
                    Brand = table.Column<string>(maxLength: 50, nullable: true),
                    Campaign = table.Column<string>(maxLength: 50, nullable: true),
                    SubCampaign = table.Column<string>(maxLength: 50, nullable: true),
                    Commitment = table.Column<string>(maxLength: 50, nullable: true),
                    ActualShareBrand = table.Column<string>(maxLength: 10, nullable: true),
                    ActualShare = table.Column<string>(maxLength: 10, nullable: true),
                    CompetitorShare = table.Column<string>(maxLength: 10, nullable: true),
                    NoOfPresc = table.Column<string>(maxLength: 10, nullable: true),
                    NoOfPatient = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RptDocCampWiseInvestment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RptDocLocWiseInvestment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    SBUName = table.Column<string>(maxLength: 50, nullable: true),
                    SBUCode = table.Column<string>(maxLength: 10, nullable: true),
                    MarketGroupCode = table.Column<string>(maxLength: 10, nullable: true),
                    MarketGroupName = table.Column<string>(maxLength: 70, nullable: true),
                    MarketCode = table.Column<string>(maxLength: 10, nullable: true),
                    MarketName = table.Column<string>(maxLength: 70, nullable: true),
                    TerritoryCode = table.Column<string>(maxLength: 10, nullable: true),
                    TerritoryName = table.Column<string>(maxLength: 70, nullable: true),
                    RegionCode = table.Column<string>(maxLength: 10, nullable: true),
                    RegionName = table.Column<string>(maxLength: 70, nullable: true),
                    DivisionCode = table.Column<string>(maxLength: 10, nullable: true),
                    DivisionName = table.Column<string>(maxLength: 50, nullable: true),
                    ZoneCode = table.Column<string>(maxLength: 10, nullable: true),
                    ZoneName = table.Column<string>(maxLength: 50, nullable: true),
                    DoctorId = table.Column<string>(maxLength: 10, nullable: true),
                    DoctorName = table.Column<string>(maxLength: 150, nullable: true),
                    InstitutionId = table.Column<string>(maxLength: 10, nullable: true),
                    InstitutionName = table.Column<string>(maxLength: 150, nullable: true),
                    DonationType = table.Column<string>(maxLength: 20, nullable: true),
                    FromDate = table.Column<DateTime>(nullable: false),
                    ToDate = table.Column<DateTime>(nullable: false),
                    InvestedAmt = table.Column<double>(nullable: false),
                    Commitment = table.Column<string>(maxLength: 50, nullable: true),
                    ActualShare = table.Column<string>(maxLength: 10, nullable: true),
                    CompetitorShare = table.Column<string>(maxLength: 10, nullable: true),
                    NoOfPresc = table.Column<string>(maxLength: 10, nullable: true),
                    NoOfPatient = table.Column<string>(maxLength: 10, nullable: true),
                    Deviation = table.Column<string>(maxLength: 10, nullable: true),
                    LeaderNonLeader = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RptDocLocWiseInvestment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RptInsSocBcdsInvestment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    SBUName = table.Column<string>(maxLength: 50, nullable: true),
                    SBUCode = table.Column<string>(maxLength: 10, nullable: true),
                    MarketGroupCode = table.Column<string>(maxLength: 10, nullable: true),
                    MarketGroupName = table.Column<string>(maxLength: 70, nullable: true),
                    MarketCode = table.Column<string>(maxLength: 10, nullable: true),
                    MarketName = table.Column<string>(maxLength: 70, nullable: true),
                    TerritoryCode = table.Column<string>(maxLength: 10, nullable: true),
                    TerritoryName = table.Column<string>(maxLength: 70, nullable: true),
                    RegionCode = table.Column<string>(maxLength: 10, nullable: true),
                    RegionName = table.Column<string>(maxLength: 70, nullable: true),
                    DivisionCode = table.Column<string>(maxLength: 10, nullable: true),
                    DivisionName = table.Column<string>(maxLength: 50, nullable: true),
                    ZoneCode = table.Column<string>(maxLength: 10, nullable: true),
                    ZoneName = table.Column<string>(maxLength: 50, nullable: true),
                    InstitutionId = table.Column<string>(maxLength: 10, nullable: true),
                    InstitutionName = table.Column<string>(maxLength: 150, nullable: true),
                    SocietyId = table.Column<string>(maxLength: 10, nullable: true),
                    SocietyName = table.Column<string>(maxLength: 150, nullable: true),
                    BcdsId = table.Column<string>(maxLength: 10, nullable: true),
                    BcdsName = table.Column<string>(maxLength: 150, nullable: true),
                    DonationType = table.Column<string>(maxLength: 20, nullable: true),
                    FromDate = table.Column<DateTime>(nullable: false),
                    ToDate = table.Column<DateTime>(nullable: false),
                    InvestedAmt = table.Column<double>(nullable: false),
                    Commitment = table.Column<string>(maxLength: 50, nullable: true),
                    ActualShare = table.Column<string>(maxLength: 10, nullable: true),
                    CompetitorShare = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RptInsSocBcdsInvestment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SBU",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    SBUName = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SBU", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SBUWiseBudget",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    SBU = table.Column<string>(nullable: true),
                    SBUName = table.Column<string>(nullable: true),
                    FromDate = table.Column<DateTimeOffset>(nullable: true),
                    ToDate = table.Column<DateTimeOffset>(nullable: true),
                    Amount = table.Column<long>(nullable: false),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SBUWiseBudget", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Society",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    SocietyName = table.Column<string>(nullable: true),
                    SocietyAddress = table.Column<string>(nullable: true),
                    NoOfMember = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Society", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubCampaign",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    SubCampaignName = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCampaign", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalCeiling",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    ApprovalAuthorityId = table.Column<int>(nullable: false),
                    DonationType = table.Column<string>(nullable: true),
                    InvestmentFrom = table.Column<DateTimeOffset>(nullable: true),
                    InvestmentTo = table.Column<DateTimeOffset>(nullable: true),
                    AmountPerTransacion = table.Column<int>(nullable: false),
                    AmountPerMonth = table.Column<int>(nullable: false),
                    Additional = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalCeiling", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalCeiling_ApprovalAuthority_ApprovalAuthorityId",
                        column: x => x.ApprovalAuthorityId,
                        principalTable: "ApprovalAuthority",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalTimeLimit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    ApprovalAuthorityId = table.Column<int>(nullable: false),
                    TimeLimit = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalTimeLimit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalTimeLimit_ApprovalAuthority_ApprovalAuthorityId",
                        column: x => x.ApprovalAuthorityId,
                        principalTable: "ApprovalAuthority",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApprAuthConfig",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: false),
                    ApprovalAuthorityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprAuthConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprAuthConfig_ApprovalAuthority_ApprovalAuthorityId",
                        column: x => x.ApprovalAuthorityId,
                        principalTable: "ApprovalAuthority",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApprAuthConfig_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentInit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    ReferenceNo = table.Column<string>(nullable: true),
                    ProposeFor = table.Column<string>(nullable: true),
                    DonationType = table.Column<string>(nullable: true),
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
                    MarketGroupName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentInit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentInit_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MarketGroupMst",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    GroupName = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketGroupMst", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarketGroupMst_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostComments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    CommentText = table.Column<string>(nullable: true),
                    NoOfLike = table.Column<int>(nullable: false),
                    NoOfDisLike = table.Column<int>(nullable: false),
                    CommentBy = table.Column<string>(nullable: true),
                    PostId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostComments_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampaignDtl",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    MstId = table.Column<int>(nullable: false),
                    SubCampaignId = table.Column<int>(nullable: false),
                    Budget = table.Column<long>(nullable: false),
                    SubCampStartDate = table.Column<DateTimeOffset>(nullable: false),
                    SubCampEndDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignDtl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignDtl_CampaignMst_MstId",
                        column: x => x.MstId,
                        principalTable: "CampaignMst",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignDtl_SubCampaign_SubCampaignId",
                        column: x => x.SubCampaignId,
                        principalTable: "SubCampaign",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoctorHonAppr",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InvestmentInitId = table.Column<int>(nullable: true),
                    DoctorId = table.Column<int>(nullable: false),
                    HonAmount = table.Column<long>(nullable: false),
                    HonMonth = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorHonAppr", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorHonAppr_DoctorInfo_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "DoctorInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorHonAppr_InvestmentInit_InvestmentInitId",
                        column: x => x.InvestmentInitId,
                        principalTable: "InvestmentInit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentApr",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InvestmentInitId = table.Column<int>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: false),
                    ChequeTitle = table.Column<string>(nullable: true),
                    PaymentMethod = table.Column<string>(nullable: true),
                    CommitmentAllSBU = table.Column<string>(nullable: true),
                    CommitmentOwnSBU = table.Column<string>(nullable: true),
                    FromDate = table.Column<DateTimeOffset>(nullable: false),
                    ToDate = table.Column<DateTimeOffset>(nullable: false),
                    TotalMonth = table.Column<int>(nullable: false),
                    ProposedAmount = table.Column<long>(nullable: false),
                    Purpose = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentApr", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentApr_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentApr_InvestmentInit_InvestmentInitId",
                        column: x => x.InvestmentInitId,
                        principalTable: "InvestmentInit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentAprComment",
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
                    AprStatus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentAprComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentAprComment_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentAprComment_InvestmentInit_InvestmentInitId",
                        column: x => x.InvestmentInitId,
                        principalTable: "InvestmentInit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentAprProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InvestmentInitId = table.Column<int>(nullable: true),
                    SBU = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentAprProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentAprProducts_InvestmentInit_InvestmentInitId",
                        column: x => x.InvestmentInitId,
                        principalTable: "InvestmentInit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvestmentAprProducts_ProductInfo_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentBcds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InvestmentInitId = table.Column<int>(nullable: false),
                    BcdsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentBcds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentBcds_Bcds_BcdsId",
                        column: x => x.BcdsId,
                        principalTable: "Bcds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentBcds_InvestmentInit_InvestmentInitId",
                        column: x => x.InvestmentInitId,
                        principalTable: "InvestmentInit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentDetail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    ChequeTitle = table.Column<string>(nullable: true),
                    PaymentMethod = table.Column<string>(nullable: true),
                    CommitmentAllSBU = table.Column<string>(nullable: true),
                    CommitmentOwnSBU = table.Column<string>(nullable: true),
                    FromDate = table.Column<DateTimeOffset>(nullable: false),
                    ToDate = table.Column<DateTimeOffset>(nullable: false),
                    TotalMonth = table.Column<int>(nullable: false),
                    ProposedAmount = table.Column<long>(nullable: false),
                    Purpose = table.Column<string>(nullable: true),
                    InvestmentInitId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentDetail_InvestmentInit_InvestmentInitId",
                        column: x => x.InvestmentInitId,
                        principalTable: "InvestmentInit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentDoctor",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InvestmentInitId = table.Column<int>(nullable: false),
                    DoctorId = table.Column<int>(nullable: false),
                    InstitutionId = table.Column<int>(nullable: false),
                    DoctorCategory = table.Column<string>(nullable: true),
                    DoctorType = table.Column<string>(nullable: true),
                    PracticeDayPerMonth = table.Column<string>(nullable: true),
                    PatientsPerDay = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentDoctor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentDoctor_DoctorInfo_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "DoctorInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentDoctor_InstitutionInfo_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "InstitutionInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentDoctor_InvestmentInit_InvestmentInitId",
                        column: x => x.InvestmentInitId,
                        principalTable: "InvestmentInit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentInstitution",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InvestmentInitId = table.Column<int>(nullable: false),
                    InstitutionId = table.Column<int>(nullable: false),
                    ResposnsibleDoctorId = table.Column<int>(nullable: false),
                    NoOfBed = table.Column<string>(nullable: true),
                    DepartmentUnit = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentInstitution", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentInstitution_InstitutionInfo_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "InstitutionInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentInstitution_InvestmentInit_InvestmentInitId",
                        column: x => x.InvestmentInitId,
                        principalTable: "InvestmentInit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentInstitution_DoctorInfo_ResposnsibleDoctorId",
                        column: x => x.ResposnsibleDoctorId,
                        principalTable: "DoctorInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentRec",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InvestmentInitId = table.Column<int>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: false),
                    ChequeTitle = table.Column<string>(nullable: true),
                    PaymentMethod = table.Column<string>(nullable: true),
                    CommitmentAllSBU = table.Column<string>(nullable: true),
                    CommitmentOwnSBU = table.Column<string>(nullable: true),
                    FromDate = table.Column<DateTimeOffset>(nullable: false),
                    ToDate = table.Column<DateTimeOffset>(nullable: false),
                    TotalMonth = table.Column<int>(nullable: false),
                    ProposedAmount = table.Column<long>(nullable: false),
                    Purpose = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentRec", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentRec_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentRec_InvestmentInit_InvestmentInitId",
                        column: x => x.InvestmentInitId,
                        principalTable: "InvestmentInit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentRecComment",
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
                    RecStatus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentRecComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentRecComment_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentRecComment_InvestmentInit_InvestmentInitId",
                        column: x => x.InvestmentInitId,
                        principalTable: "InvestmentInit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentRecProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InvestmentInitId = table.Column<int>(nullable: true),
                    SBU = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentRecProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentRecProducts_InvestmentInit_InvestmentInitId",
                        column: x => x.InvestmentInitId,
                        principalTable: "InvestmentInit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvestmentRecProducts_ProductInfo_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentSociety",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InvestmentInitId = table.Column<int>(nullable: false),
                    SocietyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentSociety", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentSociety_InvestmentInit_InvestmentInitId",
                        column: x => x.InvestmentInitId,
                        principalTable: "InvestmentInit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentSociety_Society_SocietyId",
                        column: x => x.SocietyId,
                        principalTable: "Society",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentTargetedProd",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InvestmentInitId = table.Column<int>(nullable: false),
                    SBU = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentTargetedProd", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentTargetedProd_InvestmentInit_InvestmentInitId",
                        column: x => x.InvestmentInitId,
                        principalTable: "InvestmentInit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentTargetedProd_ProductInfo_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentTargetedGroup",
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
                    MarketGroupMstId = table.Column<int>(nullable: true),
                    CompletionStatus = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentTargetedGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentTargetedGroup_InvestmentInit_InvestmentInitId",
                        column: x => x.InvestmentInitId,
                        principalTable: "InvestmentInit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentTargetedGroup_MarketGroupMst_MarketGroupMstId",
                        column: x => x.MarketGroupMstId,
                        principalTable: "MarketGroupMst",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MarketGroupDtl",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    MstId = table.Column<int>(nullable: true),
                    MarketCode = table.Column<string>(nullable: true),
                    MarketName = table.Column<string>(nullable: true),
                    SBU = table.Column<string>(nullable: true),
                    SBUName = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketGroupDtl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarketGroupDtl_MarketGroupMst_MstId",
                        column: x => x.MstId,
                        principalTable: "MarketGroupMst",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CampaignDtlProduct",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    DtlId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignDtlProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignDtlProduct_CampaignDtl_DtlId",
                        column: x => x.DtlId,
                        principalTable: "CampaignDtl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignDtlProduct_ProductInfo_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentCampaign",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InvestmentInitId = table.Column<int>(nullable: false),
                    CampaignDtlId = table.Column<int>(nullable: false),
                    InstitutionId = table.Column<int>(nullable: false),
                    DoctorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentCampaign", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentCampaign_CampaignDtl_CampaignDtlId",
                        column: x => x.CampaignDtlId,
                        principalTable: "CampaignDtl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentCampaign_DoctorInfo_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "DoctorInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentCampaign_InstitutionInfo_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "InstitutionInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentCampaign_InvestmentInit_InvestmentInitId",
                        column: x => x.InvestmentInitId,
                        principalTable: "InvestmentInit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportInvestmentInfo",
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
                    DivisionCode = table.Column<string>(nullable: true),
                    DivisionName = table.Column<string>(nullable: true),
                    NationalCode = table.Column<string>(nullable: true),
                    BcdsId = table.Column<string>(nullable: true),
                    BcdsId1 = table.Column<int>(nullable: true),
                    SocietyId = table.Column<string>(nullable: true),
                    InvestmentSocietyId = table.Column<int>(nullable: true),
                    DoctorId = table.Column<string>(nullable: true),
                    DoctorInfoId = table.Column<int>(nullable: true),
                    InstituteId = table.Column<string>(nullable: true),
                    InstitutionInfoId = table.Column<int>(nullable: true),
                    DonationType = table.Column<string>(nullable: true),
                    ExpenseDetail = table.Column<string>(nullable: true),
                    InvestmentAmount = table.Column<string>(nullable: true),
                    FromDate = table.Column<string>(nullable: true),
                    ToDate = table.Column<string>(nullable: true),
                    PrescribedSharePrcnt = table.Column<string>(nullable: true),
                    PrescribedSharePrcntAll = table.Column<string>(nullable: true),
                    PrescShareFromDate = table.Column<string>(nullable: true),
                    PrescShareToDate = table.Column<string>(nullable: true),
                    ComtSharePrcnt = table.Column<string>(nullable: true),
                    ComtSharePrcntAll = table.Column<string>(nullable: true),
                    DonationDuration = table.Column<string>(nullable: true),
                    DonationFromDate = table.Column<string>(nullable: true),
                    DonationToDate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportInvestmentInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportInvestmentInfo_Bcds_BcdsId1",
                        column: x => x.BcdsId1,
                        principalTable: "Bcds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportInvestmentInfo_DoctorInfo_DoctorInfoId",
                        column: x => x.DoctorInfoId,
                        principalTable: "DoctorInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportInvestmentInfo_InstitutionInfo_InstitutionInfoId",
                        column: x => x.InstitutionInfoId,
                        principalTable: "InstitutionInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportInvestmentInfo_InvestmentSociety_InvestmentSocietyId",
                        column: x => x.InvestmentSocietyId,
                        principalTable: "InvestmentSociety",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReportProductInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    ProductCode = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    SBU = table.Column<string>(nullable: true),
                    MarketCode = table.Column<string>(nullable: true),
                    MarketName = table.Column<string>(nullable: true),
                    TerritoryCode = table.Column<string>(nullable: true),
                    TerritoryName = table.Column<string>(nullable: true),
                    RegionCode = table.Column<string>(nullable: true),
                    RegionName = table.Column<string>(nullable: true),
                    ZoneCode = table.Column<string>(nullable: true),
                    ZoneName = table.Column<string>(nullable: true),
                    DivisionCode = table.Column<string>(nullable: true),
                    DivisionName = table.Column<string>(nullable: true),
                    NationalCode = table.Column<string>(nullable: true),
                    SocietyId = table.Column<string>(nullable: true),
                    InvestmentSocietyId = table.Column<int>(nullable: true),
                    DoctorId = table.Column<string>(nullable: true),
                    DoctorInfoId = table.Column<int>(nullable: true),
                    InstituteId = table.Column<string>(nullable: true),
                    InstitutionInfoId = table.Column<int>(nullable: true),
                    DonationType = table.Column<string>(nullable: true),
                    ExpenseDetail = table.Column<string>(nullable: true),
                    InvestmentAmount = table.Column<string>(nullable: true),
                    FromDate = table.Column<string>(nullable: true),
                    ToDate = table.Column<string>(nullable: true),
                    PrescribedSharePrcnt = table.Column<string>(nullable: true),
                    PrescribedSharePrcntAll = table.Column<string>(nullable: true),
                    PrescShareFromDate = table.Column<string>(nullable: true),
                    PrescShareToDate = table.Column<string>(nullable: true),
                    ComtSharePrcnt = table.Column<string>(nullable: true),
                    ComtSharePrcntAll = table.Column<string>(nullable: true),
                    DonationDuration = table.Column<string>(nullable: true),
                    DonationFromDate = table.Column<string>(nullable: true),
                    DonationToDate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportProductInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportProductInfo_DoctorInfo_DoctorInfoId",
                        column: x => x.DoctorInfoId,
                        principalTable: "DoctorInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportProductInfo_InstitutionInfo_InstitutionInfoId",
                        column: x => x.InstitutionInfoId,
                        principalTable: "InstitutionInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportProductInfo_InvestmentSociety_InvestmentSocietyId",
                        column: x => x.InvestmentSocietyId,
                        principalTable: "InvestmentSociety",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApprAuthConfig_ApprovalAuthorityId",
                table: "ApprAuthConfig",
                column: "ApprovalAuthorityId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprAuthConfig_EmployeeId",
                table: "ApprAuthConfig",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalCeiling_ApprovalAuthorityId",
                table: "ApprovalCeiling",
                column: "ApprovalAuthorityId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalTimeLimit_ApprovalAuthorityId",
                table: "ApprovalTimeLimit",
                column: "ApprovalAuthorityId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignDtl_MstId",
                table: "CampaignDtl",
                column: "MstId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignDtl_SubCampaignId",
                table: "CampaignDtl",
                column: "SubCampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignDtlProduct_DtlId",
                table: "CampaignDtlProduct",
                column: "DtlId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignDtlProduct_ProductId",
                table: "CampaignDtlProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorHonAppr_DoctorId",
                table: "DoctorHonAppr",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorHonAppr_InvestmentInitId",
                table: "DoctorHonAppr",
                column: "InvestmentInitId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentApr_EmployeeId",
                table: "InvestmentApr",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentApr_InvestmentInitId",
                table: "InvestmentApr",
                column: "InvestmentInitId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentAprComment_EmployeeId",
                table: "InvestmentAprComment",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentAprComment_InvestmentInitId",
                table: "InvestmentAprComment",
                column: "InvestmentInitId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentAprProducts_InvestmentInitId",
                table: "InvestmentAprProducts",
                column: "InvestmentInitId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentAprProducts_ProductId",
                table: "InvestmentAprProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentBcds_BcdsId",
                table: "InvestmentBcds",
                column: "BcdsId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentBcds_InvestmentInitId",
                table: "InvestmentBcds",
                column: "InvestmentInitId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentCampaign_CampaignDtlId",
                table: "InvestmentCampaign",
                column: "CampaignDtlId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentCampaign_DoctorId",
                table: "InvestmentCampaign",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentCampaign_InstitutionId",
                table: "InvestmentCampaign",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentCampaign_InvestmentInitId",
                table: "InvestmentCampaign",
                column: "InvestmentInitId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentDetail_InvestmentInitId",
                table: "InvestmentDetail",
                column: "InvestmentInitId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentDoctor_DoctorId",
                table: "InvestmentDoctor",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentDoctor_InstitutionId",
                table: "InvestmentDoctor",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentDoctor_InvestmentInitId",
                table: "InvestmentDoctor",
                column: "InvestmentInitId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentInit_EmployeeId",
                table: "InvestmentInit",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentInstitution_InstitutionId",
                table: "InvestmentInstitution",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentInstitution_InvestmentInitId",
                table: "InvestmentInstitution",
                column: "InvestmentInitId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentInstitution_ResposnsibleDoctorId",
                table: "InvestmentInstitution",
                column: "ResposnsibleDoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentRec_EmployeeId",
                table: "InvestmentRec",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentRec_InvestmentInitId",
                table: "InvestmentRec",
                column: "InvestmentInitId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentRecComment_EmployeeId",
                table: "InvestmentRecComment",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentRecComment_InvestmentInitId",
                table: "InvestmentRecComment",
                column: "InvestmentInitId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentRecProducts_InvestmentInitId",
                table: "InvestmentRecProducts",
                column: "InvestmentInitId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentRecProducts_ProductId",
                table: "InvestmentRecProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentSociety_InvestmentInitId",
                table: "InvestmentSociety",
                column: "InvestmentInitId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentSociety_SocietyId",
                table: "InvestmentSociety",
                column: "SocietyId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentTargetedGroup_InvestmentInitId",
                table: "InvestmentTargetedGroup",
                column: "InvestmentInitId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentTargetedGroup_MarketGroupMstId",
                table: "InvestmentTargetedGroup",
                column: "MarketGroupMstId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentTargetedProd_InvestmentInitId",
                table: "InvestmentTargetedProd",
                column: "InvestmentInitId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentTargetedProd_ProductId",
                table: "InvestmentTargetedProd",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketGroupDtl_MstId",
                table: "MarketGroupDtl",
                column: "MstId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketGroupMst_EmployeeId",
                table: "MarketGroupMst",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_PostId",
                table: "PostComments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportInvestmentInfo_BcdsId1",
                table: "ReportInvestmentInfo",
                column: "BcdsId1");

            migrationBuilder.CreateIndex(
                name: "IX_ReportInvestmentInfo_DoctorInfoId",
                table: "ReportInvestmentInfo",
                column: "DoctorInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportInvestmentInfo_InstitutionInfoId",
                table: "ReportInvestmentInfo",
                column: "InstitutionInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportInvestmentInfo_InvestmentSocietyId",
                table: "ReportInvestmentInfo",
                column: "InvestmentSocietyId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportProductInfo_DoctorInfoId",
                table: "ReportProductInfo",
                column: "DoctorInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportProductInfo_InstitutionInfoId",
                table: "ReportProductInfo",
                column: "InstitutionInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportProductInfo_InvestmentSocietyId",
                table: "ReportProductInfo",
                column: "InvestmentSocietyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprAuthConfig");

            migrationBuilder.DropTable(
                name: "ApprovalCeiling");

            migrationBuilder.DropTable(
                name: "ApprovalTimeLimit");

            migrationBuilder.DropTable(
                name: "BrandInfo");

            migrationBuilder.DropTable(
                name: "CampaignDtlProduct");

            migrationBuilder.DropTable(
                name: "DoctorHonAppr");

            migrationBuilder.DropTable(
                name: "DoctorMarket");

            migrationBuilder.DropTable(
                name: "Donation");

            migrationBuilder.DropTable(
                name: "InstitutionMarket");

            migrationBuilder.DropTable(
                name: "InvestmentApr");

            migrationBuilder.DropTable(
                name: "InvestmentAprComment");

            migrationBuilder.DropTable(
                name: "InvestmentAprProducts");

            migrationBuilder.DropTable(
                name: "InvestmentBcds");

            migrationBuilder.DropTable(
                name: "InvestmentCampaign");

            migrationBuilder.DropTable(
                name: "InvestmentDetail");

            migrationBuilder.DropTable(
                name: "InvestmentDoctor");

            migrationBuilder.DropTable(
                name: "InvestmentInstitution");

            migrationBuilder.DropTable(
                name: "InvestmentRec");

            migrationBuilder.DropTable(
                name: "InvestmentRecComment");

            migrationBuilder.DropTable(
                name: "InvestmentRecProducts");

            migrationBuilder.DropTable(
                name: "InvestmentTargetedGroup");

            migrationBuilder.DropTable(
                name: "InvestmentTargetedProd");

            migrationBuilder.DropTable(
                name: "InvestmentType");

            migrationBuilder.DropTable(
                name: "MarketGroupDtl");

            migrationBuilder.DropTable(
                name: "PostComments");

            migrationBuilder.DropTable(
                name: "ReportConfig");

            migrationBuilder.DropTable(
                name: "ReportInvestmentInfo");

            migrationBuilder.DropTable(
                name: "ReportProductInfo");

            migrationBuilder.DropTable(
                name: "RptDocCampWiseInvestment");

            migrationBuilder.DropTable(
                name: "RptDocLocWiseInvestment");

            migrationBuilder.DropTable(
                name: "RptInsSocBcdsInvestment");

            migrationBuilder.DropTable(
                name: "SBU");

            migrationBuilder.DropTable(
                name: "SBUWiseBudget");

            migrationBuilder.DropTable(
                name: "ApprovalAuthority");

            migrationBuilder.DropTable(
                name: "CampaignDtl");

            migrationBuilder.DropTable(
                name: "ProductInfo");

            migrationBuilder.DropTable(
                name: "MarketGroupMst");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "Bcds");

            migrationBuilder.DropTable(
                name: "DoctorInfo");

            migrationBuilder.DropTable(
                name: "InstitutionInfo");

            migrationBuilder.DropTable(
                name: "InvestmentSociety");

            migrationBuilder.DropTable(
                name: "CampaignMst");

            migrationBuilder.DropTable(
                name: "SubCampaign");

            migrationBuilder.DropTable(
                name: "InvestmentInit");

            migrationBuilder.DropTable(
                name: "Society");

            migrationBuilder.DropTable(
                name: "Employee");
        }
    }
}
