using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StagebeheerAPI.Migrations
{
    public partial class updateinternship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountTotalAssignedReviewers",
                table: "Internship",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SentToReviewersAt",
                table: "Internship",
                type: "datetime",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountTotalAssignedReviewers",
                table: "Internship");

            migrationBuilder.DropColumn(
                name: "SentToReviewersAt",
                table: "Internship");
        }
    }
}
