using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_21062022_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalAuthId",
                table: "InvestmentRapidAppr");

            migrationBuilder.AddColumn<int>(
                name: "ApproverId",
                table: "InvestmentRapidAppr",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApproverId",
                table: "InvestmentRapidAppr");

            migrationBuilder.AddColumn<int>(
                name: "ApprovalAuthId",
                table: "InvestmentRapidAppr",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
