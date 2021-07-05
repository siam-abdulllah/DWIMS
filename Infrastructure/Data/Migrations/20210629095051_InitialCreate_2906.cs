using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_2906 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    EmployeeSAPCode = table.Column<string>(nullable: true),
                    EmployeeName = table.Column<string>(nullable: true),
                    DepartmentId = table.Column<int>(nullable: false),
                    DesignationId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    JoiningDate = table.Column<DateTime>(nullable: false),
                    JoiningPlace = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PostingType = table.Column<string>(nullable: true),
                    MarketId = table.Column<string>(nullable: true),
                    MarketName = table.Column<string>(nullable: true),
                    RegionId = table.Column<string>(nullable: true),
                    RegionName = table.Column<string>(nullable: true),
                    ZoneId = table.Column<string>(nullable: true),
                    ZoneName = table.Column<string>(nullable: true),
                    TerritoryId = table.Column<string>(nullable: true),
                    TerritoryName = table.Column<string>(nullable: true),
                    DivisionId = table.Column<string>(nullable: true),
                    DivisionName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApprAuthConfig",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: false),
                    ApprovalAuthorityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprAuthConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprAuthConfig_ApprovalAuthority_ApprovalAuthorityId",
                        column: x => x.ApprovalAuthorityId,
                        principalTable: "ApprovalAuthority",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApprAuthConfig_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApprAuthConfig_ApprovalAuthorityId",
                table: "ApprAuthConfig",
                column: "ApprovalAuthorityId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprAuthConfig_EmployeeId",
                table: "ApprAuthConfig",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprAuthConfig");

            migrationBuilder.DropTable(
                name: "Employee");
        }
    }
}
