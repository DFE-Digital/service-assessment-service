using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceAssessmentService.Application.Migrations;

/// <inheritdoc />
public partial class SplitPersonName : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Name",
            table: "People");

        migrationBuilder.AlterColumn<string>(
            name: "Email",
            table: "People",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AddColumn<string>(
            name: "FamilyName",
            table: "People",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "PersonalName",
            table: "People",
            type: "nvarchar(max)",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "FamilyName",
            table: "People");

        migrationBuilder.DropColumn(
            name: "PersonalName",
            table: "People");

        migrationBuilder.AlterColumn<string>(
            name: "Email",
            table: "People",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Name",
            table: "People",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");
    }
}
