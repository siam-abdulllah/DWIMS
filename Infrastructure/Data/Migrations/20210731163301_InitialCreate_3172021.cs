using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_3172021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Degree",
                table: "InstitutionInfo");

            migrationBuilder.DropColumn(
                name: "Designation",
                table: "InstitutionInfo");

            migrationBuilder.AddColumn<string>(
                name: "InstitutionType",
                table: "InstitutionInfo",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstitutionType",
                table: "InstitutionInfo");

            migrationBuilder.AddColumn<string>(
                name: "Degree",
                table: "InstitutionInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Designation",
                table: "InstitutionInfo",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
