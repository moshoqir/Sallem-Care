using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaleemCare.Api.Migrations
{
    /// <inheritdoc />
    public partial class Encounter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EncounterId",
                table: "UserSymptomAnswers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Encounters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encounters", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSymptomAnswers_EncounterId",
                table: "UserSymptomAnswers",
                column: "EncounterId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSymptomAnswers_UserId_SymptomId_SymptomQuestionId",
                table: "UserSymptomAnswers",
                columns: new[] { "UserId", "SymptomId", "SymptomQuestionId" });

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_UserId_StartedAt",
                table: "Encounters",
                columns: new[] { "UserId", "StartedAt" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserSymptomAnswers_Encounters_EncounterId",
                table: "UserSymptomAnswers",
                column: "EncounterId",
                principalTable: "Encounters",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSymptomAnswers_Encounters_EncounterId",
                table: "UserSymptomAnswers");

            migrationBuilder.DropTable(
                name: "Encounters");

            migrationBuilder.DropIndex(
                name: "IX_UserSymptomAnswers_EncounterId",
                table: "UserSymptomAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UserSymptomAnswers_UserId_SymptomId_SymptomQuestionId",
                table: "UserSymptomAnswers");

            migrationBuilder.DropColumn(
                name: "EncounterId",
                table: "UserSymptomAnswers");
        }
    }
}
