using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_15112021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DonationType",
                table: "InvestmentInit");

            migrationBuilder.AddColumn<int>(
                name: "DonationId",
                table: "InvestmentInit",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentInit_DonationId",
                table: "InvestmentInit",
                column: "DonationId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentInit_Donation_DonationId",
                table: "InvestmentInit",
                column: "DonationId",
                principalTable: "Donation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentInit_Donation_DonationId",
                table: "InvestmentInit");

            migrationBuilder.DropIndex(
                name: "IX_InvestmentInit_DonationId",
                table: "InvestmentInit");

            migrationBuilder.DropColumn(
                name: "DonationId",
                table: "InvestmentInit");

            migrationBuilder.AddColumn<string>(
                name: "DonationType",
                table: "InvestmentInit",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
