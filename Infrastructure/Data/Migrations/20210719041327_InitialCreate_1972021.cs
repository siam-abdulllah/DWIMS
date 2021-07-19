using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_1972021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BrandCode",
                table: "ProductInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BrandName",
                table: "ProductInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SBU",
                table: "ProductInfo",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrandCode",
                table: "ProductInfo");

            migrationBuilder.DropColumn(
                name: "BrandName",
                table: "ProductInfo");

            migrationBuilder.DropColumn(
                name: "SBU",
                table: "ProductInfo");
        }
    }
}
