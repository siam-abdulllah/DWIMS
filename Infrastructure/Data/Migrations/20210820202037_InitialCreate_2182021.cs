using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_2182021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorHonAppr_InvestmentInit_InvestmentInitId",
                table: "DoctorHonAppr");

            migrationBuilder.AlterColumn<int>(
                name: "InvestmentInitId",
                table: "DoctorHonAppr",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorHonAppr_InvestmentInit_InvestmentInitId",
                table: "DoctorHonAppr",
                column: "InvestmentInitId",
                principalTable: "InvestmentInit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorHonAppr_InvestmentInit_InvestmentInitId",
                table: "DoctorHonAppr");

            migrationBuilder.AlterColumn<int>(
                name: "InvestmentInitId",
                table: "DoctorHonAppr",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorHonAppr_InvestmentInit_InvestmentInitId",
                table: "DoctorHonAppr",
                column: "InvestmentInitId",
                principalTable: "InvestmentInit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
