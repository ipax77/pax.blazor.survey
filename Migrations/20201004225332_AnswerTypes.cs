using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace pax.blazor.survey.Migrations
{
    public partial class AnswerTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AnswerBool",
                table: "Answers",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswerBool",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "AnswerDate",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "AnswerFloat",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "AnswerInt",
                table: "Answers");
        }
    }
}
