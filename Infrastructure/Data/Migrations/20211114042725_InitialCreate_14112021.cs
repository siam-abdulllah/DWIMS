using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_14112021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DonationId",
                table: "SBUWiseBudget",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DonationTypeId",
                table: "SBUWiseBudget",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SBUWiseBudget_DonationId",
                table: "SBUWiseBudget",
                column: "DonationId");

            migrationBuilder.AddForeignKey(
                name: "FK_SBUWiseBudget_Donation_DonationId",
                table: "SBUWiseBudget",
                column: "DonationId",
                principalTable: "Donation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SBUWiseBudget_Donation_DonationId",
                table: "SBUWiseBudget");

            migrationBuilder.DropIndex(
                name: "IX_SBUWiseBudget_DonationId",
                table: "SBUWiseBudget");

            migrationBuilder.DropColumn(
                name: "DonationId",
                table: "SBUWiseBudget");

            migrationBuilder.DropColumn(
                name: "DonationTypeId",
                table: "SBUWiseBudget");
        }
    }
}
