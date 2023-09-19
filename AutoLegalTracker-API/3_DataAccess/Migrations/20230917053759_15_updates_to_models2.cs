using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoLegalTracker_API._3_DataAccess.Migrations
{
    public partial class _15_updates_to_models2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LegalCaseActions_NotificationCondition_NotificationConditionId",
                table: "LegalCaseActions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationCondition",
                table: "NotificationCondition");

            migrationBuilder.RenameTable(
                name: "NotificationCondition",
                newName: "NotificationConditions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationConditions",
                table: "NotificationConditions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LegalCaseActions_NotificationConditions_NotificationConditionId",
                table: "LegalCaseActions",
                column: "NotificationConditionId",
                principalTable: "NotificationConditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LegalCaseActions_NotificationConditions_NotificationConditionId",
                table: "LegalCaseActions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationConditions",
                table: "NotificationConditions");

            migrationBuilder.RenameTable(
                name: "NotificationConditions",
                newName: "NotificationCondition");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationCondition",
                table: "NotificationCondition",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LegalCaseActions_NotificationCondition_NotificationConditionId",
                table: "LegalCaseActions",
                column: "NotificationConditionId",
                principalTable: "NotificationCondition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
