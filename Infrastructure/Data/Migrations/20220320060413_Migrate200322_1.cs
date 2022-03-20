using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class Migrate200322_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DepotCode",
                table: "RptMedDisp",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ProposedAmount",
                table: "InvestmentRcvPending",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "InvestmentInitId",
                table: "ChangeDepotSearch",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AuditTrail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    ActionName = table.Column<string>(nullable: true),
                    ActionType = table.Column<string>(nullable: true),
                    ActionDate = table.Column<DateTime>(nullable: false),
                    ActionTable = table.Column<string>(nullable: true),
                    FormName = table.Column<string>(nullable: true),
                    ActionBy = table.Column<string>(nullable: true),
                    HostAddress = table.Column<string>(nullable: true),
                    TransID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTrail", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditTrail");

            migrationBuilder.DropColumn(
                name: "DepotCode",
                table: "RptMedDisp");

            migrationBuilder.DropColumn(
                name: "ProposedAmount",
                table: "InvestmentRcvPending");

            migrationBuilder.DropColumn(
                name: "InvestmentInitId",
                table: "ChangeDepotSearch");
        }
    }
}
