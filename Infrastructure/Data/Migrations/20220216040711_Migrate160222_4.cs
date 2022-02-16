using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Migrate160222_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssueReference",
                table: "MedicineDispatch");

            migrationBuilder.DropColumn(
                name: "PaymentRefNo",
                table: "DepotPrintTrack");

            migrationBuilder.AddColumn<string>(
                name: "SAPRefNo",
                table: "MedicineDispatch",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SAPRefNo",
                table: "DepotPrintTrack",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SAPRefNo",
                table: "MedicineDispatch");

            migrationBuilder.DropColumn(
                name: "SAPRefNo",
                table: "DepotPrintTrack");

            migrationBuilder.AddColumn<string>(
                name: "IssueReference",
                table: "MedicineDispatch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentRefNo",
                table: "DepotPrintTrack",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
