using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaleemCare.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Symptoms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Symptoms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasDiabetes = table.Column<bool>(type: "bit", nullable: false),
                    HasHypertension = table.Column<bool>(type: "bit", nullable: false),
                    IsAthlete = table.Column<bool>(type: "bit", nullable: false),
                    SurgicalHistoryJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dob = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSymptomAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SymptomId = table.Column<int>(type: "int", nullable: false),
                    SymptomQuestionId = table.Column<int>(type: "int", nullable: false),
                    AnswerJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSymptomAnswers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegionSymptoms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    SymptomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionSymptoms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegionSymptoms_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegionSymptoms_Symptoms_SymptomId",
                        column: x => x.SymptomId,
                        principalTable: "Symptoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SymptomQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SymptomId = table.Column<int>(type: "int", nullable: false),
                    QuestionAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionsArJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymptomQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SymptomQuestions_Symptoms_SymptomId",
                        column: x => x.SymptomId,
                        principalTable: "Symptoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Slug",
                table: "Regions",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegionSymptoms_RegionId_SymptomId",
                table: "RegionSymptoms",
                columns: new[] { "RegionId", "SymptomId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegionSymptoms_SymptomId",
                table: "RegionSymptoms",
                column: "SymptomId");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomQuestions_SymptomId",
                table: "SymptomQuestions",
                column: "SymptomId");

            migrationBuilder.CreateIndex(
                name: "IX_Symptoms_Slug",
                table: "Symptoms",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegionSymptoms");

            migrationBuilder.DropTable(
                name: "SymptomQuestions");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "UserSymptomAnswers");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Symptoms");
        }
    }
}
