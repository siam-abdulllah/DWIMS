using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_14112021_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SBUWiseBudget_Donation_DonationId",
                table: "SBUWiseBudget");

            migrationBuilder.DropColumn(
                name: "DonationTypeId",
                table: "SBUWiseBudget");

            migrationBuilder.AlterColumn<int>(
                name: "DonationId",
                table: "SBUWiseBudget",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SBUWiseBudget_Donation_DonationId",
                table: "SBUWiseBudget",
                column: "DonationId",
                principalTable: "Donation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SBUWiseBudget_Donation_DonationId",
                table: "SBUWiseBudget");

            migrationBuilder.AlterColumn<int>(
                name: "DonationId",
                table: "SBUWiseBudget",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "DonationTypeId",
                table: "SBUWiseBudget",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_SBUWiseBudget_Donation_DonationId",
                table: "SBUWiseBudget",
                column: "DonationId",
                principalTable: "Donation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
