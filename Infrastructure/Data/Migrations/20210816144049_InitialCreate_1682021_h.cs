using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_1682021_h : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DivisionName",
                table: "ReportProductInfo",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DoctorInfoId",
                table: "ReportProductInfo",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InstitutionInfoId",
                table: "ReportProductInfo",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InvestmentSocietyId",
                table: "ReportProductInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarketName",
                table: "ReportProductInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegionName",
                table: "ReportProductInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TerritoryName",
                table: "ReportProductInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZoneName",
                table: "ReportProductInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DivisionName",
                table: "ReportInvestmentInfo",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DoctorInfoId",
                table: "ReportInvestmentInfo",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InstitutionInfoId",
                table: "ReportInvestmentInfo",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InvestmentSocietyId",
                table: "ReportInvestmentInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarketName",
                table: "ReportInvestmentInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegionName",
                table: "ReportInvestmentInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TerritoryName",
                table: "ReportInvestmentInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZoneName",
                table: "ReportInvestmentInfo",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReportProductInfo_DoctorInfoId",
                table: "ReportProductInfo",
                column: "DoctorInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportProductInfo_InstitutionInfoId",
                table: "ReportProductInfo",
                column: "InstitutionInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportProductInfo_InvestmentSocietyId",
                table: "ReportProductInfo",
                column: "InvestmentSocietyId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportInvestmentInfo_DoctorInfoId",
                table: "ReportInvestmentInfo",
                column: "DoctorInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportInvestmentInfo_InstitutionInfoId",
                table: "ReportInvestmentInfo",
                column: "InstitutionInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportInvestmentInfo_InvestmentSocietyId",
                table: "ReportInvestmentInfo",
                column: "InvestmentSocietyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportInvestmentInfo_DoctorInfo_DoctorInfoId",
                table: "ReportInvestmentInfo",
                column: "DoctorInfoId",
                principalTable: "DoctorInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportInvestmentInfo_InstitutionInfo_InstitutionInfoId",
                table: "ReportInvestmentInfo",
                column: "InstitutionInfoId",
                principalTable: "InstitutionInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportInvestmentInfo_InvestmentSociety_InvestmentSocietyId",
                table: "ReportInvestmentInfo",
                column: "InvestmentSocietyId",
                principalTable: "InvestmentSociety",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportProductInfo_DoctorInfo_DoctorInfoId",
                table: "ReportProductInfo",
                column: "DoctorInfoId",
                principalTable: "DoctorInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportProductInfo_InstitutionInfo_InstitutionInfoId",
                table: "ReportProductInfo",
                column: "InstitutionInfoId",
                principalTable: "InstitutionInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportProductInfo_InvestmentSociety_InvestmentSocietyId",
                table: "ReportProductInfo",
                column: "InvestmentSocietyId",
                principalTable: "InvestmentSociety",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportInvestmentInfo_DoctorInfo_DoctorInfoId",
                table: "ReportInvestmentInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportInvestmentInfo_InstitutionInfo_InstitutionInfoId",
                table: "ReportInvestmentInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportInvestmentInfo_InvestmentSociety_InvestmentSocietyId",
                table: "ReportInvestmentInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportProductInfo_DoctorInfo_DoctorInfoId",
                table: "ReportProductInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportProductInfo_InstitutionInfo_InstitutionInfoId",
                table: "ReportProductInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportProductInfo_InvestmentSociety_InvestmentSocietyId",
                table: "ReportProductInfo");

            migrationBuilder.DropIndex(
                name: "IX_ReportProductInfo_DoctorInfoId",
                table: "ReportProductInfo");

            migrationBuilder.DropIndex(
                name: "IX_ReportProductInfo_InstitutionInfoId",
                table: "ReportProductInfo");

            migrationBuilder.DropIndex(
                name: "IX_ReportProductInfo_InvestmentSocietyId",
                table: "ReportProductInfo");

            migrationBuilder.DropIndex(
                name: "IX_ReportInvestmentInfo_DoctorInfoId",
                table: "ReportInvestmentInfo");

            migrationBuilder.DropIndex(
                name: "IX_ReportInvestmentInfo_InstitutionInfoId",
                table: "ReportInvestmentInfo");

            migrationBuilder.DropIndex(
                name: "IX_ReportInvestmentInfo_InvestmentSocietyId",
                table: "ReportInvestmentInfo");

            migrationBuilder.DropColumn(
                name: "DivisionName",
                table: "ReportProductInfo");

            migrationBuilder.DropColumn(
                name: "DoctorInfoId",
                table: "ReportProductInfo");

            migrationBuilder.DropColumn(
                name: "InstitutionInfoId",
                table: "ReportProductInfo");

            migrationBuilder.DropColumn(
                name: "InvestmentSocietyId",
                table: "ReportProductInfo");

            migrationBuilder.DropColumn(
                name: "MarketName",
                table: "ReportProductInfo");

            migrationBuilder.DropColumn(
                name: "RegionName",
                table: "ReportProductInfo");

            migrationBuilder.DropColumn(
                name: "TerritoryName",
                table: "ReportProductInfo");

            migrationBuilder.DropColumn(
                name: "ZoneName",
                table: "ReportProductInfo");

            migrationBuilder.DropColumn(
                name: "DivisionName",
                table: "ReportInvestmentInfo");

            migrationBuilder.DropColumn(
                name: "DoctorInfoId",
                table: "ReportInvestmentInfo");

            migrationBuilder.DropColumn(
                name: "InstitutionInfoId",
                table: "ReportInvestmentInfo");

            migrationBuilder.DropColumn(
                name: "InvestmentSocietyId",
                table: "ReportInvestmentInfo");

            migrationBuilder.DropColumn(
                name: "MarketName",
                table: "ReportInvestmentInfo");

            migrationBuilder.DropColumn(
                name: "RegionName",
                table: "ReportInvestmentInfo");

            migrationBuilder.DropColumn(
                name: "TerritoryName",
                table: "ReportInvestmentInfo");

            migrationBuilder.DropColumn(
                name: "ZoneName",
                table: "ReportInvestmentInfo");
        }
    }
}
