using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InithialCreate4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorMarket_DoctorInfo_DoctorId",
                table: "DoctorMarket");

            migrationBuilder.DropForeignKey(
                name: "FK_InstitutionMarket_InstitutionInfo_InstitutionId",
                table: "InstitutionMarket");

            migrationBuilder.DropIndex(
                name: "IX_InstitutionMarket_InstitutionId",
                table: "InstitutionMarket");

            migrationBuilder.DropIndex(
                name: "IX_DoctorMarket_DoctorId",
                table: "DoctorMarket");

            migrationBuilder.DropColumn(
                name: "InstitutionId",
                table: "InstitutionMarket");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "DoctorMarket");

            migrationBuilder.AddColumn<int>(
                name: "InstitutionCode",
                table: "InstitutionMarket",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DoctorCode",
                table: "DoctorMarket",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AvgPrescValue",
                table: "DoctorInfo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PatientPerDay",
                table: "DoctorInfo",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstitutionCode",
                table: "InstitutionMarket");

            migrationBuilder.DropColumn(
                name: "DoctorCode",
                table: "DoctorMarket");

            migrationBuilder.DropColumn(
                name: "AvgPrescValue",
                table: "DoctorInfo");

            migrationBuilder.DropColumn(
                name: "PatientPerDay",
                table: "DoctorInfo");

            migrationBuilder.AddColumn<int>(
                name: "InstitutionId",
                table: "InstitutionMarket",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "DoctorMarket",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionMarket_InstitutionId",
                table: "InstitutionMarket",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorMarket_DoctorId",
                table: "DoctorMarket",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorMarket_DoctorInfo_DoctorId",
                table: "DoctorMarket",
                column: "DoctorId",
                principalTable: "DoctorInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstitutionMarket_InstitutionInfo_InstitutionId",
                table: "InstitutionMarket",
                column: "InstitutionId",
                principalTable: "InstitutionInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
