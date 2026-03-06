using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Edufy.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddSavedLessons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavedPrograms",
                schema: "gp_edufy");

            migrationBuilder.AddColumn<bool>(
                name: "IsDemo",
                schema: "gp_edufy",
                table: "Lessons",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                schema: "gp_edufy",
                table: "Lessons",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SavedLessons",
                schema: "gp_edufy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonId = table.Column<int>(type: "integer", nullable: false),
                    SavedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedLessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedLessons_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "gp_edufy",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedLessons_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalSchema: "gp_edufy",
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SavedLessons_LessonId",
                schema: "gp_edufy",
                table: "SavedLessons",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedLessons_UserId_LessonId",
                schema: "gp_edufy",
                table: "SavedLessons",
                columns: new[] { "UserId", "LessonId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavedLessons",
                schema: "gp_edufy");

            migrationBuilder.DropColumn(
                name: "IsDemo",
                schema: "gp_edufy",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                schema: "gp_edufy",
                table: "Lessons");

            migrationBuilder.CreateTable(
                name: "SavedPrograms",
                schema: "gp_edufy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SavedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedPrograms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedPrograms_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "gp_edufy",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedPrograms_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalSchema: "gp_edufy",
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SavedPrograms_ProgramId",
                schema: "gp_edufy",
                table: "SavedPrograms",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedPrograms_UserId_ProgramId",
                schema: "gp_edufy",
                table: "SavedPrograms",
                columns: new[] { "UserId", "ProgramId" },
                unique: true);
        }
    }
}
