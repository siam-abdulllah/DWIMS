using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_1682021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentRecComment_InvestmentRec_InvestmentRecId",
                table: "InvestmentRecComment");

            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentRecProducts_InvestmentRecComment_InvestmenRecCmntId",
                table: "InvestmentRecProducts");

            migrationBuilder.DropIndex(
                name: "IX_InvestmentRecProducts_InvestmenRecCmntId",
                table: "InvestmentRecProducts");

            migrationBuilder.DropIndex(
                name: "IX_InvestmentRecComment_InvestmentRecId",
                table: "InvestmentRecComment");

            migrationBuilder.DropColumn(
                name: "InvestmenRecCmntId",
                table: "InvestmentRecProducts");

            migrationBuilder.DropColumn(
                name: "InvestmentRecId",
                table: "InvestmentRecComment");

            migrationBuilder.AddColumn<string>(
                name: "SBU",
                table: "InvestmentTargetedProd",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InvestmentInitId",
                table: "InvestmentRecProducts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SBU",
                table: "InvestmentRecProducts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InvestmentInitId",
                table: "InvestmentRecComment",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentRecProducts_InvestmentInitId",
                table: "InvestmentRecProducts",
                column: "InvestmentInitId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentRecComment_InvestmentInitId",
                table: "InvestmentRecComment",
                column: "InvestmentInitId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentRecComment_InvestmentInit_InvestmentInitId",
                table: "InvestmentRecComment",
                column: "InvestmentInitId",
                principalTable: "InvestmentInit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentRecProducts_InvestmentInit_InvestmentInitId",
                table: "InvestmentRecProducts",
                column: "InvestmentInitId",
                principalTable: "InvestmentInit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentRecComment_InvestmentInit_InvestmentInitId",
                table: "InvestmentRecComment");

            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentRecProducts_InvestmentInit_InvestmentInitId",
                table: "InvestmentRecProducts");

            migrationBuilder.DropIndex(
                name: "IX_InvestmentRecProducts_InvestmentInitId",
                table: "InvestmentRecProducts");

            migrationBuilder.DropIndex(
                name: "IX_InvestmentRecComment_InvestmentInitId",
                table: "InvestmentRecComment");

            migrationBuilder.DropColumn(
                name: "SBU",
                table: "InvestmentTargetedProd");

            migrationBuilder.DropColumn(
                name: "InvestmentInitId",
                table: "InvestmentRecProducts");

            migrationBuilder.DropColumn(
                name: "SBU",
                table: "InvestmentRecProducts");

            migrationBuilder.DropColumn(
                name: "InvestmentInitId",
                table: "InvestmentRecComment");

            migrationBuilder.AddColumn<int>(
                name: "InvestmenRecCmntId",
                table: "InvestmentRecProducts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InvestmentRecId",
                table: "InvestmentRecComment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentRecProducts_InvestmenRecCmntId",
                table: "InvestmentRecProducts",
                column: "InvestmenRecCmntId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentRecProducts_InvestmentRecComment_InvestmenRecCmntId",
                table: "InvestmentRecProducts",
                column: "InvestmenRecCmntId",
                principalTable: "InvestmentRecComment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
