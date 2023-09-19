using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoLegalTracker_API._3_DataAccess.Migrations
{
    public partial class _15_updates_to_models : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LegalCaseAttribute_LegalCaseAttributeCondition_LegalCaseAttributeConditionId",
                table: "LegalCaseAttribute");

            migrationBuilder.DropForeignKey(
                name: "FK_LegalCaseAttribute_LegalCases_LegalCaseId",
                table: "LegalCaseAttribute");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestedAnalysis_LegalCases_LegalCaseId",
                table: "RequestedAnalysis");

            migrationBuilder.DropTable(
                name: "CourtOrderRequested");

            migrationBuilder.DropTable(
                name: "CourtOrderSent");

            migrationBuilder.DropTable(
                name: "LegalCaseAttributeConditionLegalCaseProperty");

            migrationBuilder.DropTable(
                name: "LegalCaseAttributeCondition");

            migrationBuilder.DropTable(
                name: "LegalCaseProperty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestedAnalysis",
                table: "RequestedAnalysis");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LegalCaseAttribute",
                table: "LegalCaseAttribute");

            migrationBuilder.DropIndex(
                name: "IX_LegalCaseAttribute_LegalCaseAttributeConditionId",
                table: "LegalCaseAttribute");

            migrationBuilder.DropColumn(
                name: "UseAutomation",
                table: "LegalNotifications");

            migrationBuilder.DropColumn(
                name: "LegalCaseAttributeConditionId",
                table: "LegalCaseAttribute");

            migrationBuilder.RenameTable(
                name: "RequestedAnalysis",
                newName: "RequestedAnalyses");

            migrationBuilder.RenameTable(
                name: "LegalCaseAttribute",
                newName: "LegalCaseAttributes");

            migrationBuilder.RenameIndex(
                name: "IX_RequestedAnalysis_LegalCaseId",
                table: "RequestedAnalyses",
                newName: "IX_RequestedAnalyses_LegalCaseId");

            migrationBuilder.RenameIndex(
                name: "IX_LegalCaseAttribute_LegalCaseId",
                table: "LegalCaseAttributes",
                newName: "IX_LegalCaseAttributes_LegalCaseId");

            migrationBuilder.AddColumn<int>(
                name: "UserTypeId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "From",
                table: "LegalNotifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "To",
                table: "LegalNotifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "ExpenseAdvances",
                table: "LegalCases",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LegalCaseHonorarium",
                table: "LegalCases",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "LegalCaseId",
                table: "RequestedAnalyses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFulfilled",
                table: "RequestedAnalyses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestedAnalyses",
                table: "RequestedAnalyses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LegalCaseAttributes",
                table: "LegalCaseAttributes",
                column: "Id");

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
                name: "NotificationCondition",
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
                    table.PrimaryKey("PK_NotificationCondition", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "UserType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LegalCaseAttribute_LegalCaseCondition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LegalCaseAttributeId = table.Column<int>(type: "int", nullable: false),
                    LegalCaseConditionId = table.Column<int>(type: "int", nullable: false),
                    MustBePresent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalCaseAttribute_LegalCaseCondition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegalCaseAttribute_LegalCaseCondition_LegalCaseAttributes_LegalCaseAttributeId",
                        column: x => x.LegalCaseAttributeId,
                        principalTable: "LegalCaseAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LegalCaseAttribute_LegalCaseCondition_LegalCaseConditions_LegalCaseConditionId",
                        column: x => x.LegalCaseConditionId,
                        principalTable: "LegalCaseConditions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    LegalCaseConditionId = table.Column<int>(type: "int", nullable: true),
                    NotificationCondtionId = table.Column<int>(type: "int", nullable: true),
                    NotificationConditionId = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_LegalCaseActions_NotificationCondition_NotificationConditionId",
                        column: x => x.NotificationConditionId,
                        principalTable: "NotificationCondition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LegalCaseAction_LegalCaseAttribute",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LegalCaseActionId = table.Column<int>(type: "int", nullable: false),
                    LegalCaseAttributeId = table.Column<int>(type: "int", nullable: false),
                    Add = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalCaseAction_LegalCaseAttribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegalCaseAction_LegalCaseAttribute_LegalCaseActions_LegalCaseActionId",
                        column: x => x.LegalCaseActionId,
                        principalTable: "LegalCaseActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LegalCaseAction_LegalCaseAttribute_LegalCaseAttributes_LegalCaseAttributeId",
                        column: x => x.LegalCaseAttributeId,
                        principalTable: "LegalCaseAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LegalCaseAction_UserType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LegalCaseActionId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalCaseAction_UserType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LegalCaseAction_UserType_LegalCaseActions_LegalCaseActionId",
                        column: x => x.LegalCaseActionId,
                        principalTable: "LegalCaseActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LegalCaseAction_UserType_UserType_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "UserType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserTypeId",
                table: "Users",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalCaseAction_LegalCaseAttribute_LegalCaseActionId",
                table: "LegalCaseAction_LegalCaseAttribute",
                column: "LegalCaseActionId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalCaseAction_LegalCaseAttribute_LegalCaseAttributeId",
                table: "LegalCaseAction_LegalCaseAttribute",
                column: "LegalCaseAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalCaseAction_UserType_LegalCaseActionId",
                table: "LegalCaseAction_UserType",
                column: "LegalCaseActionId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalCaseAction_UserType_UserTypeId",
                table: "LegalCaseAction_UserType",
                column: "UserTypeId");

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
                name: "IX_LegalCaseAttribute_LegalCaseCondition_LegalCaseAttributeId",
                table: "LegalCaseAttribute_LegalCaseCondition",
                column: "LegalCaseAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalCaseAttribute_LegalCaseCondition_LegalCaseConditionId",
                table: "LegalCaseAttribute_LegalCaseCondition",
                column: "LegalCaseConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestedCourtOrders_LegalCaseId",
                table: "RequestedCourtOrders",
                column: "LegalCaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_LegalCaseAttributes_LegalCases_LegalCaseId",
                table: "LegalCaseAttributes",
                column: "LegalCaseId",
                principalTable: "LegalCases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestedAnalyses_LegalCases_LegalCaseId",
                table: "RequestedAnalyses",
                column: "LegalCaseId",
                principalTable: "LegalCases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserType_UserTypeId",
                table: "Users",
                column: "UserTypeId",
                principalTable: "UserType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LegalCaseAttributes_LegalCases_LegalCaseId",
                table: "LegalCaseAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestedAnalyses_LegalCases_LegalCaseId",
                table: "RequestedAnalyses");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserType_UserTypeId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "LegalCaseAction_LegalCaseAttribute");

            migrationBuilder.DropTable(
                name: "LegalCaseAction_UserType");

            migrationBuilder.DropTable(
                name: "LegalCaseAttribute_LegalCaseCondition");

            migrationBuilder.DropTable(
                name: "RequestedCourtOrders");

            migrationBuilder.DropTable(
                name: "LegalCaseActions");

            migrationBuilder.DropTable(
                name: "UserType");

            migrationBuilder.DropTable(
                name: "LegalCaseConditions");

            migrationBuilder.DropTable(
                name: "LegalResponseTemplate");

            migrationBuilder.DropTable(
                name: "NotificationCondition");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserTypeId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestedAnalyses",
                table: "RequestedAnalyses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LegalCaseAttributes",
                table: "LegalCaseAttributes");

            migrationBuilder.DropColumn(
                name: "UserTypeId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "From",
                table: "LegalNotifications");

            migrationBuilder.DropColumn(
                name: "To",
                table: "LegalNotifications");

            migrationBuilder.DropColumn(
                name: "ExpenseAdvances",
                table: "LegalCases");

            migrationBuilder.DropColumn(
                name: "LegalCaseHonorarium",
                table: "LegalCases");

            migrationBuilder.DropColumn(
                name: "IsFulfilled",
                table: "RequestedAnalyses");

            migrationBuilder.RenameTable(
                name: "RequestedAnalyses",
                newName: "RequestedAnalysis");

            migrationBuilder.RenameTable(
                name: "LegalCaseAttributes",
                newName: "LegalCaseAttribute");

            migrationBuilder.RenameIndex(
                name: "IX_RequestedAnalyses_LegalCaseId",
                table: "RequestedAnalysis",
                newName: "IX_RequestedAnalysis_LegalCaseId");

            migrationBuilder.RenameIndex(
                name: "IX_LegalCaseAttributes_LegalCaseId",
                table: "LegalCaseAttribute",
                newName: "IX_LegalCaseAttribute_LegalCaseId");

            migrationBuilder.AddColumn<bool>(
                name: "UseAutomation",
                table: "LegalNotifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "LegalCaseId",
                table: "RequestedAnalysis",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "LegalCaseAttributeConditionId",
                table: "LegalCaseAttribute",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestedAnalysis",
                table: "RequestedAnalysis",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LegalCaseAttribute",
                table: "LegalCaseAttribute",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CourtOrderRequested",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestedCourtOrderId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    SentCourtOrderId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LegalCaseAttributeConditionId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                    PropertyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropertyValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropertyValueType = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalCaseProperty", x => x.Id);
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
                name: "IX_LegalCaseAttribute_LegalCaseAttributeConditionId",
                table: "LegalCaseAttribute",
                column: "LegalCaseAttributeConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_CourtOrderRequested_RequestedCourtOrderId",
                table: "CourtOrderRequested",
                column: "RequestedCourtOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CourtOrderSent_SentCourtOrderId",
                table: "CourtOrderSent",
                column: "SentCourtOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalCaseAttributeCondition_LegalCaseAttributeConditionId",
                table: "LegalCaseAttributeCondition",
                column: "LegalCaseAttributeConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_LegalCaseAttributeConditionLegalCaseProperty_LegalCasePropertiesId",
                table: "LegalCaseAttributeConditionLegalCaseProperty",
                column: "LegalCasePropertiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_LegalCaseAttribute_LegalCaseAttributeCondition_LegalCaseAttributeConditionId",
                table: "LegalCaseAttribute",
                column: "LegalCaseAttributeConditionId",
                principalTable: "LegalCaseAttributeCondition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LegalCaseAttribute_LegalCases_LegalCaseId",
                table: "LegalCaseAttribute",
                column: "LegalCaseId",
                principalTable: "LegalCases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestedAnalysis_LegalCases_LegalCaseId",
                table: "RequestedAnalysis",
                column: "LegalCaseId",
                principalTable: "LegalCases",
                principalColumn: "Id");
        }
    }
}
