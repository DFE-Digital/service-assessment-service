using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceAssessmentService.Application.Migrations;

/// <inheritdoc />
public partial class FixScaffoldingOfOtherQuestionTypes : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "RadioQuestionId",
            table: "RadioOptions",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<DateOnly>(
            name: "DateOnlyAnswer",
            table: "Questions",
            type: "date",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Discriminator",
            table: "Questions",
            type: "nvarchar(21)",
            maxLength: 21,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "LongTextAnswer",
            table: "Questions",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "SelectedOptionId",
            table: "Questions",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "SimpleTextAnswer",
            table: "Questions",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_RadioOptions_RadioQuestionId",
            table: "RadioOptions",
            column: "RadioQuestionId");

        migrationBuilder.CreateIndex(
            name: "IX_Questions_SelectedOptionId",
            table: "Questions",
            column: "SelectedOptionId");

        migrationBuilder.AddForeignKey(
            name: "FK_Questions_RadioOptions_SelectedOptionId",
            table: "Questions",
            column: "SelectedOptionId",
            principalTable: "RadioOptions",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_RadioOptions_Questions_RadioQuestionId",
            table: "RadioOptions",
            column: "RadioQuestionId",
            principalTable: "Questions",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Questions_RadioOptions_SelectedOptionId",
            table: "Questions");

        migrationBuilder.DropForeignKey(
            name: "FK_RadioOptions_Questions_RadioQuestionId",
            table: "RadioOptions");

        migrationBuilder.DropIndex(
            name: "IX_RadioOptions_RadioQuestionId",
            table: "RadioOptions");

        migrationBuilder.DropIndex(
            name: "IX_Questions_SelectedOptionId",
            table: "Questions");

        migrationBuilder.DropColumn(
            name: "RadioQuestionId",
            table: "RadioOptions");

        migrationBuilder.DropColumn(
            name: "DateOnlyAnswer",
            table: "Questions");

        migrationBuilder.DropColumn(
            name: "Discriminator",
            table: "Questions");

        migrationBuilder.DropColumn(
            name: "LongTextAnswer",
            table: "Questions");

        migrationBuilder.DropColumn(
            name: "SelectedOptionId",
            table: "Questions");

        migrationBuilder.DropColumn(
            name: "SimpleTextAnswer",
            table: "Questions");
    }
}
