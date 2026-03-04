using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Edufy.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class ModifyApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                schema: "gp_edufy",
                table: "Applications");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "gp_edufy",
                table: "Applications",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "gp_edufy",
                table: "Applications",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "gp_edufy",
                table: "Applications",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "gp_edufy",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "gp_edufy",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "gp_edufy",
                table: "Applications");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                schema: "gp_edufy",
                table: "Applications",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);
        }
    }
}
