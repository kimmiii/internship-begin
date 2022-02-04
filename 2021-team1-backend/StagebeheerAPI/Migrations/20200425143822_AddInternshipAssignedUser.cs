using Microsoft.EntityFrameworkCore.Migrations;

namespace StagebeheerAPI.Migrations
{
    public partial class AddInternshipAssignedUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Internship_User_AssignedToUserId",
                table: "Internship");

            migrationBuilder.DropIndex(
                name: "IX_Internship_AssignedToUserId",
                table: "Internship");

            migrationBuilder.DropColumn(
                name: "AssignedToUserId",
                table: "Internship");

            migrationBuilder.CreateTable(
                name: "InternshipAssignedUser",
                columns: table => new
                {
                    InternshipId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternshipAssignedUser", x => new { x.InternshipId, x.UserId });
                    table.ForeignKey(
                        name: "FK_InternshipAssignedUser_Internship_InternshipId",
                        column: x => x.InternshipId,
                        principalTable: "Internship",
                        principalColumn: "InternshipId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InternshipAssignedUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InternshipAssignedUser_UserId",
                table: "InternshipAssignedUser",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InternshipAssignedUser");

            migrationBuilder.AddColumn<int>(
                name: "AssignedToUserId",
                table: "Internship",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Internship_AssignedToUserId",
                table: "Internship",
                column: "AssignedToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Internship_User_AssignedToUserId",
                table: "Internship",
                column: "AssignedToUserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
