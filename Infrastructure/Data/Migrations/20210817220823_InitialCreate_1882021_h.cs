using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate_1882021_h : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalCeiling_InvestmentType_InvestmentTypeId",
                table: "ApprovalCeiling");

            

            migrationBuilder.DropIndex(
                name: "IX_ApprovalCeiling_InvestmentTypeId",
                table: "ApprovalCeiling");

           

            migrationBuilder.DropColumn(
                name: "InvestmentTypeId",
                table: "ApprovalCeiling");

            migrationBuilder.DropColumn(
                name: "TransacionAmount",
                table: "ApprovalCeiling");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ToDate",
                table: "SBUWiseBudget",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "FromDate",
                table: "SBUWiseBudget",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "InvestmentTo",
                table: "ApprovalCeiling",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "InvestmentFrom",
                table: "ApprovalCeiling",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AmountPerMonth",
                table: "ApprovalCeiling",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AmountPerTransacion",
                table: "ApprovalCeiling",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DonationType",
                table: "ApprovalCeiling",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalTimeLimit_ApprovalAuthorityId",
                table: "ApprovalTimeLimit",
                column: "ApprovalAuthorityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalTimeLimit_ApprovalAuthority_ApprovalAuthorityId",
                table: "ApprovalTimeLimit",
                column: "ApprovalAuthorityId",
                principalTable: "ApprovalAuthority",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalTimeLimit_ApprovalAuthority_ApprovalAuthorityId",
                table: "ApprovalTimeLimit");

            migrationBuilder.DropIndex(
                name: "IX_ApprovalTimeLimit_ApprovalAuthorityId",
                table: "ApprovalTimeLimit");

            migrationBuilder.DropColumn(
                name: "SBU",
                table: "SBUWiseBudget");

            migrationBuilder.DropColumn(
                name: "AmountPerMonth",
                table: "ApprovalCeiling");

            migrationBuilder.DropColumn(
                name: "AmountPerTransacion",
                table: "ApprovalCeiling");

            migrationBuilder.DropColumn(
                name: "DonationType",
                table: "ApprovalCeiling");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ToDate",
                table: "SBUWiseBudget",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FromDate",
                table: "SBUWiseBudget",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SBUId",
                table: "SBUWiseBudget",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InvestmentTo",
                table: "ApprovalCeiling",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InvestmentFrom",
                table: "ApprovalCeiling",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InvestmentTypeId",
                table: "ApprovalCeiling",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransacionAmount",
                table: "ApprovalCeiling",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SBUWiseBudget_SBUId",
                table: "SBUWiseBudget",
                column: "SBUId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalCeiling_InvestmentTypeId",
                table: "ApprovalCeiling",
                column: "InvestmentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalCeiling_InvestmentType_InvestmentTypeId",
                table: "ApprovalCeiling",
                column: "InvestmentTypeId",
                principalTable: "InvestmentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SBUWiseBudget_SBU_SBUId",
                table: "SBUWiseBudget",
                column: "SBUId",
                principalTable: "SBU",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
