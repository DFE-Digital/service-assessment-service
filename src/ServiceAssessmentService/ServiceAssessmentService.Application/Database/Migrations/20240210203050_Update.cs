using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceAssessmentService.Application.Migrations;

/// <inheritdoc />
public partial class Update : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "PeriodEnd",
            table: "AssessmentRequests")
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.DropColumn(
            name: "PeriodStart",
            table: "AssessmentRequests")
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterTable(
            name: "AssessmentRequests")
            .OldAnnotation("SqlServer:IsTemporal", true)
            .OldAnnotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
            .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<DateTime>(
            name: "UpdatedUtc",
            table: "AssessmentRequests",
            type: "datetime2",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "datetime2")
            .OldAnnotation("SqlServer:IsTemporal", true)
            .OldAnnotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
            .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<Guid>(
            name: "SeniorResponsibleOfficerId",
            table: "AssessmentRequests",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true)
            .OldAnnotation("SqlServer:IsTemporal", true)
            .OldAnnotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
            .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<string>(
            name: "RequestedReviewDates",
            table: "AssessmentRequests",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)")
            .OldAnnotation("SqlServer:IsTemporal", true)
            .OldAnnotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
            .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<string>(
            name: "ProjectCode",
            table: "AssessmentRequests",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true)
            .OldAnnotation("SqlServer:IsTemporal", true)
            .OldAnnotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
            .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<Guid>(
            name: "ProductOwnerManagerId",
            table: "AssessmentRequests",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true)
            .OldAnnotation("SqlServer:IsTemporal", true)
            .OldAnnotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
            .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<Guid>(
            name: "PortfolioId",
            table: "AssessmentRequests",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true)
            .OldAnnotation("SqlServer:IsTemporal", true)
            .OldAnnotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
            .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<DateOnly>(
            name: "PhaseStartDate",
            table: "AssessmentRequests",
            type: "date",
            nullable: true,
            oldClrType: typeof(DateOnly),
            oldType: "date",
            oldNullable: true)
            .OldAnnotation("SqlServer:IsTemporal", true)
            .OldAnnotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
            .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<DateOnly>(
            name: "PhaseEndDate",
            table: "AssessmentRequests",
            type: "date",
            nullable: true,
            oldClrType: typeof(DateOnly),
            oldType: "date",
            oldNullable: true)
            .OldAnnotation("SqlServer:IsTemporal", true)
            .OldAnnotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
            .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<Guid>(
            name: "PhaseConcludingId",
            table: "AssessmentRequests",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true)
            .OldAnnotation("SqlServer:IsTemporal", true)
            .OldAnnotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
            .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "AssessmentRequests",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)")
            .OldAnnotation("SqlServer:IsTemporal", true)
            .OldAnnotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
            .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<bool>(
            name: "IsReassessment",
            table: "AssessmentRequests",
            type: "bit",
            nullable: true,
            oldClrType: typeof(bool),
            oldType: "bit",
            oldNullable: true)
            .OldAnnotation("SqlServer:IsTemporal", true)
            .OldAnnotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
            .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<bool>(
            name: "IsProjectCodeKnown",
            table: "AssessmentRequests",
            type: "bit",
            nullable: true,
            oldClrType: typeof(bool),
            oldType: "bit",
            oldNullable: true)
            .OldAnnotation("SqlServer:IsTemporal", true)
            .OldAnnotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
            .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<bool>(
            name: "IsPhaseEndDateKnown",
            table: "AssessmentRequests",
            type: "bit",
            nullable: true,
            oldClrType: typeof(bool),
            oldType: "bit",
            oldNullable: true)
            .OldAnnotation("SqlServer:IsTemporal", true)
            .OldAnnotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
            .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<bool>(
            name: "IsDeputyDirectorTheSeniorResponsibleOfficer",
            table: "AssessmentRequests",
            type: "bit",
            nullable: true,
            oldClrType: typeof(bool),
            oldType: "bit",
            oldNullable: true)
            .OldAnnotation("SqlServer:IsTemporal", true)
            .OldAnnotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
            .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<bool>(
            name: "HasProductOwnerManager",
            table: "AssessmentRequests",
            type: "bit",
            nullable: true,
            oldClrType: typeof(bool),
            oldType: "bit",
            oldNullable: true)
            .OldAnnotation("SqlServer:IsTemporal", true)
            .OldAnnotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
            .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<bool>(
            name: "HasDeliveryManager",
            table: "AssessmentRequests",
            type: "bit",
            nullable: true,
            oldClrType: typeof(bool),
            oldType: "bit",
            oldNullable: true)
            .OldAnnotation("SqlServer:IsTemporal", true)
            .OldAnnotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
            .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<string>(
            name: "Description",
            table: "AssessmentRequests",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true)
            .OldAnnotation("SqlServer:IsTemporal", true)
            .OldAnnotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
            .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<Guid>(
            name: "DeputyDirectorId",
            table: "AssessmentRequests",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true)
            .OldAnnotation("SqlServer:IsTemporal", true)
            .OldAnnotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
            .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<Guid>(
            name: "DeliveryManagerId",
            table: "AssessmentRequests",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true)
            .OldAnnotation("SqlServer:IsTemporal", true)
            .OldAnnotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
            .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<DateTime>(
            name: "CreatedUtc",
            table: "AssessmentRequests",
            type: "datetime2",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "datetime2")
            .OldAnnotation("SqlServer:IsTemporal", true)
            .OldAnnotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
            .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<Guid>(
            name: "AssessmentTypeRequestedId",
            table: "AssessmentRequests",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true)
            .OldAnnotation("SqlServer:IsTemporal", true)
            .OldAnnotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
            .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "AssessmentRequests",
            type: "uniqueidentifier",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier")
            .OldAnnotation("SqlServer:IsTemporal", true)
            .OldAnnotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
            .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterTable(
            name: "AssessmentRequests")
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<DateTime>(
            name: "UpdatedUtc",
            table: "AssessmentRequests",
            type: "datetime2",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "datetime2")
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<Guid>(
            name: "SeniorResponsibleOfficerId",
            table: "AssessmentRequests",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true)
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<string>(
            name: "RequestedReviewDates",
            table: "AssessmentRequests",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)")
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<string>(
            name: "ProjectCode",
            table: "AssessmentRequests",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true)
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<Guid>(
            name: "ProductOwnerManagerId",
            table: "AssessmentRequests",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true)
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<Guid>(
            name: "PortfolioId",
            table: "AssessmentRequests",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true)
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<DateOnly>(
            name: "PhaseStartDate",
            table: "AssessmentRequests",
            type: "date",
            nullable: true,
            oldClrType: typeof(DateOnly),
            oldType: "date",
            oldNullable: true)
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<DateOnly>(
            name: "PhaseEndDate",
            table: "AssessmentRequests",
            type: "date",
            nullable: true,
            oldClrType: typeof(DateOnly),
            oldType: "date",
            oldNullable: true)
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<Guid>(
            name: "PhaseConcludingId",
            table: "AssessmentRequests",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true)
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "AssessmentRequests",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true)
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<bool>(
            name: "IsReassessment",
            table: "AssessmentRequests",
            type: "bit",
            nullable: true,
            oldClrType: typeof(bool),
            oldType: "bit",
            oldNullable: true)
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<bool>(
            name: "IsProjectCodeKnown",
            table: "AssessmentRequests",
            type: "bit",
            nullable: true,
            oldClrType: typeof(bool),
            oldType: "bit",
            oldNullable: true)
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<bool>(
            name: "IsPhaseEndDateKnown",
            table: "AssessmentRequests",
            type: "bit",
            nullable: true,
            oldClrType: typeof(bool),
            oldType: "bit",
            oldNullable: true)
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<bool>(
            name: "IsDeputyDirectorTheSeniorResponsibleOfficer",
            table: "AssessmentRequests",
            type: "bit",
            nullable: true,
            oldClrType: typeof(bool),
            oldType: "bit",
            oldNullable: true)
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<bool>(
            name: "HasProductOwnerManager",
            table: "AssessmentRequests",
            type: "bit",
            nullable: true,
            oldClrType: typeof(bool),
            oldType: "bit",
            oldNullable: true)
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<bool>(
            name: "HasDeliveryManager",
            table: "AssessmentRequests",
            type: "bit",
            nullable: true,
            oldClrType: typeof(bool),
            oldType: "bit",
            oldNullable: true)
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<string>(
            name: "Description",
            table: "AssessmentRequests",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true)
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<Guid>(
            name: "DeputyDirectorId",
            table: "AssessmentRequests",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true)
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<Guid>(
            name: "DeliveryManagerId",
            table: "AssessmentRequests",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true)
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<DateTime>(
            name: "CreatedUtc",
            table: "AssessmentRequests",
            type: "datetime2",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "datetime2")
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<Guid>(
            name: "AssessmentTypeRequestedId",
            table: "AssessmentRequests",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true)
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "AssessmentRequests",
            type: "uniqueidentifier",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier")
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AddColumn<DateTime>(
            name: "PeriodEnd",
            table: "AssessmentRequests",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

        migrationBuilder.AddColumn<DateTime>(
            name: "PeriodStart",
            table: "AssessmentRequests",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
            .Annotation("SqlServer:IsTemporal", true)
            .Annotation("SqlServer:TemporalHistoryTableName", "AssessmentRequestsHistory")
            .Annotation("SqlServer:TemporalHistoryTableSchema", null)
            .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
            .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
    }
}
