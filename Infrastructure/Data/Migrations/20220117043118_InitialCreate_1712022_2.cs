using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_1712022_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicineProduct",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    ProductCode = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    PackSize = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    UnitTp = table.Column<double>(nullable: false),
                    UnitVat = table.Column<double>(nullable: false),
                    SorgaCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineProduct", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentMedicineProd",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InvestmentInitId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: true),
                    TpVat = table.Column<double>(nullable: false),
                    BoxQuantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentMedicineProd", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentMedicineProd_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvestmentMedicineProd_InvestmentInit_InvestmentInitId",
                        column: x => x.InvestmentInitId,
                        principalTable: "InvestmentInit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentMedicineProd_MedicineProduct_ProductId",
                        column: x => x.ProductId,
                        principalTable: "MedicineProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentMedicineProd_EmployeeId",
                table: "InvestmentMedicineProd",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentMedicineProd_InvestmentInitId",
                table: "InvestmentMedicineProd",
                column: "InvestmentInitId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentMedicineProd_ProductId",
                table: "InvestmentMedicineProd",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvestmentMedicineProd");

            migrationBuilder.DropTable(
                name: "MedicineProduct");
        }
    }
}
