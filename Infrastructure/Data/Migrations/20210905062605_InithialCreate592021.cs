using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InithialCreate592021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "CompletionStatus",
                table: "InvestmentTargetedGroup",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "InvestmentApr",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SBUName",
                table: "Employee",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentApr_EmployeeId",
                table: "InvestmentApr",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentApr_Employee_EmployeeId",
                table: "InvestmentApr",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentApr_Employee_EmployeeId",
                table: "InvestmentApr");

            migrationBuilder.DropIndex(
                name: "IX_InvestmentApr_EmployeeId",
                table: "InvestmentApr");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "InvestmentApr");

            migrationBuilder.DropColumn(
                name: "SBUName",
                table: "Employee");

            migrationBuilder.AlterColumn<bool>(
                name: "CompletionStatus",
                table: "InvestmentTargetedGroup",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}
