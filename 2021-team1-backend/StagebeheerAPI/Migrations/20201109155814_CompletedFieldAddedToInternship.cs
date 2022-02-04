using Microsoft.EntityFrameworkCore.Migrations;

namespace StagebeheerAPI.Migrations
{
    public partial class CompletedFieldAddedToInternship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "Internship",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "Internship");
        }
    }
}
