using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_23062022_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "InvestmentRapid");

            migrationBuilder.DropColumn(
                name: "SubCampaignId",
                table: "InvestmentRapid");

            migrationBuilder.DropColumn(
                name: "SubCampaignName",
                table: "InvestmentRapid");

            migrationBuilder.CreateTable(
                name: "BgtEmpSbuDisburse",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompId = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    DeptId = table.Column<int>(nullable: false),
                    SBU = table.Column<string>(nullable: true),
                    AuthId = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false),
                    EmployeeName = table.Column<string>(nullable: true),
                    CompoCode = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Total = table.Column<long>(nullable: true),
                    Allocated = table.Column<long>(nullable: true),
                    Expense = table.Column<double>(nullable: true),
                    NewAllocated = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BgtEmpSbuDisburse", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BgtEmpSbuDisburse");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "InvestmentRapid",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubCampaignId",
                table: "InvestmentRapid",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SubCampaignName",
                table: "InvestmentRapid",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
