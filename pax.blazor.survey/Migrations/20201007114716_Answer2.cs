using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace pax.blazor.survey.Migrations
{
    public partial class Answer2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswerDate",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "AnswerFloat",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "AnswerInt",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "AnswerValue",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "AnswerBool",
                table: "Answers",
                newName: "Selected");

            migrationBuilder.AlterColumn<string>(
                name: "SubUrl",
                table: "Surveys",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Pos",
                table: "Responses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OptionID",
                table: "Answers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_OptionID",
                table: "Answers",
                column: "OptionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Options_OptionID",
                table: "Answers",
                column: "OptionID",
                principalTable: "Options",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Options_OptionID",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_OptionID",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "Pos",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "OptionID",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "Selected",
                table: "Answers",
                newName: "AnswerBool");

            migrationBuilder.AlterColumn<string>(
                name: "SubUrl",
                table: "Surveys",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "AnswerDate",
                table: "Answers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AnswerFloat",
                table: "Answers",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "AnswerInt",
                table: "Answers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AnswerValue",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
