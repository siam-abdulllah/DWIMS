using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Migrate270122 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "FromDate",
                table: "LastFiveInvestmentInfo",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MedicineDispatchDtl",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InvestmentInitId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductName = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: true),
                    TpVat = table.Column<double>(nullable: false),
                    BoxQuantity = table.Column<int>(nullable: false),
                    DispatchQuantity = table.Column<int>(nullable: false),
                    DispatchTpVat = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineDispatchDtl", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicineDispatchDtl");

            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "LastFiveInvestmentInfo");
        }
    }
}
