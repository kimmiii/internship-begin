﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StagebeheerAPI.Models;

namespace StagebeheerAPI.Migrations
{
    [DbContext(typeof(StagebeheerDBContext))]
    [Migration("20200402072911_correctFieldsAccordingToLatestSchema")]
    partial class correctFieldsAccordingToLatestSchema
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("StagebeheerAPI.Models.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Activated")
                        .HasColumnType("bit");

                    b.Property<string>("BusNr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HouseNr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalEmployees")
                        .HasColumnType("int");

                    b.Property<int>("TotalITEmployees")
                        .HasColumnType("int");

                    b.Property<int>("TotalITEmployeesActive")
                        .HasColumnType("int");

                    b.Property<string>("VATNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ZipCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CompanyId");

                    b.ToTable("Company");
                });

            modelBuilder.Entity("StagebeheerAPI.Models.Contact", b =>
                {
                    b.Property<int>("ContactId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Firstname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Function")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsPromotor")
                        .HasColumnType("bit");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ContactId");

                    b.ToTable("Contact");
                });

            modelBuilder.Entity("StagebeheerAPI.Models.Country", b =>
                {
                    b.Property<int>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CountryId");

                    b.ToTable("Country");
                });

            modelBuilder.Entity("StagebeheerAPI.Models.Environment", b =>
                {
                    b.Property<int>("EnvironmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EnvironmentId");

                    b.ToTable("Environment");
                });

            modelBuilder.Entity("StagebeheerAPI.Models.Expectation", b =>
                {
                    b.Property<int>("ExpectationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ExpectationId");

                    b.ToTable("Expectation");
                });

            modelBuilder.Entity("StagebeheerAPI.Models.Internship", b =>
                {
                    b.Property<int>("InternshipId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AssignedToUserId")
                        .HasColumnType("int");

                    b.Property<string>("AssignmentDescription")
                        .HasColumnType("varchar(max)")
                        .IsUnicode(false);

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("Conditions")
                        .HasColumnType("varchar(max)")
                        .IsUnicode(false);

                    b.Property<int>("ContactId")
                        .HasColumnType("int");

                    b.Property<string>("ContactStudentName")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime");

                    b.Property<string>("ExternalFeedback")
                        .HasColumnName("ExternalFeedback")
                        .HasColumnType("nvarchar(max)")
                        .IsUnicode(false);

                    b.Property<string>("InternalFeedback")
                        .HasColumnName("InternalFeedback")
                        .HasColumnType("nvarchar(max)")
                        .IsUnicode(false);

                    b.Property<string>("InternshipEnvironmentOthers")
                        .HasColumnType("varchar(500)")
                        .HasMaxLength(500)
                        .IsUnicode(false);

                    b.Property<int>("ProjectStatusId")
                        .HasColumnType("int");

                    b.Property<string>("Remark")
                        .HasColumnType("varchar(max)")
                        .IsUnicode(false);

                    b.Property<string>("TechnicalDetails")
                        .HasColumnType("varchar(max)")
                        .IsUnicode(false);

                    b.Property<int?>("TotalInternsRequired")
                        .HasColumnType("int");

                    b.Property<string>("WpBusNr")
                        .HasColumnName("WP_BusNr")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("WpCity")
                        .HasColumnName("WP_City")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("WpCountry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WpHouseNr")
                        .HasColumnName("WP_HouseNr")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("WpStreet")
                        .HasColumnName("WP_Street")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("WpZipCode")
                        .HasColumnName("WP_ZipCode")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("InternshipId");

                    b.HasIndex("AssignedToUserId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("ContactId");

                    b.HasIndex("ProjectStatusId");

                    b.ToTable("Internship");
                });

            modelBuilder.Entity("StagebeheerAPI.Models.InternshipEnvironment", b =>
                {
                    b.Property<int>("InternshipId")
                        .HasColumnType("int");

                    b.Property<int>("EnvironmentId")
                        .HasColumnType("int");

                    b.HasKey("InternshipId", "EnvironmentId");

                    b.HasIndex("EnvironmentId");

                    b.ToTable("InternshipEnvironment");
                });

            modelBuilder.Entity("StagebeheerAPI.Models.InternshipExpectation", b =>
                {
                    b.Property<int>("InternshipId")
                        .HasColumnType("int");

                    b.Property<int>("ExpectationId")
                        .HasColumnType("int");

                    b.HasKey("InternshipId", "ExpectationId");

                    b.HasIndex("ExpectationId");

                    b.ToTable("InternshipExpectation");
                });

            modelBuilder.Entity("StagebeheerAPI.Models.InternshipPeriod", b =>
                {
                    b.Property<int>("InternshipId")
                        .HasColumnType("int");

                    b.Property<int>("PeriodId")
                        .HasColumnType("int");

                    b.HasKey("InternshipId", "PeriodId");

                    b.HasIndex("PeriodId");

                    b.ToTable("InternshipPeriod");
                });

            modelBuilder.Entity("StagebeheerAPI.Models.InternshipSpecialisation", b =>
                {
                    b.Property<int>("InternshipId")
                        .HasColumnType("int");

                    b.Property<int>("SpecialisationId")
                        .HasColumnType("int");

                    b.HasKey("InternshipId", "SpecialisationId");

                    b.HasIndex("SpecialisationId");

                    b.ToTable("InternshipSpecialisation");
                });

            modelBuilder.Entity("StagebeheerAPI.Models.Period", b =>
                {
                    b.Property<int>("PeriodId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PeriodId");

                    b.ToTable("Period");
                });

            modelBuilder.Entity("StagebeheerAPI.Models.ProjectStatus", b =>
                {
                    b.Property<int>("ProjectStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProjectStatusId");

                    b.ToTable("ProjectStatus");
                });

            modelBuilder.Entity("StagebeheerAPI.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("StagebeheerAPI.Models.Specialisation", b =>
                {
                    b.Property<int>("SpecialisationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SpecialisationId");

                    b.ToTable("Specialisation");
                });

            modelBuilder.Entity("StagebeheerAPI.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Activated")
                        .HasColumnType("bit");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("Salt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserEmailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserPass")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.HasIndex("RoleId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("StagebeheerAPI.Models.Internship", b =>
                {
                    b.HasOne("StagebeheerAPI.Models.User", "AssignedTo")
                        .WithMany("Internships")
                        .HasForeignKey("AssignedToUserId");

                    b.HasOne("StagebeheerAPI.Models.Company", "Company")
                        .WithMany("Internships")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StagebeheerAPI.Models.Contact", "Contact")
                        .WithMany("Internships")
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StagebeheerAPI.Models.ProjectStatus", "ProjectStatus")
                        .WithMany("Internships")
                        .HasForeignKey("ProjectStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StagebeheerAPI.Models.InternshipEnvironment", b =>
                {
                    b.HasOne("StagebeheerAPI.Models.Environment", "Environment")
                        .WithMany("InternshipEnvironment")
                        .HasForeignKey("EnvironmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StagebeheerAPI.Models.Internship", "Internship")
                        .WithMany("InternshipEnvironment")
                        .HasForeignKey("InternshipId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StagebeheerAPI.Models.InternshipExpectation", b =>
                {
                    b.HasOne("StagebeheerAPI.Models.Expectation", "Expectation")
                        .WithMany("InternshipExpectation")
                        .HasForeignKey("ExpectationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StagebeheerAPI.Models.Internship", "Internship")
                        .WithMany("InternshipExpectation")
                        .HasForeignKey("InternshipId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StagebeheerAPI.Models.InternshipPeriod", b =>
                {
                    b.HasOne("StagebeheerAPI.Models.Internship", "Internship")
                        .WithMany("InternshipPeriod")
                        .HasForeignKey("InternshipId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StagebeheerAPI.Models.Period", "Period")
                        .WithMany("InternshipPeriod")
                        .HasForeignKey("PeriodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StagebeheerAPI.Models.InternshipSpecialisation", b =>
                {
                    b.HasOne("StagebeheerAPI.Models.Internship", "Internship")
                        .WithMany("InternshipSpecialisation")
                        .HasForeignKey("InternshipId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StagebeheerAPI.Models.Specialisation", "Specialisation")
                        .WithMany("InternshipSpecialisation")
                        .HasForeignKey("SpecialisationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StagebeheerAPI.Models.User", b =>
                {
                    b.HasOne("StagebeheerAPI.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId");
                });
#pragma warning restore 612, 618
        }
    }
}