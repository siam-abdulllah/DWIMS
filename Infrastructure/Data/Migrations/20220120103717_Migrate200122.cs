using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Migrate200122 : Migration
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

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "EmployeeLocation",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MedicineDispatch",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InvestmentInitId = table.Column<int>(nullable: false),
                    IssueReference = table.Column<string>(nullable: true),
                    IssueDate = table.Column<DateTimeOffset>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: true),
                    ProposeAmt = table.Column<double>(nullable: false),
                    DispatchAmt = table.Column<double>(nullable: false),
                    DepotCode = table.Column<string>(nullable: true),
                    DepotName = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineDispatch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicineDispatch_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicineDispatch_InvestmentInit_InvestmentInitId",
                        column: x => x.InvestmentInitId,
                        principalTable: "InvestmentInit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicineDispatch_EmployeeId",
                table: "MedicineDispatch",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineDispatch_InvestmentInitId",
                table: "MedicineDispatch",
                column: "InvestmentInitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicineDispatch");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "EmployeeLocation");

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
