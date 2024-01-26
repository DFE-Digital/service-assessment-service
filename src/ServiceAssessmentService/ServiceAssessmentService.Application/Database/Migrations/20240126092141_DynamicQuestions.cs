using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceAssessmentService.Application.Migrations
{
    /// <inheritdoc />
    public partial class DynamicQuestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplatedFromId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssessmentRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HintText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Question_AssessmentRequests_AssessmentRequestId",
                        column: x => x.AssessmentRequestId,
                        principalTable: "AssessmentRequests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Question_Question_TemplatedFromId",
                        column: x => x.TemplatedFromId,
                        principalTable: "Question",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Question_AssessmentRequestId",
                table: "Question",
                column: "AssessmentRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_TemplatedFromId",
                table: "Question",
                column: "TemplatedFromId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Question");
        }
    }
}
