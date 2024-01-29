using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceAssessmentService.Application.Migrations;

/// <inheritdoc />
public partial class InitialDatabaseQuestionModel : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Question_AssessmentRequests_AssessmentRequestId",
            table: "Question");

        migrationBuilder.DropForeignKey(
            name: "FK_Question_Question_TemplatedFromId",
            table: "Question");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Question",
            table: "Question");

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

        migrationBuilder.RenameTable(
            name: "Question",
            newName: "Questions");

        migrationBuilder.RenameIndex(
            name: "IX_Question_TemplatedFromId",
            table: "Questions",
            newName: "IX_Questions_TemplatedFromId");

        migrationBuilder.RenameIndex(
            name: "IX_Question_AssessmentRequestId",
            table: "Questions",
            newName: "IX_Questions_AssessmentRequestId");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Questions",
            table: "Questions",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Questions_AssessmentRequests_AssessmentRequestId",
            table: "Questions",
            column: "AssessmentRequestId",
            principalTable: "AssessmentRequests",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Questions_Questions_TemplatedFromId",
            table: "Questions",
            column: "TemplatedFromId",
            principalTable: "Questions",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Questions_AssessmentRequests_AssessmentRequestId",
            table: "Questions");

        migrationBuilder.DropForeignKey(
            name: "FK_Questions_Questions_TemplatedFromId",
            table: "Questions");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Questions",
            table: "Questions");

        migrationBuilder.RenameTable(
            name: "Questions",
            newName: "Question");

        migrationBuilder.RenameIndex(
            name: "IX_Questions_TemplatedFromId",
            table: "Question",
            newName: "IX_Question_TemplatedFromId");

        migrationBuilder.RenameIndex(
            name: "IX_Questions_AssessmentRequestId",
            table: "Question",
            newName: "IX_Question_AssessmentRequestId");

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

        migrationBuilder.AddPrimaryKey(
            name: "PK_Question",
            table: "Question",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Question_AssessmentRequests_AssessmentRequestId",
            table: "Question",
            column: "AssessmentRequestId",
            principalTable: "AssessmentRequests",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Question_Question_TemplatedFromId",
            table: "Question",
            column: "TemplatedFromId",
            principalTable: "Question",
            principalColumn: "Id");
    }
}
