using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InithialCreate_18052022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TegCode",
                table: "EmpSbuMapping",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppAuthDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    TotalPerson = table.Column<int>(nullable: false),
                    Expense = table.Column<int>(nullable: false),
                    NewAmount = table.Column<int>(nullable: false),
                    TotalAmount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAuthDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BgtEmployeeLocationWiseSBUExp",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompId = table.Column<int>(nullable: false),
                    SBU = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    SBUAmount = table.Column<long>(nullable: false),
                    DonationId = table.Column<int>(nullable: false),
                    DonationTypeName = table.Column<string>(nullable: true),
                    Expense = table.Column<double>(nullable: false),
                    AuthId = table.Column<int>(nullable: false),
                    ApprovalAuthorityName = table.Column<string>(nullable: true),
                    TotalPerson = table.Column<int>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    Limit = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BgtEmployeeLocationWiseSBUExp", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppAuthDetails");

            migrationBuilder.DropTable(
                name: "BgtEmployeeLocationWiseSBUExp");

            migrationBuilder.DropColumn(
                name: "TegCode",
                table: "EmpSbuMapping");
        }
    }
}
