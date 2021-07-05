using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_19042021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrandInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    BrandName = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    ProductCode = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CampaignMst",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    CampaignNo = table.Column<string>(nullable: true),
                    CampaignName = table.Column<string>(nullable: true),
                    SBU = table.Column<string>(nullable: true),
                    BrandId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignMst", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignMst_BrandInfo_BrandId",
                        column: x => x.BrandId,
                        principalTable: "BrandInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampaignDtl",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    MstId = table.Column<int>(nullable: false),
                    SubCampaignId = table.Column<int>(nullable: false),
                    Budget = table.Column<long>(nullable: false),
                    SubCampStartDate = table.Column<DateTime>(nullable: false),
                    SubCampEndDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignDtl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignDtl_CampaignMst_MstId",
                        column: x => x.MstId,
                        principalTable: "CampaignMst",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignDtl_SubCampaign_SubCampaignId",
                        column: x => x.SubCampaignId,
                        principalTable: "SubCampaign",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampaignDtlProduct",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataStatus = table.Column<int>(nullable: false),
                    SetOn = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedOn = table.Column<DateTimeOffset>(nullable: false),
                    DtlId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignDtlProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignDtlProduct_CampaignDtl_DtlId",
                        column: x => x.DtlId,
                        principalTable: "CampaignDtl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignDtlProduct_ProductInfo_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CampaignDtl_MstId",
                table: "CampaignDtl",
                column: "MstId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignDtl_SubCampaignId",
                table: "CampaignDtl",
                column: "SubCampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignDtlProduct_DtlId",
                table: "CampaignDtlProduct",
                column: "DtlId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignDtlProduct_ProductId",
                table: "CampaignDtlProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignMst_BrandId",
                table: "CampaignMst",
                column: "BrandId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampaignDtlProduct");

            migrationBuilder.DropTable(
                name: "CampaignDtl");

            migrationBuilder.DropTable(
                name: "ProductInfo");

            migrationBuilder.DropTable(
                name: "CampaignMst");

            migrationBuilder.DropTable(
                name: "BrandInfo");
        }
    }
}
