using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_21052022_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "AppAuthDetails");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "AppAuthDetails");

            migrationBuilder.DropColumn(
                name: "TotalPerson",
                table: "AppAuthDetails");

            migrationBuilder.AddColumn<int>(
                name: "CompId",
                table: "ApprovalAuthority",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeptId",
                table: "ApprovalAuthority",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "AppAuthDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AuthId",
                table: "AppAuthDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Authority",
                table: "AppAuthDetails",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoOfEmployee",
                table: "AppAuthDetails",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompId",
                table: "ApprovalAuthority");

            migrationBuilder.DropColumn(
                name: "DeptId",
                table: "ApprovalAuthority");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "AppAuthDetails");

            migrationBuilder.DropColumn(
                name: "AuthId",
                table: "AppAuthDetails");

            migrationBuilder.DropColumn(
                name: "Authority",
                table: "AppAuthDetails");

            migrationBuilder.DropColumn(
                name: "NoOfEmployee",
                table: "AppAuthDetails");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "AppAuthDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalAmount",
                table: "AppAuthDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalPerson",
                table: "AppAuthDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
