using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CompletionStatus",
                table: "InvestmentRecComment",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "InvestmentRecComment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "CompletionStatus",
                table: "InvestmentRec",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "InvestmentRec",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletionStatus",
                table: "InvestmentRecComment");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "InvestmentRecComment");

            migrationBuilder.DropColumn(
                name: "CompletionStatus",
                table: "InvestmentRec");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "InvestmentRec");
        }
    }
}
