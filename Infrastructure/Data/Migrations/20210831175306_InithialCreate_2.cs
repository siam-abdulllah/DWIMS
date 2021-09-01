using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InithialCreate_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "JoiningDate",
                table: "Employee",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Employee",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Employee");

            migrationBuilder.AlterColumn<DateTime>(
                name: "JoiningDate",
                table: "Employee",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
