using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoLegalTracker_API.DataAccess.Migrations
{
    public partial class _2_adding_user_to_context3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sub = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FamilyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoogleProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebCredentialUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebCredentialPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoogleOAuth2RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoogleOAuth2AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoogleOAuth2TokenExpiration = table.Column<long>(type: "bigint", nullable: true),
                    GoogleOAuth2TokenCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GoogleOAuth2IdToken = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

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

            migrationBuilder.DropTable(
                name: "User");

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
    }
}
