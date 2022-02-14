using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Migrate140222_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "ChangeDepot",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Remarks",
                table: "ChangeDepot",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
