using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_14042021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DOnationTypeName",
                table: "Donation",
                newName: "DonationTypeName");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Donation",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Donation");

            migrationBuilder.RenameColumn(
                name: "DonationTypeName",
                table: "Donation",
                newName: "DOnationTypeName");
        }
    }
}
