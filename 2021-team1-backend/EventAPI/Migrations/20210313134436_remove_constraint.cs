using Microsoft.EntityFrameworkCore.Migrations;

namespace EventAPI.Migrations
{
    public partial class remove_constraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Appointments_EventId_CompanyId_AttendeeId_BeginHour",
                table: "Appointments");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_EventId",
                table: "Appointments",
                column: "EventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_EventId",
                table: "Appointments");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Appointments_EventId_CompanyId_AttendeeId_BeginHour",
                table: "Appointments",
                columns: new[] { "EventId", "CompanyId", "AttendeeId", "BeginHour" });
        }
    }
}
