using Microsoft.EntityFrameworkCore.Migrations;

namespace StagebeheerAPI.Migrations
{
    public partial class InternshipPromotorFieldsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactPromotorId",
                table: "Internship");

            migrationBuilder.AddColumn<string>(
                name: "PromotorEmail",
                table: "Internship",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PromotorFirstname",
                table: "Internship",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PromotorFunction",
                table: "Internship",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PromotorSurname",
                table: "Internship",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PromotorEmail",
                table: "Internship");

            migrationBuilder.DropColumn(
                name: "PromotorFirstname",
                table: "Internship");

            migrationBuilder.DropColumn(
                name: "PromotorFunction",
                table: "Internship");

            migrationBuilder.DropColumn(
                name: "PromotorSurname",
                table: "Internship");

            migrationBuilder.AddColumn<int>(
                name: "ContactPromotorId",
                table: "Internship",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
