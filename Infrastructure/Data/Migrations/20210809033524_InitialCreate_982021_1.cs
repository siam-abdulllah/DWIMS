using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_982021_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentTargetedGroup_MarketGroupDtl_MarketGroupMstId",
                table: "InvestmentTargetedGroup");

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentTargetedGroup_MarketGroupMst_MarketGroupMstId",
                table: "InvestmentTargetedGroup",
                column: "MarketGroupMstId",
                principalTable: "MarketGroupMst",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentTargetedGroup_MarketGroupMst_MarketGroupMstId",
                table: "InvestmentTargetedGroup");

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentTargetedGroup_MarketGroupDtl_MarketGroupMstId",
                table: "InvestmentTargetedGroup",
                column: "MarketGroupMstId",
                principalTable: "MarketGroupDtl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
