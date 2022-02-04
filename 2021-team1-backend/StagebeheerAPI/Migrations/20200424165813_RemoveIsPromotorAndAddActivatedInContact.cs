using Microsoft.EntityFrameworkCore.Migrations;

namespace StagebeheerAPI.Migrations
{
    public partial class RemoveIsPromotorAndAddActivatedInContact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Internship_Contact_ContactId",
                table: "Internship");

            migrationBuilder.DropIndex(
                name: "IX_Internship_ContactId",
                table: "Internship");

            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "Internship");

            migrationBuilder.DropColumn(
                name: "IsPromotor",
                table: "Contact");

            migrationBuilder.AddColumn<bool>(
                name: "Activated",
                table: "Contact",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activated",
                table: "Contact");

            migrationBuilder.AddColumn<int>(
                name: "ContactId",
                table: "Internship",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPromotor",
                table: "Contact",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Internship_ContactId",
                table: "Internship",
                column: "ContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_Internship_Contact_ContactId",
                table: "Internship",
                column: "ContactId",
                principalTable: "Contact",
                principalColumn: "ContactId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
