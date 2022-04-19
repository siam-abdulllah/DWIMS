using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate15112021_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DonationType",
                table: "InvestmentDetailTracker");

            migrationBuilder.AddColumn<int>(
                name: "DonationId",
                table: "InvestmentDetailTracker",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentDetailTracker_DonationId",
                table: "InvestmentDetailTracker",
                column: "DonationId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentDetailTracker_Donation_DonationId",
                table: "InvestmentDetailTracker",
                column: "DonationId",
                principalTable: "Donation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentDetailTracker_Donation_DonationId",
                table: "InvestmentDetailTracker");

            migrationBuilder.DropIndex(
                name: "IX_InvestmentDetailTracker_DonationId",
                table: "InvestmentDetailTracker");

            migrationBuilder.DropColumn(
                name: "DonationId",
                table: "InvestmentDetailTracker");

            migrationBuilder.AddColumn<string>(
                name: "DonationType",
                table: "InvestmentDetailTracker",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
