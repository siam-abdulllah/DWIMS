using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_2572021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampaignMst_BrandInfo_BrandId",
                table: "CampaignMst");

            migrationBuilder.DropIndex(
                name: "IX_CampaignMst_BrandId",
                table: "CampaignMst");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "CampaignMst");

            migrationBuilder.AddColumn<string>(
                name: "BrandCode",
                table: "CampaignMst",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrandCode",
                table: "CampaignMst");

            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "CampaignMst",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CampaignMst_BrandId",
                table: "CampaignMst",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignMst_BrandInfo_BrandId",
                table: "CampaignMst",
                column: "BrandId",
                principalTable: "BrandInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
