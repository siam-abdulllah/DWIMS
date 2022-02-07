using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_07022022_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RptInvestmentSummary",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ProposedAmount",
                table: "InvestmentApr",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "RptInvestmentSummary");

            migrationBuilder.AlterColumn<long>(
                name: "ProposedAmount",
                table: "InvestmentApr",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
