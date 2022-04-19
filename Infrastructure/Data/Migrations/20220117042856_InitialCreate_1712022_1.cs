using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_1712022_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "DepotPrintTrack",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChequeNo",
                table: "DepotPrintTrack",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankName",
                table: "DepotPrintTrack");

            migrationBuilder.DropColumn(
                name: "ChequeNo",
                table: "DepotPrintTrack");
        }
    }
}
