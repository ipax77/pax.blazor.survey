using Microsoft.EntityFrameworkCore.Migrations;

namespace pax.blazor.survey.Migrations
{
    public partial class suburl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubUrl",
                table: "Surveys",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubUrl",
                table: "Surveys");
        }
    }
}
