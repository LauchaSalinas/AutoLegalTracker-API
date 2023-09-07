using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoLegalTracker_API.DataAccess.Migrations
{
    public partial class _2_adding_legal_cases_models3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalAppointments_LegalNotifications_LegalNotificationId",
                table: "MedicalAppointments");

            migrationBuilder.DropColumn(
                name: "LegalNotification",
                table: "MedicalAppointments");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "LegalCases",
                newName: "UserId");

            migrationBuilder.AlterColumn<int>(
                name: "LegalNotificationId",
                table: "MedicalAppointments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LegalNotifications_LegalAutomationId",
                table: "LegalNotifications",
                column: "LegalAutomationId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalCases_UserId",
                table: "LegalCases",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LegalCases_Users_UserId",
                table: "LegalCases",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LegalNotifications_LegalAutomations_LegalAutomationId",
                table: "LegalNotifications",
                column: "LegalAutomationId",
                principalTable: "LegalAutomations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalAppointments_LegalNotifications_LegalNotificationId",
                table: "MedicalAppointments",
                column: "LegalNotificationId",
                principalTable: "LegalNotifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LegalCases_Users_UserId",
                table: "LegalCases");

            migrationBuilder.DropForeignKey(
                name: "FK_LegalNotifications_LegalAutomations_LegalAutomationId",
                table: "LegalNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalAppointments_LegalNotifications_LegalNotificationId",
                table: "MedicalAppointments");

            migrationBuilder.DropIndex(
                name: "IX_LegalNotifications_LegalAutomationId",
                table: "LegalNotifications");

            migrationBuilder.DropIndex(
                name: "IX_LegalCases_UserId",
                table: "LegalCases");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "LegalCases",
                newName: "userId");

            migrationBuilder.AlterColumn<int>(
                name: "LegalNotificationId",
                table: "MedicalAppointments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "LegalNotification",
                table: "MedicalAppointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalAppointments_LegalNotifications_LegalNotificationId",
                table: "MedicalAppointments",
                column: "LegalNotificationId",
                principalTable: "LegalNotifications",
                principalColumn: "Id");
        }
    }
}
