using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StagebeheerAPI.Migrations
{
    public partial class AdditionsCompanyModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EvaluatedAt",
                table: "Company",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EvaluationFeedback",
                table: "Company",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EvaluatedAt",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "EvaluationFeedback",
                table: "Company");
        }
    }
}
