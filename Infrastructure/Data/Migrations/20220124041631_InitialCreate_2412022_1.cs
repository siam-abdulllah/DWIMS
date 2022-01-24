using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_2412022_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ToDate",
                table: "RptInvestmentSummary",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<double>(
                name: "ProposedAmount",
                table: "RptInvestmentSummary",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "FromDate",
                table: "RptInvestmentSummary",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<double>(
                name: "ProposedAmount",
                table: "RptDepotLetterSearch",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<double>(
                name: "ProposedAmount",
                table: "RptDepotLetter",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<double>(
                name: "InvestmentAmount",
                table: "LastFiveInvestmentInfo",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            

            migrationBuilder.CreateTable(
                name: "EmployeePosting",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false),
                    PostingDate = table.Column<DateTimeOffset>(nullable: false),
                    TransferDate = table.Column<DateTimeOffset>(nullable: false),
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
                    DepotCode = table.Column<string>(nullable: true),
                    DepotName = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_EmployeePosting", x => x.Id);
                });

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeePosting");

            


            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ToDate",
                table: "RptInvestmentSummary",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ProposedAmount",
                table: "RptInvestmentSummary",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "FromDate",
                table: "RptInvestmentSummary",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ProposedAmount",
                table: "RptDepotLetterSearch",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<long>(
                name: "ProposedAmount",
                table: "RptDepotLetter",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<long>(
                name: "InvestmentAmount",
                table: "LastFiveInvestmentInfo",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}
