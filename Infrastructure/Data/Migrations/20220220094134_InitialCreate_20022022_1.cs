using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_20022022_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvestmentInitId",
                table: "RptDepotLetterSearch",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InvestmentInitId",
                table: "MedDispSearch",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PayRefNo",
                table: "MedDispSearch",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CommitmentFromDate",
                table: "InvestmentDetail",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CommitmentToDate",
                table: "InvestmentDetail",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CommitmentTotalMonth",
                table: "InvestmentDetail",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PaymentFreq",
                table: "InvestmentDetail",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvestmentInitId",
                table: "RptDepotLetterSearch");

            migrationBuilder.DropColumn(
                name: "InvestmentInitId",
                table: "MedDispSearch");

            migrationBuilder.DropColumn(
                name: "PayRefNo",
                table: "MedDispSearch");

            migrationBuilder.DropColumn(
                name: "CommitmentFromDate",
                table: "InvestmentDetail");

            migrationBuilder.DropColumn(
                name: "CommitmentToDate",
                table: "InvestmentDetail");

            migrationBuilder.DropColumn(
                name: "CommitmentTotalMonth",
                table: "InvestmentDetail");

            migrationBuilder.DropColumn(
                name: "PaymentFreq",
                table: "InvestmentDetail");
        }
    }
}
