using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InithialCreate_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InstitutionMarket",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    InstitutionId = table.Column<int>(nullable: false),
                    MarketCode = table.Column<string>(nullable: true),
                    MarketName = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    SBU = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstitutionMarket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstitutionMarket_InstitutionInfo_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "InstitutionInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstitutionMarket_InstitutionId",
                table: "InstitutionMarket",
                column: "InstitutionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstitutionMarket");
        }
    }
}
