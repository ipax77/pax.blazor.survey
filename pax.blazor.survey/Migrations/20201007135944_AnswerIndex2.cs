using Microsoft.EntityFrameworkCore.Migrations;

namespace pax.blazor.survey.Migrations
{
    public partial class AnswerIndex2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Feedback",
                table: "Responses");

            migrationBuilder.AddColumn<bool>(
                name: "Selected",
                table: "Responses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AnswerValue",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Selected",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "AnswerValue",
                table: "Answers");

            migrationBuilder.AddColumn<string>(
                name: "Feedback",
                table: "Responses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
