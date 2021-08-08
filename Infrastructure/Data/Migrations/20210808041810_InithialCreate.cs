using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InithialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InstitutionId",
                table: "InvestmentCampaign",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentCampaign_InstitutionId",
                table: "InvestmentCampaign",
                column: "InstitutionId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentCampaign_InstitutionInfo_InstitutionId",
                table: "InvestmentCampaign",
                column: "InstitutionId",
                principalTable: "InstitutionInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentCampaign_InstitutionInfo_InstitutionId",
                table: "InvestmentCampaign");

            migrationBuilder.DropIndex(
                name: "IX_InvestmentCampaign_InstitutionId",
                table: "InvestmentCampaign");

            migrationBuilder.DropColumn(
                name: "InstitutionId",
                table: "InvestmentCampaign");
        }
    }
}
