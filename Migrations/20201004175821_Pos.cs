using Microsoft.EntityFrameworkCore.Migrations;

namespace pax.blazor.survey.Migrations
{
    public partial class Pos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Pos",
                table: "Surveys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Pos",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Pos",
                table: "Options",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pos",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "Pos",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Pos",
                table: "Options");
        }
    }
}
