using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_2282021_O : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ComtSharePrcntAll",
                table: "ReportProductInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrescribedSharePrcntAll",
                table: "ReportProductInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ComtSharePrcntAll",
                table: "ReportInvestmentInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrescribedSharePrcntAll",
                table: "ReportInvestmentInfo",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComtSharePrcntAll",
                table: "ReportProductInfo");

            migrationBuilder.DropColumn(
                name: "PrescribedSharePrcntAll",
                table: "ReportProductInfo");

            migrationBuilder.DropColumn(
                name: "ComtSharePrcntAll",
                table: "ReportInvestmentInfo");

            migrationBuilder.DropColumn(
                name: "PrescribedSharePrcntAll",
                table: "ReportInvestmentInfo");
        }
    }
}
