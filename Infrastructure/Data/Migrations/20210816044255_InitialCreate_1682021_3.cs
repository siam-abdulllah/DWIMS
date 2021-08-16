using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_1682021_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvestmentPurpose",
                table: "InvestmentRec");

            migrationBuilder.DropColumn(
                name: "ProposedAmt",
                table: "InvestmentRec");

            migrationBuilder.DropColumn(
                name: "InvestmentPurpose",
                table: "InvestmentApr");

            migrationBuilder.DropColumn(
                name: "ProposedAmt",
                table: "InvestmentApr");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ToDate",
                table: "InvestmentRec",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "FromDate",
                table: "InvestmentRec",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<long>(
                name: "ProposedAmount",
                table: "InvestmentRec",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Purpose",
                table: "InvestmentRec",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TotalMonth",
                table: "InvestmentRec",
                nullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ToDate",
                table: "InvestmentApr",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "FromDate",
                table: "InvestmentApr",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<long>(
                name: "ProposedAmount",
                table: "InvestmentApr",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Purpose",
                table: "InvestmentApr",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TotalMonth",
                table: "InvestmentApr",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProposedAmount",
                table: "InvestmentRec");

            migrationBuilder.DropColumn(
                name: "Purpose",
                table: "InvestmentRec");

            migrationBuilder.DropColumn(
                name: "TotalMonth",
                table: "InvestmentRec");

            migrationBuilder.DropColumn(
                name: "ProposedAmount",
                table: "InvestmentApr");

            migrationBuilder.DropColumn(
                name: "Purpose",
                table: "InvestmentApr");

            migrationBuilder.DropColumn(
                name: "TotalMonth",
                table: "InvestmentApr");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ToDate",
                table: "InvestmentRec",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AlterColumn<DateTime>(
                name: "FromDate",
                table: "InvestmentRec",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AddColumn<string>(
                name: "InvestmentPurpose",
                table: "InvestmentRec",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ProposedAmt",
                table: "InvestmentRec",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ToDate",
                table: "InvestmentApr",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AlterColumn<DateTime>(
                name: "FromDate",
                table: "InvestmentApr",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AddColumn<string>(
                name: "InvestmentPurpose",
                table: "InvestmentApr",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ProposedAmt",
                table: "InvestmentApr",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
