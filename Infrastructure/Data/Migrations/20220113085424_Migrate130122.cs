using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Migrate130122 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChequeTitle",
                table: "RptDepotLetter",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DonationTo",
                table: "RptDepotLetter",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecStatus",
                table: "EmployeeLocation",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PaymentDate",
                table: "DepotPrintTrack",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentRefNo",
                table: "DepotPrintTrack",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChequeTitle",
                table: "RptDepotLetter");

            migrationBuilder.DropColumn(
                name: "DonationTo",
                table: "RptDepotLetter");

            migrationBuilder.DropColumn(
                name: "RecStatus",
                table: "EmployeeLocation");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "DepotPrintTrack");

            migrationBuilder.DropColumn(
                name: "PaymentRefNo",
                table: "DepotPrintTrack");
        }
    }
}
