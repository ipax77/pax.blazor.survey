using Microsoft.EntityFrameworkCore.Migrations;

namespace pax.blazor.survey.Migrations
{
    public partial class Options : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Surveys_SurveyID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SurveyID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SurveyID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Answers",
                table: "Questions");

            migrationBuilder.AddColumn<int>(
                name: "SurveyID",
                table: "Responses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Answer_Responses_ResponseID",
                        column: x => x.ResponseID,
                        principalTable: "Responses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Option",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OptionValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Option", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SurveyUser",
                columns: table => new
                {
                    SurveysID = table.Column<int>(type: "int", nullable: false),
                    UsersID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyUser", x => new { x.SurveysID, x.UsersID });
                    table.ForeignKey(
                        name: "FK_SurveyUser_Surveys_SurveysID",
                        column: x => x.SurveysID,
                        principalTable: "Surveys",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SurveyUser_Users_UsersID",
                        column: x => x.UsersID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OptionQuestion",
                columns: table => new
                {
                    OptionsID = table.Column<int>(type: "int", nullable: false),
                    QuestionsID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionQuestion", x => new { x.OptionsID, x.QuestionsID });
                    table.ForeignKey(
                        name: "FK_OptionQuestion_Option_OptionsID",
                        column: x => x.OptionsID,
                        principalTable: "Option",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OptionQuestion_Questions_QuestionsID",
                        column: x => x.QuestionsID,
                        principalTable: "Questions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Responses_SurveyID",
                table: "Responses",
                column: "SurveyID");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_ResponseID",
                table: "Answer",
                column: "ResponseID");

            migrationBuilder.CreateIndex(
                name: "IX_OptionQuestion_QuestionsID",
                table: "OptionQuestion",
                column: "QuestionsID");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyUser_UsersID",
                table: "SurveyUser",
                column: "UsersID");

            migrationBuilder.AddForeignKey(
                name: "FK_Responses_Surveys_SurveyID",
                table: "Responses",
                column: "SurveyID",
                principalTable: "Surveys",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Responses_Surveys_SurveyID",
                table: "Responses");

            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropTable(
                name: "OptionQuestion");

            migrationBuilder.DropTable(
                name: "SurveyUser");

            migrationBuilder.DropTable(
                name: "Option");

            migrationBuilder.DropIndex(
                name: "IX_Responses_SurveyID",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "SurveyID",
                table: "Responses");

            migrationBuilder.AddColumn<int>(
                name: "SurveyID",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Answers",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_SurveyID",
                table: "Users",
                column: "SurveyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Surveys_SurveyID",
                table: "Users",
                column: "SurveyID",
                principalTable: "Surveys",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
