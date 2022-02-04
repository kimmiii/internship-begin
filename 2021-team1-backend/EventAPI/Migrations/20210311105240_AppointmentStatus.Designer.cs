﻿// <auto-generated />
using System;
using EventAPI.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EventAPI.Migrations
{
    [DbContext(typeof(EventDBContext))]
    [Migration("20210311105240_AppointmentStatus")]
    partial class AppointmentStatus
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("EventAPI.Domain.Models.AcademicYear", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("nvarchar(max)")
                        .HasComputedColumnSql("CAST([StartYear] as varchar(200)) + '-' + CAST([StartYear] + 1 as varchar(200))");

                    b.Property<int>("EndYear")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasMaxLength(4)
                        .HasColumnType("int")
                        .HasComputedColumnSql("[StartYear] + 1");

                    b.Property<int>("StartYear")
                        .HasMaxLength(4)
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasAlternateKey("StartYear");

                    b.ToTable("AcademicYears");
                });

            modelBuilder.Entity("EventAPI.Domain.Models.Appointment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AppointmentStatus")
                        .HasColumnType("int");

                    b.Property<Guid>("AttendeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<TimeSpan>("BeginHour")
                        .HasColumnType("time");

                    b.Property<string>("CancelMotivation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<bool>("Disabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<TimeSpan>("EndHour")
                        .HasColumnType("time");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("InternshipId")
                        .HasColumnType("int");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasAlternateKey("EventId", "CompanyId", "AttendeeId", "BeginHour");

                    b.HasIndex("AttendeeId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("EventAPI.Domain.Models.Attendee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EventCompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EventCompanyId");

                    b.ToTable("Attendees");
                });

            modelBuilder.Entity("EventAPI.Domain.Models.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AcademicYearId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateEvent")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan>("EndHour")
                        .HasColumnType("time");

                    b.Property<bool>("IsActivated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("Location")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("StartHour")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.HasIndex("AcademicYearId")
                        .IsUnique();

                    b.ToTable("Events");
                });

            modelBuilder.Entity("EventAPI.Domain.Models.EventCompany", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<TimeSpan>("ArrivalTime")
                        .HasColumnType("time");

                    b.Property<int>("CancelAppointmentUntil")
                        .HasColumnType("int");

                    b.Property<string>("CompanyDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int>("CreateAppointmentUntil")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("DepartureTime")
                        .HasColumnType("time");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TimeSlot")
                        .HasColumnType("int");

                    b.Property<string>("Website")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasAlternateKey("EventId", "CompanyId");

                    b.ToTable("EventCompanies");
                });

            modelBuilder.Entity("EventAPI.Domain.Models.Appointment", b =>
                {
                    b.HasOne("EventAPI.Domain.Models.Attendee", null)
                        .WithMany("Appointments")
                        .HasForeignKey("AttendeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EventAPI.Domain.Models.Event", null)
                        .WithMany("Appointments")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EventAPI.Domain.Models.Attendee", b =>
                {
                    b.HasOne("EventAPI.Domain.Models.EventCompany", null)
                        .WithMany("Attendees")
                        .HasForeignKey("EventCompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EventAPI.Domain.Models.Event", b =>
                {
                    b.HasOne("EventAPI.Domain.Models.AcademicYear", null)
                        .WithOne("Event")
                        .HasForeignKey("EventAPI.Domain.Models.Event", "AcademicYearId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EventAPI.Domain.Models.EventCompany", b =>
                {
                    b.HasOne("EventAPI.Domain.Models.Event", null)
                        .WithMany("EventCompanies")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EventAPI.Domain.Models.AcademicYear", b =>
                {
                    b.Navigation("Event");
                });

            modelBuilder.Entity("EventAPI.Domain.Models.Attendee", b =>
                {
                    b.Navigation("Appointments");
                });

            modelBuilder.Entity("EventAPI.Domain.Models.Event", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("EventCompanies");
                });

            modelBuilder.Entity("EventAPI.Domain.Models.EventCompany", b =>
                {
                    b.Navigation("Attendees");
                });
#pragma warning restore 612, 618
        }
    }
}
