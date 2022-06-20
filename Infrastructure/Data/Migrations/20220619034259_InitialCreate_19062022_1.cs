using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_19062022_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalAuthorityId",
                table: "RptEmpWiseExp");

            migrationBuilder.DropColumn(
                name: "Expense",
                table: "RptEmpWiseExp");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "RptEmpWiseExp");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "RptEmpWiseExp");

            migrationBuilder.AlterColumn<string>(
                name: "Budget",
                table: "RptEmpWiseExp",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "April",
                table: "RptEmpWiseExp",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "August",
                table: "RptEmpWiseExp",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuthId",
                table: "RptEmpWiseExp",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "RptEmpWiseExp",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompId",
                table: "RptEmpWiseExp",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CompoCode",
                table: "RptEmpWiseExp",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "December",
                table: "RptEmpWiseExp",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeptId",
                table: "RptEmpWiseExp",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "February",
                table: "RptEmpWiseExp",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "January",
                table: "RptEmpWiseExp",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "July",
                table: "RptEmpWiseExp",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "June",
                table: "RptEmpWiseExp",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "March",
                table: "RptEmpWiseExp",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "May",
                table: "RptEmpWiseExp",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "November",
                table: "RptEmpWiseExp",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "October",
                table: "RptEmpWiseExp",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SBU",
                table: "RptEmpWiseExp",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Segment",
                table: "RptEmpWiseExp",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "September",
                table: "RptEmpWiseExp",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TotalAmount",
                table: "RptEmpWiseExp",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeptId",
                table: "Donation",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "April",
                table: "RptEmpWiseExp");

            migrationBuilder.DropColumn(
                name: "August",
                table: "RptEmpWiseExp");

            migrationBuilder.DropColumn(
                name: "AuthId",
                table: "RptEmpWiseExp");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "RptEmpWiseExp");

            migrationBuilder.DropColumn(
                name: "CompId",
                table: "RptEmpWiseExp");

            migrationBuilder.DropColumn(
                name: "CompoCode",
                table: "RptEmpWiseExp");

            migrationBuilder.DropColumn(
                name: "December",
                table: "RptEmpWiseExp");

            migrationBuilder.DropColumn(
                name: "DeptId",
                table: "RptEmpWiseExp");

            migrationBuilder.DropColumn(
                name: "February",
                table: "RptEmpWiseExp");

            migrationBuilder.DropColumn(
                name: "January",
                table: "RptEmpWiseExp");

            migrationBuilder.DropColumn(
                name: "July",
                table: "RptEmpWiseExp");

            migrationBuilder.DropColumn(
                name: "June",
                table: "RptEmpWiseExp");

            migrationBuilder.DropColumn(
                name: "March",
                table: "RptEmpWiseExp");

            migrationBuilder.DropColumn(
                name: "May",
                table: "RptEmpWiseExp");

            migrationBuilder.DropColumn(
                name: "November",
                table: "RptEmpWiseExp");

            migrationBuilder.DropColumn(
                name: "October",
                table: "RptEmpWiseExp");

            migrationBuilder.DropColumn(
                name: "SBU",
                table: "RptEmpWiseExp");

            migrationBuilder.DropColumn(
                name: "Segment",
                table: "RptEmpWiseExp");

            migrationBuilder.DropColumn(
                name: "September",
                table: "RptEmpWiseExp");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "RptEmpWiseExp");

            migrationBuilder.DropColumn(
                name: "DeptId",
                table: "Donation");

            migrationBuilder.AlterColumn<double>(
                name: "Budget",
                table: "RptEmpWiseExp",
                type: "float",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApprovalAuthorityId",
                table: "RptEmpWiseExp",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Expense",
                table: "RptEmpWiseExp",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Month",
                table: "RptEmpWiseExp",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "RptEmpWiseExp",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
