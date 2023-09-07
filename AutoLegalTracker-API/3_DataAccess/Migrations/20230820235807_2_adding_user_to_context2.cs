using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoLegalTracker_API.DataAccess.Migrations
{
    public partial class _2_adding_user_to_context2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailLogs_Emails_EmailTemplateId",
                table: "EmailLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailLogs",
                table: "EmailLogs");

            migrationBuilder.RenameTable(
                name: "EmailLogs",
                newName: "EmailLog");

            migrationBuilder.RenameIndex(
                name: "IX_EmailLogs_EmailTemplateId",
                table: "EmailLog",
                newName: "IX_EmailLog_EmailTemplateId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailLog",
                table: "EmailLog",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailLog_Emails_EmailTemplateId",
                table: "EmailLog",
                column: "EmailTemplateId",
                principalTable: "Emails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailLog_Emails_EmailTemplateId",
                table: "EmailLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailLog",
                table: "EmailLog");

            migrationBuilder.RenameTable(
                name: "EmailLog",
                newName: "EmailLogs");

            migrationBuilder.RenameIndex(
                name: "IX_EmailLog_EmailTemplateId",
                table: "EmailLogs",
                newName: "IX_EmailLogs_EmailTemplateId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailLogs",
                table: "EmailLogs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailLogs_Emails_EmailTemplateId",
                table: "EmailLogs",
                column: "EmailTemplateId",
                principalTable: "Emails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
