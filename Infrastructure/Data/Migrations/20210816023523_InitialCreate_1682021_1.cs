using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_1682021_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentApr_InvestmentRec_InvestmentRecId",
                table: "InvestmentApr");

            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentAprComment_InvestmentApr_InvestmentAprId",
                table: "InvestmentAprComment");

            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentAprProducts_InvestmentAprComment_InvestmentAprCmntId",
                table: "InvestmentAprProducts");

            migrationBuilder.DropIndex(
                name: "IX_InvestmentAprProducts_InvestmentAprCmntId",
                table: "InvestmentAprProducts");

            migrationBuilder.DropIndex(
                name: "IX_InvestmentAprComment_InvestmentAprId",
                table: "InvestmentAprComment");

            migrationBuilder.DropIndex(
                name: "IX_InvestmentApr_InvestmentRecId",
                table: "InvestmentApr");

            migrationBuilder.DropColumn(
                name: "InvestmentAprCmntId",
                table: "InvestmentAprProducts");

            migrationBuilder.DropColumn(
                name: "InvestmentAprId",
                table: "InvestmentAprComment");

            migrationBuilder.DropColumn(
                name: "InvestmentRecId",
                table: "InvestmentApr");

            migrationBuilder.AddColumn<int>(
                name: "InvestmentInitId",
                table: "InvestmentAprProducts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SBU",
                table: "InvestmentAprProducts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InvestmentInitId",
                table: "InvestmentAprComment",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InvestmentInitId",
                table: "InvestmentApr",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentAprProducts_InvestmentInitId",
                table: "InvestmentAprProducts",
                column: "InvestmentInitId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentAprComment_InvestmentInitId",
                table: "InvestmentAprComment",
                column: "InvestmentInitId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentApr_InvestmentInitId",
                table: "InvestmentApr",
                column: "InvestmentInitId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentApr_InvestmentInit_InvestmentInitId",
                table: "InvestmentApr",
                column: "InvestmentInitId",
                principalTable: "InvestmentInit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentAprComment_InvestmentInit_InvestmentInitId",
                table: "InvestmentAprComment",
                column: "InvestmentInitId",
                principalTable: "InvestmentInit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentAprProducts_InvestmentInit_InvestmentInitId",
                table: "InvestmentAprProducts",
                column: "InvestmentInitId",
                principalTable: "InvestmentInit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentApr_InvestmentInit_InvestmentInitId",
                table: "InvestmentApr");

            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentAprComment_InvestmentInit_InvestmentInitId",
                table: "InvestmentAprComment");

            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentAprProducts_InvestmentInit_InvestmentInitId",
                table: "InvestmentAprProducts");

            migrationBuilder.DropIndex(
                name: "IX_InvestmentAprProducts_InvestmentInitId",
                table: "InvestmentAprProducts");

            migrationBuilder.DropIndex(
                name: "IX_InvestmentAprComment_InvestmentInitId",
                table: "InvestmentAprComment");

            migrationBuilder.DropIndex(
                name: "IX_InvestmentApr_InvestmentInitId",
                table: "InvestmentApr");

            migrationBuilder.DropColumn(
                name: "InvestmentInitId",
                table: "InvestmentAprProducts");

            migrationBuilder.DropColumn(
                name: "SBU",
                table: "InvestmentAprProducts");

            migrationBuilder.DropColumn(
                name: "InvestmentInitId",
                table: "InvestmentAprComment");

            migrationBuilder.DropColumn(
                name: "InvestmentInitId",
                table: "InvestmentApr");

            migrationBuilder.AddColumn<int>(
                name: "InvestmentAprCmntId",
                table: "InvestmentAprProducts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InvestmentAprId",
                table: "InvestmentAprComment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InvestmentRecId",
                table: "InvestmentApr",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentAprProducts_InvestmentAprCmntId",
                table: "InvestmentAprProducts",
                column: "InvestmentAprCmntId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentAprComment_InvestmentAprId",
                table: "InvestmentAprComment",
                column: "InvestmentAprId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentApr_InvestmentRecId",
                table: "InvestmentApr",
                column: "InvestmentRecId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentApr_InvestmentRec_InvestmentRecId",
                table: "InvestmentApr",
                column: "InvestmentRecId",
                principalTable: "InvestmentRec",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentAprComment_InvestmentApr_InvestmentAprId",
                table: "InvestmentAprComment",
                column: "InvestmentAprId",
                principalTable: "InvestmentApr",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentAprProducts_InvestmentAprComment_InvestmentAprCmntId",
                table: "InvestmentAprProducts",
                column: "InvestmentAprCmntId",
                principalTable: "InvestmentAprComment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
