using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoLegalTracker_API._3_DataAccess.Migrations
{
    public partial class _2_update_to_user_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailLogs_Emails_EmailId",
                table: "EmailLogs");

            migrationBuilder.RenameColumn(
                name: "EmailId",
                table: "EmailLogs",
                newName: "EmailTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_EmailLogs_EmailId",
                table: "EmailLogs",
                newName: "IX_EmailLogs_EmailTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailLogs_Emails_EmailTemplateId",
                table: "EmailLogs",
                column: "EmailTemplateId",
                principalTable: "Emails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailLogs_Emails_EmailTemplateId",
                table: "EmailLogs");

            migrationBuilder.RenameColumn(
                name: "EmailTemplateId",
                table: "EmailLogs",
                newName: "EmailId");

            migrationBuilder.RenameIndex(
                name: "IX_EmailLogs_EmailTemplateId",
                table: "EmailLogs",
                newName: "IX_EmailLogs_EmailId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailLogs_Emails_EmailId",
                table: "EmailLogs",
                column: "EmailId",
                principalTable: "Emails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
