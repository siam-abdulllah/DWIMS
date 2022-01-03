using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_312022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "CampaignMst",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CountInt",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountInt", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CampaignMst_EmployeeId",
                table: "CampaignMst",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignMst_Employee_EmployeeId",
                table: "CampaignMst",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampaignMst_Employee_EmployeeId",
                table: "CampaignMst");

            migrationBuilder.DropTable(
                name: "CountInt");

            migrationBuilder.DropIndex(
                name: "IX_CampaignMst_EmployeeId",
                table: "CampaignMst");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "CampaignMst");
        }
    }
}
