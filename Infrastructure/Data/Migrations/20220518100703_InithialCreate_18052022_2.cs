using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InithialCreate_18052022_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TegCode",
                table: "EmpSbuMapping");

            migrationBuilder.AddColumn<string>(
                name: "TagCode",
                table: "EmpSbuMapping",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TagCode",
                table: "EmpSbuMapping");

            migrationBuilder.AddColumn<string>(
                name: "TegCode",
                table: "EmpSbuMapping",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
