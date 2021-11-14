using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_14112021_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DonationType",
                table: "ApprovalCeiling");

            migrationBuilder.AddColumn<int>(
                name: "DonationId",
                table: "ApprovalCeiling",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalCeiling_DonationId",
                table: "ApprovalCeiling",
                column: "DonationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalCeiling_Donation_DonationId",
                table: "ApprovalCeiling",
                column: "DonationId",
                principalTable: "Donation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalCeiling_Donation_DonationId",
                table: "ApprovalCeiling");

            migrationBuilder.DropIndex(
                name: "IX_ApprovalCeiling_DonationId",
                table: "ApprovalCeiling");

            migrationBuilder.DropColumn(
                name: "DonationId",
                table: "ApprovalCeiling");

            migrationBuilder.AddColumn<string>(
                name: "DonationType",
                table: "ApprovalCeiling",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
