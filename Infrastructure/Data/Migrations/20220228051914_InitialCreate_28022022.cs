using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_28022022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CommitmentFromDate",
                table: "InvestmentApr",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CommitmentToDate",
                table: "InvestmentApr",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CommitmentTotalMonth",
                table: "InvestmentApr",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PaymentFreq",
                table: "InvestmentApr",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommitmentFromDate",
                table: "InvestmentApr");

            migrationBuilder.DropColumn(
                name: "CommitmentToDate",
                table: "InvestmentApr");

            migrationBuilder.DropColumn(
                name: "CommitmentTotalMonth",
                table: "InvestmentApr");

            migrationBuilder.DropColumn(
                name: "PaymentFreq",
                table: "InvestmentApr");
        }
    }
}
