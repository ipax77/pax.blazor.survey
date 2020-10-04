using Microsoft.EntityFrameworkCore.Migrations;

namespace pax.blazor.survey.Migrations
{
    public partial class BoolQs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowProgress",
                table: "Surveys",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowResult",
                table: "Surveys",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowProgress",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "ShowResult",
                table: "Surveys");
        }
    }
}
