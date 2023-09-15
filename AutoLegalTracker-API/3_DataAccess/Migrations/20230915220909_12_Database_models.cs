using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoLegalTracker_API._3_DataAccess.Migrations
{
    public partial class _12_Database_models : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MedicalAppointments_LegalNotificationId",
                table: "MedicalAppointments");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "LegalCases",
                newName: "Caption");

            migrationBuilder.AlterColumn<long>(
                name: "GoogleOAuth2TokenExpiration",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PatientAttendedOnline",
                table: "MedicalAppointments",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MedicalAppointmentId",
                table: "LegalNotifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ExpenseAdvancesPaid",
                table: "LegalCases",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PossibleCourtDate",
                table: "LegalCases",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CaseCaptionContains",
                table: "LegalAutomations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CaseCaptionDoesNotContain",
                table: "LegalAutomations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "ExpenseAdvancesPaid",
                table: "LegalAutomations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "JurisdictionContains",
                table: "LegalAutomations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "JurisdictionDoesNotContain",
                table: "LegalAutomations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NotificationBodyContains",
                table: "LegalAutomations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NotificationBodyDoesNotContain",
                table: "LegalAutomations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NotificationFromContains",
                table: "LegalAutomations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NotificationFromDoesNotContain",
                table: "LegalAutomations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NotificationToContains",
                table: "LegalAutomations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NotificationToDoesNotContain",
                table: "LegalAutomations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "RunsAutomatically",
                table: "LegalAutomations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "LegalAutomations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CourtOrderRequested",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestedCourtOrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourtOrderRequested", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourtOrderRequested_LegalCases_RequestedCourtOrderId",
                        column: x => x.RequestedCourtOrderId,
                        principalTable: "LegalCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourtOrderSent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentCourtOrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourtOrderSent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourtOrderSent_LegalCases_SentCourtOrderId",
                        column: x => x.SentCourtOrderId,
                        principalTable: "LegalCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LegalCaseAttributeCondition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LegalCaseAttributeConditionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalCaseAttributeCondition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegalCaseAttributeCondition_LegalCaseAttributeCondition_LegalCaseAttributeConditionId",
                        column: x => x.LegalCaseAttributeConditionId,
                        principalTable: "LegalCaseAttributeCondition",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LegalCaseProperty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PropertyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropertyValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropertyValueType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalCaseProperty", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestedAnalysis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LegalCaseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestedAnalysis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestedAnalysis_LegalCases_LegalCaseId",
                        column: x => x.LegalCaseId,
                        principalTable: "LegalCases",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LegalCaseAttribute",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LegalCaseAttributeConditionId = table.Column<int>(type: "int", nullable: false),
                    LegalCaseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalCaseAttribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegalCaseAttribute_LegalCaseAttributeCondition_LegalCaseAttributeConditionId",
                        column: x => x.LegalCaseAttributeConditionId,
                        principalTable: "LegalCaseAttributeCondition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LegalCaseAttribute_LegalCases_LegalCaseId",
                        column: x => x.LegalCaseId,
                        principalTable: "LegalCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LegalCaseAttributeConditionLegalCaseProperty",
                columns: table => new
                {
                    LegalCaseAttributeConditionsId = table.Column<int>(type: "int", nullable: false),
                    LegalCasePropertiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalCaseAttributeConditionLegalCaseProperty", x => new { x.LegalCaseAttributeConditionsId, x.LegalCasePropertiesId });
                    table.ForeignKey(
                        name: "FK_LegalCaseAttributeConditionLegalCaseProperty_LegalCaseAttributeCondition_LegalCaseAttributeConditionsId",
                        column: x => x.LegalCaseAttributeConditionsId,
                        principalTable: "LegalCaseAttributeCondition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LegalCaseAttributeConditionLegalCaseProperty_LegalCaseProperty_LegalCasePropertiesId",
                        column: x => x.LegalCasePropertiesId,
                        principalTable: "LegalCaseProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalAppointments_LegalNotificationId",
                table: "MedicalAppointments",
                column: "LegalNotificationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LegalAutomations_UserId",
                table: "LegalAutomations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourtOrderRequested_RequestedCourtOrderId",
                table: "CourtOrderRequested",
                column: "RequestedCourtOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CourtOrderSent_SentCourtOrderId",
                table: "CourtOrderSent",
                column: "SentCourtOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalCaseAttribute_LegalCaseAttributeConditionId",
                table: "LegalCaseAttribute",
                column: "LegalCaseAttributeConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalCaseAttribute_LegalCaseId",
                table: "LegalCaseAttribute",
                column: "LegalCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalCaseAttributeCondition_LegalCaseAttributeConditionId",
                table: "LegalCaseAttributeCondition",
                column: "LegalCaseAttributeConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalCaseAttributeConditionLegalCaseProperty_LegalCasePropertiesId",
                table: "LegalCaseAttributeConditionLegalCaseProperty",
                column: "LegalCasePropertiesId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestedAnalysis_LegalCaseId",
                table: "RequestedAnalysis",
                column: "LegalCaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_LegalAutomations_Users_UserId",
                table: "LegalAutomations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LegalAutomations_Users_UserId",
                table: "LegalAutomations");

            migrationBuilder.DropTable(
                name: "CourtOrderRequested");

            migrationBuilder.DropTable(
                name: "CourtOrderSent");

            migrationBuilder.DropTable(
                name: "LegalCaseAttribute");

            migrationBuilder.DropTable(
                name: "LegalCaseAttributeConditionLegalCaseProperty");

            migrationBuilder.DropTable(
                name: "RequestedAnalysis");

            migrationBuilder.DropTable(
                name: "LegalCaseAttributeCondition");

            migrationBuilder.DropTable(
                name: "LegalCaseProperty");

            migrationBuilder.DropIndex(
                name: "IX_MedicalAppointments_LegalNotificationId",
                table: "MedicalAppointments");

            migrationBuilder.DropIndex(
                name: "IX_LegalAutomations_UserId",
                table: "LegalAutomations");

            migrationBuilder.DropColumn(
                name: "PatientAttendedOnline",
                table: "MedicalAppointments");

            migrationBuilder.DropColumn(
                name: "MedicalAppointmentId",
                table: "LegalNotifications");

            migrationBuilder.DropColumn(
                name: "ExpenseAdvancesPaid",
                table: "LegalCases");

            migrationBuilder.DropColumn(
                name: "PossibleCourtDate",
                table: "LegalCases");

            migrationBuilder.DropColumn(
                name: "CaseCaptionContains",
                table: "LegalAutomations");

            migrationBuilder.DropColumn(
                name: "CaseCaptionDoesNotContain",
                table: "LegalAutomations");

            migrationBuilder.DropColumn(
                name: "ExpenseAdvancesPaid",
                table: "LegalAutomations");

            migrationBuilder.DropColumn(
                name: "JurisdictionContains",
                table: "LegalAutomations");

            migrationBuilder.DropColumn(
                name: "JurisdictionDoesNotContain",
                table: "LegalAutomations");

            migrationBuilder.DropColumn(
                name: "NotificationBodyContains",
                table: "LegalAutomations");

            migrationBuilder.DropColumn(
                name: "NotificationBodyDoesNotContain",
                table: "LegalAutomations");

            migrationBuilder.DropColumn(
                name: "NotificationFromContains",
                table: "LegalAutomations");

            migrationBuilder.DropColumn(
                name: "NotificationFromDoesNotContain",
                table: "LegalAutomations");

            migrationBuilder.DropColumn(
                name: "NotificationToContains",
                table: "LegalAutomations");

            migrationBuilder.DropColumn(
                name: "NotificationToDoesNotContain",
                table: "LegalAutomations");

            migrationBuilder.DropColumn(
                name: "RunsAutomatically",
                table: "LegalAutomations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "LegalAutomations");

            migrationBuilder.RenameColumn(
                name: "Caption",
                table: "LegalCases",
                newName: "Name");

            migrationBuilder.AlterColumn<long>(
                name: "GoogleOAuth2TokenExpiration",
                table: "Users",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalAppointments_LegalNotificationId",
                table: "MedicalAppointments",
                column: "LegalNotificationId");
        }
    }
}
