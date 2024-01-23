using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceAssessmentService.Application.Migrations;

/// <inheritdoc />
public partial class Simplify : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Questions")
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "QuestionsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AddColumn<string>(
            name: "AssessmentType",
            table: "AssessmentRequests",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "")
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AddColumn<string>(
            name: "Description",
            table: "AssessmentRequests",
            type: "nvarchar(max)",
            nullable: true)
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AddColumn<string>(
            name: "Name",
            table: "AssessmentRequests",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "")
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AddColumn<string>(
            name: "PhaseConcluding",
            table: "AssessmentRequests",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "")
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AddColumn<DateOnly>(
            name: "PhaseEndDate",
            table: "AssessmentRequests",
            type: "date",
            nullable: true)
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AddColumn<DateOnly>(
            name: "PhaseStartDate",
            table: "AssessmentRequests",
            type: "date",
            nullable: true)
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "AssessmentType",
            table: "AssessmentRequests")
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.DropColumn(
            name: "Description",
            table: "AssessmentRequests")
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.DropColumn(
            name: "Name",
            table: "AssessmentRequests")
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.DropColumn(
            name: "PhaseConcluding",
            table: "AssessmentRequests")
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.DropColumn(
            name: "PhaseEndDate",
            table: "AssessmentRequests")
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.DropColumn(
            name: "PhaseStartDate",
            table: "AssessmentRequests")
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.CreateTable(
            name: "Questions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                    .Annotation("SqlServer:IsTemporal", true)
                    .Annotation("SqlServer:TemporalHistoryTableName", "QuestionsHistory")
                    .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                    .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                    .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                AssessmentRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                    .Annotation("SqlServer:IsTemporal", true)
                    .Annotation("SqlServer:TemporalHistoryTableName", "QuestionsHistory")
                    .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                    .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                    .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                HintText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                    .Annotation("SqlServer:IsTemporal", true)
                    .Annotation("SqlServer:TemporalHistoryTableName", "QuestionsHistory")
                    .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                    .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                    .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                    .Annotation("SqlServer:IsTemporal", true)
                    .Annotation("SqlServer:TemporalHistoryTableName", "QuestionsHistory")
                    .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                    .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                    .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                    .Annotation("SqlServer:IsTemporal", true)
                    .Annotation("SqlServer:TemporalHistoryTableName", "QuestionsHistory")
                    .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                    .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                    .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                Response = table.Column<string>(type: "nvarchar(max)", nullable: true)
                    .Annotation("SqlServer:IsTemporal", true)
                    .Annotation("SqlServer:TemporalHistoryTableName", "QuestionsHistory")
                    .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                    .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                    .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                TemplatedFromId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                    .Annotation("SqlServer:IsTemporal", true)
                    .Annotation("SqlServer:TemporalHistoryTableName", "QuestionsHistory")
                    .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                    .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                    .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                    .Annotation("SqlServer:IsTemporal", true)
                    .Annotation("SqlServer:TemporalHistoryTableName", "QuestionsHistory")
                    .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                    .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                    .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                    .Annotation("SqlServer:IsTemporal", true)
                    .Annotation("SqlServer:TemporalHistoryTableName", "QuestionsHistory")
                    .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                    .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                    .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Questions", x => x.Id);
                table.ForeignKey(
                    name: "FK_Questions_AssessmentRequests_AssessmentRequestId",
                    column: x => x.AssessmentRequestId,
                    principalTable: "AssessmentRequests",
                    principalColumn: "Id");
            })
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "QuestionsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.CreateIndex(
            name: "IX_Questions_AssessmentRequestId",
            table: "Questions",
            column: "AssessmentRequestId");
    }
}
