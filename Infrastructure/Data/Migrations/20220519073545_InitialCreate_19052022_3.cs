using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_19052022_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BgtEmployeeDetail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    CompId = table.Column<int>(nullable: false),
                    DeptId = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    SBU = table.Column<string>(nullable: true),
                    AuthId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    CompoCode = table.Column<string>(nullable: true),
                    Amount = table.Column<long>(nullable: false),
                    PermEdit = table.Column<bool>(nullable: false),
                    PermView = table.Column<bool>(nullable: false),
                    PermAmt = table.Column<bool>(nullable: false),
                    PermDonation = table.Column<bool>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    EnteredBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BgtEmployeeDetail", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BgtEmployeeDetail");
        }
    }
}
