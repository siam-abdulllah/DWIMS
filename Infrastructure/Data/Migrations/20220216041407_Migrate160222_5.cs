using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Migrate160222_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IssueReference",
                table: "MedicineDispatch",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentRefNo",
                table: "DepotPrintTrack",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssueReference",
                table: "MedicineDispatch");

            migrationBuilder.DropColumn(
                name: "PaymentRefNo",
                table: "DepotPrintTrack");
        }
    }
}
