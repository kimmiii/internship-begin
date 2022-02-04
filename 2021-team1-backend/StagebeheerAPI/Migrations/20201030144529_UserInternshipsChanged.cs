using Microsoft.EntityFrameworkCore.Migrations;

namespace StagebeheerAPI.Migrations
{
    public partial class UserInternshipsChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HireApproved",
                table: "UserInternships",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HireConfirmed",
                table: "UserInternships",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HireRequested",
                table: "UserInternships",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HireApproved",
                table: "UserInternships");

            migrationBuilder.DropColumn(
                name: "HireConfirmed",
                table: "UserInternships");

            migrationBuilder.DropColumn(
                name: "HireRequested",
                table: "UserInternships");
        }
    }
}
