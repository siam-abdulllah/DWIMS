using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_992021_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InstitutionId",
                table: "RptDocCampWiseInvestment",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstitutionName",
                table: "RptDocCampWiseInvestment",
                maxLength: 150,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstitutionId",
                table: "RptDocCampWiseInvestment");

            migrationBuilder.DropColumn(
                name: "InstitutionName",
                table: "RptDocCampWiseInvestment");
        }
    }
}
