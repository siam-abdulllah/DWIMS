using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_22022022_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CommitmentFromDate",
                table: "InvestmentRec",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CommitmentToDate",
                table: "InvestmentRec",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CommitmentTotalMonth",
                table: "InvestmentRec",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PaymentFreq",
                table: "InvestmentRec",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommitmentFromDate",
                table: "InvestmentRec");

            migrationBuilder.DropColumn(
                name: "CommitmentToDate",
                table: "InvestmentRec");

            migrationBuilder.DropColumn(
                name: "CommitmentTotalMonth",
                table: "InvestmentRec");

            migrationBuilder.DropColumn(
                name: "PaymentFreq",
                table: "InvestmentRec");
        }
    }
}
