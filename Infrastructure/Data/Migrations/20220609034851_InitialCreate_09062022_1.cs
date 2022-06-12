using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_09062022_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApprovalAuthorityId",
                table: "EmpSbuMapping",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CampaignExp",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SBU = table.Column<string>(nullable: true),
                    Amount = table.Column<long>(nullable: true),
                    TotalAlloc = table.Column<long>(nullable: true),
                    TotalExp = table.Column<double>(nullable: true),
                    CampExp = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignExp", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampaignExp");

            migrationBuilder.DropColumn(
                name: "ApprovalAuthorityId",
                table: "EmpSbuMapping");
        }
    }
}
