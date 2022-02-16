using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Migrate160222_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PayRefNo",
                table: "RptDepotLetterSearch",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayRefNo",
                table: "MedicineDispatch",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayRefNo",
                table: "DepotPrintTrack",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayRefNo",
                table: "ChangeDepotSearch",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayRefNo",
                table: "RptDepotLetterSearch");

            migrationBuilder.DropColumn(
                name: "PayRefNo",
                table: "MedicineDispatch");

            migrationBuilder.DropColumn(
                name: "PayRefNo",
                table: "DepotPrintTrack");

            migrationBuilder.DropColumn(
                name: "PayRefNo",
                table: "ChangeDepotSearch");
        }
    }
}
