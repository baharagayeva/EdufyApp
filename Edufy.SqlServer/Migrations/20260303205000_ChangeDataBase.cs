using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Edufy.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_CourseModules_CourseModuleId",
                schema: "gp_edufy",
                table: "Lessons");

            migrationBuilder.DropTable(
                name: "CourseApplications",
                schema: "gp_edufy");

            migrationBuilder.DropTable(
                name: "CourseModules",
                schema: "gp_edufy");

            migrationBuilder.DropTable(
                name: "Courses",
                schema: "gp_edufy");

            migrationBuilder.DropTable(
                name: "InstructorProfiles",
                schema: "gp_edufy");

            migrationBuilder.DropColumn(
                name: "Content",
                schema: "gp_edufy",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "gp_edufy",
                table: "Lessons");

            migrationBuilder.RenameColumn(
                name: "CourseModuleId",
                schema: "gp_edufy",
                table: "Lessons",
                newName: "ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_CourseModuleId_Order",
                schema: "gp_edufy",
                table: "Lessons",
                newName: "IX_Lessons_ModuleId_Order");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "gp_edufy",
                table: "PasswordResetCodes",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CodeHash",
                schema: "gp_edufy",
                table: "PasswordResetCodes",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                schema: "gp_edufy",
                table: "Lessons",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "gp_edufy",
                table: "Lessons",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                schema: "gp_edufy",
                table: "Lessons",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Academies",
                schema: "gp_edufy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    LogoUrl = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    About = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    TotalApplications = table.Column<int>(type: "integer", nullable: false),
                    TotalStudents = table.Column<int>(type: "integer", nullable: false),
                    GraduationRate = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Academies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instructors",
                schema: "gp_edufy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Specialization = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Bio = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    PhotoUrl = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    PriceAzn = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                    LinkedInUrl = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Programs",
                schema: "gp_edufy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AcademyId = table.Column<int>(type: "integer", nullable: false),
                    InstructorId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    About = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: true),
                    DurationMonths = table.Column<int>(type: "integer", nullable: false),
                    GroupMinSize = table.Column<int>(type: "integer", nullable: false),
                    GroupMaxSize = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Programs_Academies_AcademyId",
                        column: x => x.AcademyId,
                        principalSchema: "gp_edufy",
                        principalTable: "Academies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Programs_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalSchema: "gp_edufy",
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                schema: "gp_edufy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProgramId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AppliedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Note = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "gp_edufy",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Applications_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalSchema: "gp_edufy",
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                schema: "gp_edufy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramId = table.Column<int>(type: "integer", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    IsOpen = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modules_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalSchema: "gp_edufy",
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SavedPrograms",
                schema: "gp_edufy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProgramId = table.Column<int>(type: "integer", nullable: false),
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
                name: "IX_PasswordResetCodes_Email",
                schema: "gp_edufy",
                table: "PasswordResetCodes",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetCodes_Email_ExpiresAt",
                schema: "gp_edufy",
                table: "PasswordResetCodes",
                columns: new[] { "Email", "ExpiresAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ProgramId",
                schema: "gp_edufy",
                table: "Applications",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_UserId_ProgramId",
                schema: "gp_edufy",
                table: "Applications",
                columns: new[] { "UserId", "ProgramId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_ProgramId_Order",
                schema: "gp_edufy",
                table: "Modules",
                columns: new[] { "ProgramId", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Programs_AcademyId",
                schema: "gp_edufy",
                table: "Programs",
                column: "AcademyId");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_InstructorId",
                schema: "gp_edufy",
                table: "Programs",
                column: "InstructorId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Modules_ModuleId",
                schema: "gp_edufy",
                table: "Lessons",
                column: "ModuleId",
                principalSchema: "gp_edufy",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Modules_ModuleId",
                schema: "gp_edufy",
                table: "Lessons");

            migrationBuilder.DropTable(
                name: "Applications",
                schema: "gp_edufy");

            migrationBuilder.DropTable(
                name: "Modules",
                schema: "gp_edufy");

            migrationBuilder.DropTable(
                name: "SavedPrograms",
                schema: "gp_edufy");

            migrationBuilder.DropTable(
                name: "Programs",
                schema: "gp_edufy");

            migrationBuilder.DropTable(
                name: "Academies",
                schema: "gp_edufy");

            migrationBuilder.DropTable(
                name: "Instructors",
                schema: "gp_edufy");

            migrationBuilder.DropIndex(
                name: "IX_PasswordResetCodes_Email",
                schema: "gp_edufy",
                table: "PasswordResetCodes");

            migrationBuilder.DropIndex(
                name: "IX_PasswordResetCodes_Email_ExpiresAt",
                schema: "gp_edufy",
                table: "PasswordResetCodes");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                schema: "gp_edufy",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "gp_edufy",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                schema: "gp_edufy",
                table: "Lessons");

            migrationBuilder.RenameColumn(
                name: "ModuleId",
                schema: "gp_edufy",
                table: "Lessons",
                newName: "CourseModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_ModuleId_Order",
                schema: "gp_edufy",
                table: "Lessons",
                newName: "IX_Lessons_CourseModuleId_Order");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "gp_edufy",
                table: "PasswordResetCodes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "CodeHash",
                schema: "gp_edufy",
                table: "PasswordResetCodes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                schema: "gp_edufy",
                table: "Lessons",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "gp_edufy",
                table: "Lessons",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "InstructorProfiles",
                schema: "gp_edufy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Bio = table.Column<string>(type: "text", nullable: false),
                    LinkedInUrl = table.Column<string>(type: "text", nullable: true),
                    PhotoUrl = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstructorProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "gp_edufy",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                schema: "gp_edufy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InstructorProfileId = table.Column<int>(type: "integer", nullable: false),
                    CoverImageUrl = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DurationMonths = table.Column<int>(type: "integer", nullable: false),
                    GroupSizeMax = table.Column<int>(type: "integer", nullable: false),
                    GroupSizeMin = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsPopular = table.Column<bool>(type: "boolean", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_InstructorProfiles_InstructorProfileId",
                        column: x => x.InstructorProfileId,
                        principalSchema: "gp_edufy",
                        principalTable: "InstructorProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseApplications",
                schema: "gp_edufy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CourseId = table.Column<int>(type: "integer", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    AppliedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StudentUserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseApplications_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "gp_edufy",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseApplications_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "gp_edufy",
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseModules",
                schema: "gp_edufy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CourseId = table.Column<int>(type: "integer", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseModules_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "gp_edufy",
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseApplications_CourseId",
                schema: "gp_edufy",
                table: "CourseApplications",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseApplications_StudentId",
                schema: "gp_edufy",
                table: "CourseApplications",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseModules_CourseId_Order",
                schema: "gp_edufy",
                table: "CourseModules",
                columns: new[] { "CourseId", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_InstructorProfileId",
                schema: "gp_edufy",
                table: "Courses",
                column: "InstructorProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorProfiles_UserId",
                schema: "gp_edufy",
                table: "InstructorProfiles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_CourseModules_CourseModuleId",
                schema: "gp_edufy",
                table: "Lessons",
                column: "CourseModuleId",
                principalSchema: "gp_edufy",
                principalTable: "CourseModules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
