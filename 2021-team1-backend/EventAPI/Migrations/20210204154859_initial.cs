using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventAPI.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "AcademicYears",
                table => new
                {
                    Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>("int", maxLength: 4, nullable: false),
                    EndYear = table.Column<int>("int", maxLength: 4, nullable: false,
                        computedColumnSql: "[StartYear] + 1"),
                    Description = table.Column<string>("nvarchar(max)", nullable: true,
                        computedColumnSql:
                        "CAST([StartYear] as varchar(200)) + '-' + CAST([StartYear] + 1 as varchar(200))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicYears", x => x.Id);
                    table.UniqueConstraint("AK_AcademicYears_StartYear", x => x.StartYear);
                });

            migrationBuilder.CreateTable(
                "Events",
                table => new
                {
                    Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                    Name = table.Column<string>("nvarchar(max)", nullable: false),
                    DateEvent = table.Column<DateTime>("datetime2", nullable: false),
                    StartHour = table.Column<TimeSpan>("time", nullable: false),
                    EndHour = table.Column<TimeSpan>("time", nullable: false),
                    Location = table.Column<int>("int", nullable: false),
                    IsActivated = table.Column<bool>("bit", nullable: false, defaultValue: false),
                    AcademicYearId = table.Column<Guid>("uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        "FK_Events_AcademicYears_AcademicYearId",
                        x => x.AcademicYearId,
                        "AcademicYears",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "EventCompanies",
                table => new
                {
                    Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>("uniqueidentifier", nullable: false),
                    CompanyId = table.Column<int>("int", nullable: false),
                    Website = table.Column<string>("nvarchar(max)", nullable: true),
                    CompanyDescription = table.Column<string>("nvarchar(max)", nullable: true),
                    ArrivalTime = table.Column<TimeSpan>("time", nullable: false),
                    DepartureTime = table.Column<TimeSpan>("time", nullable: false),
                    TimeSlot = table.Column<int>("int", nullable: false),
                    CreateAppointmentUntil = table.Column<int>("int", nullable: false),
                    CancelAppointmentUntil = table.Column<int>("int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCompanies", x => x.Id);
                    table.UniqueConstraint("AK_EventCompanies_EventId_CompanyId", x => new {x.EventId, x.CompanyId});
                    table.ForeignKey(
                        "FK_EventCompanies_Events_EventId",
                        x => x.EventId,
                        "Events",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Attendees",
                table => new
                {
                    Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                    EventCompanyId = table.Column<Guid>("uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>("nvarchar(max)", nullable: true),
                    LastName = table.Column<string>("nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendees", x => x.Id);
                    table.ForeignKey(
                        "FK_Attendees_EventCompanies_EventCompanyId",
                        x => x.EventCompanyId,
                        "EventCompanies",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Appointments",
                table => new
                {
                    Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>("uniqueidentifier", nullable: false),
                    CompanyId = table.Column<int>("int", nullable: false),
                    AttendeeId = table.Column<Guid>("uniqueidentifier", nullable: false),
                    StudentId = table.Column<int>("int", nullable: false),
                    InternshipId = table.Column<int>("int", nullable: false),
                    BeginHour = table.Column<TimeSpan>("time", nullable: false),
                    EndHour = table.Column<TimeSpan>("time", nullable: false),
                    Comment = table.Column<string>("nvarchar(max)", nullable: true),
                    Confirmed = table.Column<bool>("bit", nullable: false, defaultValue: false),
                    Disabled = table.Column<bool>("bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.UniqueConstraint("AK_Appointments_EventId_CompanyId_AttendeeId_BeginHour",
                        x => new {x.EventId, x.CompanyId, x.AttendeeId, x.BeginHour});
                    table.ForeignKey(
                        "FK_Appointments_Attendees_AttendeeId",
                        x => x.AttendeeId,
                        "Attendees",
                        "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        "FK_Appointments_Events_EventId",
                        x => x.EventId,
                        "Events",
                        "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                "IX_Appointments_AttendeeId",
                "Appointments",
                "AttendeeId");

            migrationBuilder.CreateIndex(
                "IX_Attendees_EventCompanyId",
                "Attendees",
                "EventCompanyId");

            migrationBuilder.CreateIndex(
                "IX_Events_AcademicYearId",
                "Events",
                "AcademicYearId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Appointments");

            migrationBuilder.DropTable(
                "Attendees");

            migrationBuilder.DropTable(
                "EventCompanies");

            migrationBuilder.DropTable(
                "Events");

            migrationBuilder.DropTable(
                "AcademicYears");
        }
    }
}