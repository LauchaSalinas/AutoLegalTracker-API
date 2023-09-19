using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoLegalTracker_API._3_DataAccess.Migrations
{
    public partial class _12_Database_models : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Emails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    emailCode = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LegalCaseConditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpensesShouldBePaid = table.Column<bool>(type: "bit", nullable: false),
                    JurisdictionContains = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JurisdictionDoesNotContain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseCaptionContains = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseCaptionDoesNotContain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnalysesMustBeReceived = table.Column<bool>(type: "bit", nullable: false),
                    RequestedCourtOrdersMustBeReceived = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalCaseConditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LegalResponseTemplate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalResponseTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicalAppointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientAttended = table.Column<bool>(type: "bit", nullable: true),
                    PatientAttendedOnline = table.Column<bool>(type: "bit", nullable: true),
                    LegalNotificationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalAppointments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationConditions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleContains = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleDoesNotContain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BodyContains = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BodyDoesNotContain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromContains = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromDoesNotContain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToContains = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToDoesNotContain = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationConditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeatherForecasts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TemperatureC = table.Column<int>(type: "int", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherForecasts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserTo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmailTemplateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailLogs_Emails_EmailTemplateId",
                        column: x => x.EmailTemplateId,
                        principalTable: "Emails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LegalCaseAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AttributeToAddWhenExpiredId = table.Column<int>(type: "int", nullable: true),
                    LegalCaseConditionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalCaseAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegalCaseAttributes_LegalCaseAttributes_AttributeToAddWhenExpiredId",
                        column: x => x.AttributeToAddWhenExpiredId,
                        principalTable: "LegalCaseAttributes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LegalCaseAttributes_LegalCaseConditions_LegalCaseConditionId",
                        column: x => x.LegalCaseConditionId,
                        principalTable: "LegalCaseConditions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LegalAutomations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RunsAutomatically = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CaseCaptionContains = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseCaptionDoesNotContain = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotificationBodyContains = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotificationBodyDoesNotContain = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotificationFromContains = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotificationFromDoesNotContain = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotificationToContains = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotificationToDoesNotContain = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JurisdictionContains = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JurisdictionDoesNotContain = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpenseAdvancesPaid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalAutomations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LegalCaseActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LegalCaseConditionId = table.Column<int>(type: "int", nullable: true),
                    NotificationConditionId = table.Column<int>(type: "int", nullable: true),
                    LegalResponseTemplateId = table.Column<int>(type: "int", nullable: true),
                    EmailTemplateId = table.Column<int>(type: "int", nullable: true),
                    AssignMedicalAppointment = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalCaseActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegalCaseActions_Emails_EmailTemplateId",
                        column: x => x.EmailTemplateId,
                        principalTable: "Emails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LegalCaseActions_LegalCaseConditions_LegalCaseConditionId",
                        column: x => x.LegalCaseConditionId,
                        principalTable: "LegalCaseConditions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LegalCaseActions_LegalResponseTemplate_LegalResponseTemplateId",
                        column: x => x.LegalResponseTemplateId,
                        principalTable: "LegalResponseTemplate",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LegalCaseActions_NotificationConditions_NotificationConditionId",
                        column: x => x.NotificationConditionId,
                        principalTable: "NotificationConditions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LegalCaseAttributesToAdd",
                columns: table => new
                {
                    LegalCaseActionsWhereItsAddedId = table.Column<int>(type: "int", nullable: false),
                    LegalCaseAttributesToAddId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalCaseAttributesToAdd", x => new { x.LegalCaseActionsWhereItsAddedId, x.LegalCaseAttributesToAddId });
                    table.ForeignKey(
                        name: "FK_LegalCaseAttributesToAdd_LegalCaseActions_LegalCaseActionsWhereItsAddedId",
                        column: x => x.LegalCaseActionsWhereItsAddedId,
                        principalTable: "LegalCaseActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LegalCaseAttributesToAdd_LegalCaseAttributes_LegalCaseAttributesToAddId",
                        column: x => x.LegalCaseAttributesToAddId,
                        principalTable: "LegalCaseAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LegalCaseAttributesToDelete",
                columns: table => new
                {
                    LegalCaseActionsWhereItsDeletedId = table.Column<int>(type: "int", nullable: false),
                    LegalCaseAttributesToDeleteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalCaseAttributesToDelete", x => new { x.LegalCaseActionsWhereItsDeletedId, x.LegalCaseAttributesToDeleteId });
                    table.ForeignKey(
                        name: "FK_LegalCaseAttributesToDelete_LegalCaseActions_LegalCaseActionsWhereItsDeletedId",
                        column: x => x.LegalCaseActionsWhereItsDeletedId,
                        principalTable: "LegalCaseActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LegalCaseAttributesToDelete_LegalCaseAttributes_LegalCaseAttributesToDeleteId",
                        column: x => x.LegalCaseAttributesToDeleteId,
                        principalTable: "LegalCaseAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LegalCaseActionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTypes_LegalCaseActions_LegalCaseActionId",
                        column: x => x.LegalCaseActionId,
                        principalTable: "LegalCaseActions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    Sub = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FamilyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoogleProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebCredentialUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebCredentialPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoogleOAuth2RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoogleOAuth2AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoogleOAuth2TokenExpiration = table.Column<long>(type: "bigint", nullable: false),
                    GoogleOAuth2TokenCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GoogleOAuth2IdToken = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_UserTypes_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "UserTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LegalCases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Jurisdiction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PossibleCourtDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CaseNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpenseAdvances = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpenseAdvancesPaid = table.Column<bool>(type: "bit", nullable: false),
                    LegalCaseHonorarium = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalCases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegalCases_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LegalCaseLegalCaseAttribute",
                columns: table => new
                {
                    LegalCaseAttributesId = table.Column<int>(type: "int", nullable: false),
                    LegalCaseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalCaseLegalCaseAttribute", x => new { x.LegalCaseAttributesId, x.LegalCaseId });
                    table.ForeignKey(
                        name: "FK_LegalCaseLegalCaseAttribute_LegalCaseAttributes_LegalCaseAttributesId",
                        column: x => x.LegalCaseAttributesId,
                        principalTable: "LegalCaseAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LegalCaseLegalCaseAttribute_LegalCases_LegalCaseId",
                        column: x => x.LegalCaseId,
                        principalTable: "LegalCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LegalNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActionHasBeenTaken = table.Column<bool>(type: "bit", nullable: false),
                    LegalCaseId = table.Column<int>(type: "int", nullable: false),
                    Read = table.Column<bool>(type: "bit", nullable: false),
                    MedicalAppointmentId = table.Column<int>(type: "int", nullable: true),
                    To = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    From = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LegalAutomationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegalNotifications_LegalAutomations_LegalAutomationId",
                        column: x => x.LegalAutomationId,
                        principalTable: "LegalAutomations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LegalNotifications_LegalCases_LegalCaseId",
                        column: x => x.LegalCaseId,
                        principalTable: "LegalCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LegalNotifications_MedicalAppointments_MedicalAppointmentId",
                        column: x => x.MedicalAppointmentId,
                        principalTable: "MedicalAppointments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RequestedAnalyses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LegalCaseId = table.Column<int>(type: "int", nullable: false),
                    IsFulfilled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestedAnalyses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestedAnalyses_LegalCases_LegalCaseId",
                        column: x => x.LegalCaseId,
                        principalTable: "LegalCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestedCourtOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LegalCaseId = table.Column<int>(type: "int", nullable: false),
                    IsFulfilled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestedCourtOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestedCourtOrders_LegalCases_LegalCaseId",
                        column: x => x.LegalCaseId,
                        principalTable: "LegalCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailLogs_EmailTemplateId",
                table: "EmailLogs",
                column: "EmailTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalAutomations_UserId",
                table: "LegalAutomations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalCaseActions_EmailTemplateId",
                table: "LegalCaseActions",
                column: "EmailTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalCaseActions_LegalCaseConditionId",
                table: "LegalCaseActions",
                column: "LegalCaseConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalCaseActions_LegalResponseTemplateId",
                table: "LegalCaseActions",
                column: "LegalResponseTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalCaseActions_NotificationConditionId",
                table: "LegalCaseActions",
                column: "NotificationConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalCaseActions_UserId",
                table: "LegalCaseActions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalCaseAttributes_AttributeToAddWhenExpiredId",
                table: "LegalCaseAttributes",
                column: "AttributeToAddWhenExpiredId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalCaseAttributes_LegalCaseConditionId",
                table: "LegalCaseAttributes",
                column: "LegalCaseConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalCaseAttributesToAdd_LegalCaseAttributesToAddId",
                table: "LegalCaseAttributesToAdd",
                column: "LegalCaseAttributesToAddId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalCaseAttributesToDelete_LegalCaseAttributesToDeleteId",
                table: "LegalCaseAttributesToDelete",
                column: "LegalCaseAttributesToDeleteId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalCaseLegalCaseAttribute_LegalCaseId",
                table: "LegalCaseLegalCaseAttribute",
                column: "LegalCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalCases_UserId",
                table: "LegalCases",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalNotifications_LegalAutomationId",
                table: "LegalNotifications",
                column: "LegalAutomationId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalNotifications_LegalCaseId",
                table: "LegalNotifications",
                column: "LegalCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalNotifications_MedicalAppointmentId",
                table: "LegalNotifications",
                column: "MedicalAppointmentId",
                unique: true,
                filter: "[MedicalAppointmentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RequestedAnalyses_LegalCaseId",
                table: "RequestedAnalyses",
                column: "LegalCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestedCourtOrders_LegalCaseId",
                table: "RequestedCourtOrders",
                column: "LegalCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserTypeId",
                table: "Users",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypes_LegalCaseActionId",
                table: "UserTypes",
                column: "LegalCaseActionId");

            migrationBuilder.AddForeignKey(
                name: "FK_LegalAutomations_Users_UserId",
                table: "LegalAutomations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LegalCaseActions_Users_UserId",
                table: "LegalCaseActions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LegalCaseActions_Emails_EmailTemplateId",
                table: "LegalCaseActions");

            migrationBuilder.DropForeignKey(
                name: "FK_LegalCaseActions_Users_UserId",
                table: "LegalCaseActions");

            migrationBuilder.DropTable(
                name: "EmailLogs");

            migrationBuilder.DropTable(
                name: "LegalCaseAttributesToAdd");

            migrationBuilder.DropTable(
                name: "LegalCaseAttributesToDelete");

            migrationBuilder.DropTable(
                name: "LegalCaseLegalCaseAttribute");

            migrationBuilder.DropTable(
                name: "LegalNotifications");

            migrationBuilder.DropTable(
                name: "RequestedAnalyses");

            migrationBuilder.DropTable(
                name: "RequestedCourtOrders");

            migrationBuilder.DropTable(
                name: "WeatherForecasts");

            migrationBuilder.DropTable(
                name: "LegalCaseAttributes");

            migrationBuilder.DropTable(
                name: "LegalAutomations");

            migrationBuilder.DropTable(
                name: "MedicalAppointments");

            migrationBuilder.DropTable(
                name: "LegalCases");

            migrationBuilder.DropTable(
                name: "Emails");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserTypes");

            migrationBuilder.DropTable(
                name: "LegalCaseActions");

            migrationBuilder.DropTable(
                name: "LegalCaseConditions");

            migrationBuilder.DropTable(
                name: "LegalResponseTemplate");

            migrationBuilder.DropTable(
                name: "NotificationConditions");
        }
    }
}
