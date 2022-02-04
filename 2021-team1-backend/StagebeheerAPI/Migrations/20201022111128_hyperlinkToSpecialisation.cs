using Microsoft.EntityFrameworkCore.Migrations;

namespace StagebeheerAPI.Migrations
{
    public partial class hyperlinkToSpecialisation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hyperlink",
                table: "Specialisation",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hyperlink",
                table: "Specialisation");
        }
    }
}
