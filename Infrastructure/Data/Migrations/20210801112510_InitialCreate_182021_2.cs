using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_182021_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvestmentInitId",
                table: "InvestmentRecComment");

            migrationBuilder.AddColumn<int>(
                name: "InvestmentRecId",
                table: "InvestmentRecComment",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentRecComment_InvestmentRecId",
                table: "InvestmentRecComment",
                column: "InvestmentRecId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentRecComment_InvestmentRec_InvestmentRecId",
                table: "InvestmentRecComment",
                column: "InvestmentRecId",
                principalTable: "InvestmentRec",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentRecComment_InvestmentRec_InvestmentRecId",
                table: "InvestmentRecComment");

            migrationBuilder.DropIndex(
                name: "IX_InvestmentRecComment_InvestmentRecId",
                table: "InvestmentRecComment");

            migrationBuilder.DropColumn(
                name: "InvestmentRecId",
                table: "InvestmentRecComment");

            migrationBuilder.AddColumn<int>(
                name: "InvestmentInitId",
                table: "InvestmentRecComment",
                type: "int",
                nullable: true);
        }
    }
}
