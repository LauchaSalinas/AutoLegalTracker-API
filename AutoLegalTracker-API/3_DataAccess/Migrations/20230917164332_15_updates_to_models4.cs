using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoLegalTracker_API._3_DataAccess.Migrations
{
    public partial class _15_updates_to_models4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttributeToAddWhenExpiredId",
                table: "LegalCaseAttributes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAt",
                table: "LegalCaseAttributes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LegalCaseAttributes_AttributeToAddWhenExpiredId",
                table: "LegalCaseAttributes",
                column: "AttributeToAddWhenExpiredId");

            migrationBuilder.AddForeignKey(
                name: "FK_LegalCaseAttributes_LegalCaseAttributes_AttributeToAddWhenExpiredId",
                table: "LegalCaseAttributes",
                column: "AttributeToAddWhenExpiredId",
                principalTable: "LegalCaseAttributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LegalCaseAttributes_LegalCaseAttributes_AttributeToAddWhenExpiredId",
                table: "LegalCaseAttributes");

            migrationBuilder.DropIndex(
                name: "IX_LegalCaseAttributes_AttributeToAddWhenExpiredId",
                table: "LegalCaseAttributes");

            migrationBuilder.DropColumn(
                name: "AttributeToAddWhenExpiredId",
                table: "LegalCaseAttributes");

            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                table: "LegalCaseAttributes");
        }
    }
}
