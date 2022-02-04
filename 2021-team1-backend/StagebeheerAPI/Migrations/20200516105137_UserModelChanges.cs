using Microsoft.EntityFrameworkCore.Migrations;

namespace StagebeheerAPI.Migrations
{
    public partial class UserModelChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserFirstName",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserSurname",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserFirstName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserSurname",
                table: "User");
        }
    }
}
