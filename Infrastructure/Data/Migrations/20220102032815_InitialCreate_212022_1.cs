using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_212022_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MarketGroupMst_Employee_EmployeeId",
                table: "MarketGroupMst");

            migrationBuilder.DropIndex(
                name: "IX_MarketGroupMst_EmployeeId",
                table: "MarketGroupMst");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "MarketGroupMst");

            migrationBuilder.AddColumn<string>(
                name: "MarketCode",
                table: "MarketGroupMst",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarketCode",
                table: "MarketGroupMst");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "MarketGroupMst",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MarketGroupMst_EmployeeId",
                table: "MarketGroupMst",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MarketGroupMst_Employee_EmployeeId",
                table: "MarketGroupMst",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
