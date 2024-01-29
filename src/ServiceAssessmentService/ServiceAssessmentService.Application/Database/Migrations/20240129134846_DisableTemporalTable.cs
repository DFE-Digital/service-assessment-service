using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceAssessmentService.Application.Migrations;

/// <inheritdoc />
public partial class DisableTemporalTable : Migration
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
