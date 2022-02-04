using Microsoft.EntityFrameworkCore.Migrations;

namespace StagebeheerAPI.Migrations
{
    public partial class correctFieldsAccordingToLatestSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Internship_Company_CompanyId",
                table: "Internship");

            migrationBuilder.DropForeignKey(
                name: "FK_Internship_Contact_ContactId",
                table: "Internship");

            migrationBuilder.DropForeignKey(
                name: "FK_Internship_ProjectStatus_ProjectStatusId",
                table: "Internship");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "User");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectStatusId",
                table: "Internship",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ContactId",
                table: "Internship",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Internship",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalFeedback",
                table: "Internship",
                type: "nvarchar(max)",
                unicode: false,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternalFeedback",
                table: "Internship",
                type: "nvarchar(max)",
                unicode: false,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Internship_Company_CompanyId",
                table: "Internship",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Internship_Contact_ContactId",
                table: "Internship",
                column: "ContactId",
                principalTable: "Contact",
                principalColumn: "ContactId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Internship_ProjectStatus_ProjectStatusId",
                table: "Internship",
                column: "ProjectStatusId",
                principalTable: "ProjectStatus",
                principalColumn: "ProjectStatusId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Internship_Company_CompanyId",
                table: "Internship");

            migrationBuilder.DropForeignKey(
                name: "FK_Internship_Contact_ContactId",
                table: "Internship");

            migrationBuilder.DropForeignKey(
                name: "FK_Internship_ProjectStatus_ProjectStatusId",
                table: "Internship");

            migrationBuilder.DropColumn(
                name: "ExternalFeedback",
                table: "Internship");

            migrationBuilder.DropColumn(
                name: "InternalFeedback",
                table: "Internship");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProjectStatusId",
                table: "Internship",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ContactId",
                table: "Internship",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Internship",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Internship_Company_CompanyId",
                table: "Internship",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Internship_Contact_ContactId",
                table: "Internship",
                column: "ContactId",
                principalTable: "Contact",
                principalColumn: "ContactId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Internship_ProjectStatus_ProjectStatusId",
                table: "Internship",
                column: "ProjectStatusId",
                principalTable: "ProjectStatus",
                principalColumn: "ProjectStatusId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
