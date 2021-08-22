using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_1982021_O_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DoctorHonAppr",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InvestmentInitId = table.Column<int>(nullable: false),
                    DoctorId = table.Column<int>(nullable: false),
                    HonAmount = table.Column<long>(nullable: false),
                    HonMonth = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorHonAppr", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorHonAppr_DoctorInfo_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "DoctorInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorHonAppr_InvestmentInit_InvestmentInitId",
                        column: x => x.InvestmentInitId,
                        principalTable: "InvestmentInit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorHonAppr_DoctorId",
                table: "DoctorHonAppr",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorHonAppr_InvestmentInitId",
                table: "DoctorHonAppr",
                column: "InvestmentInitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorHonAppr");
        }
    }
}
