using Microsoft.EntityFrameworkCore.Migrations;

namespace StagebeheerAPI.Migrations
{
    public partial class AddInternshipAssignedUserRemoveRestriction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternshipAssignedUser_Internship_InternshipId",
                table: "InternshipAssignedUser");

            migrationBuilder.AddForeignKey(
                name: "FK_InternshipAssignedUser_Internship_InternshipId",
                table: "InternshipAssignedUser",
                column: "InternshipId",
                principalTable: "Internship",
                principalColumn: "InternshipId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternshipAssignedUser_Internship_InternshipId",
                table: "InternshipAssignedUser");

            migrationBuilder.AddForeignKey(
                name: "FK_InternshipAssignedUser_Internship_InternshipId",
                table: "InternshipAssignedUser",
                column: "InternshipId",
                principalTable: "Internship",
                principalColumn: "InternshipId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
