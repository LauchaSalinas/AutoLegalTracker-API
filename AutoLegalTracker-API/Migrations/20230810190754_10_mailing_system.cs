using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoLegalTracker_API.Migrations
{
    public partial class _10_mailing_system : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserTo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailLogs_Emails_EmailId",
                        column: x => x.EmailId,
                        principalTable: "Emails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailLogs_EmailId",
                table: "EmailLogs",
                column: "EmailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailLogs");
        }
    }
}
