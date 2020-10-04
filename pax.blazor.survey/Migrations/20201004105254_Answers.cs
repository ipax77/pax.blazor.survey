using Microsoft.EntityFrameworkCore.Migrations;

namespace pax.blazor.survey.Migrations
{
    public partial class Answers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answer_Responses_ResponseID",
                table: "Answer");

            migrationBuilder.DropForeignKey(
                name: "FK_OptionQuestion_Option_OptionsID",
                table: "OptionQuestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Option",
                table: "Option");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Answer",
                table: "Answer");

            migrationBuilder.RenameTable(
                name: "Option",
                newName: "Options");

            migrationBuilder.RenameTable(
                name: "Answer",
                newName: "Answers");

            migrationBuilder.RenameIndex(
                name: "IX_Answer_ResponseID",
                table: "Answers",
                newName: "IX_Answers_ResponseID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Options",
                table: "Options",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Answers",
                table: "Answers",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Responses_ResponseID",
                table: "Answers",
                column: "ResponseID",
                principalTable: "Responses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OptionQuestion_Options_OptionsID",
                table: "OptionQuestion",
                column: "OptionsID",
                principalTable: "Options",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Responses_ResponseID",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_OptionQuestion_Options_OptionsID",
                table: "OptionQuestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Options",
                table: "Options");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Answers",
                table: "Answers");

            migrationBuilder.RenameTable(
                name: "Options",
                newName: "Option");

            migrationBuilder.RenameTable(
                name: "Answers",
                newName: "Answer");

            migrationBuilder.RenameIndex(
                name: "IX_Answers_ResponseID",
                table: "Answer",
                newName: "IX_Answer_ResponseID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Option",
                table: "Option",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Answer",
                table: "Answer",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_Responses_ResponseID",
                table: "Answer",
                column: "ResponseID",
                principalTable: "Responses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OptionQuestion_Option_OptionsID",
                table: "OptionQuestion",
                column: "OptionsID",
                principalTable: "Option",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
