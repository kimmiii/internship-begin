using Microsoft.EntityFrameworkCore.Migrations;

namespace StagebeheerAPI.Migrations
{
    public partial class changedFieldnameUserInternships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InternalFeedback",
                table: "UserInternships",
                newName: "RejectionFeedback");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RejectionFeedback",
                table: "UserInternships",
                newName: "InternalFeedback");
        }
    }
}
