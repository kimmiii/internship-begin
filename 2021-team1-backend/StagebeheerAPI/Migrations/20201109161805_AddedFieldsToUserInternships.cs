using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StagebeheerAPI.Migrations
{
    public partial class AddedFieldsToUserInternships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EvaluatedAt",
                table: "UserInternships",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternalFeedback",
                table: "UserInternships",
                type: "nvarchar(max)",
                unicode: false,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EvaluatedAt",
                table: "UserInternships");

            migrationBuilder.DropColumn(
                name: "InternalFeedback",
                table: "UserInternships");
        }
    }
}
