using EventAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EventAPI.DAL
{
    public class EventDBContext : DbContext
    {
        public EventDBContext()
        {
        }

        public EventDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AcademicYear> AcademicYears { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventCompany> EventCompanies { get; set; }
        public DbSet<Attendee> Attendees { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AcademicYear>()
                .HasKey(a => a.Id);
            modelBuilder.Entity<AcademicYear>()
                .HasAlternateKey(a => new {a.StartYear});
            modelBuilder.Entity<AcademicYear>()
                .Property(a => a.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<AcademicYear>()
                .Property(a => a.EndYear)
                .HasComputedColumnSql("[StartYear] + 1");
            modelBuilder.Entity<AcademicYear>()
                .Property(a => a.Description)
                .HasComputedColumnSql(
                    "CAST([StartYear] as varchar(200)) + '-' + CAST([StartYear] + 1 as varchar(200))");
            modelBuilder.Entity<AcademicYear>()
                .Ignore(a => a.Event);
            modelBuilder.Entity<AcademicYear>()
                .Property(a => a.StartYear)
                .HasMaxLength(4)
                .IsRequired();
            modelBuilder.Entity<AcademicYear>()
                .Property(a => a.EndYear)
                .HasMaxLength(4)
                .IsRequired();
            modelBuilder.Entity<AcademicYear>()
                .HasOne(e => e.Event)
                .WithOne(a => a.AcademicYear)
                .HasForeignKey<Event>(a => a.AcademicYearId)
                .OnDelete(DeleteBehavior.NoAction);

            var converter = new EnumToNumberConverter<EventLocation, int>();

            modelBuilder.Entity<Event>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<Event>()
                .Property(e => e.AcademicYearId)
                .IsRequired();
            modelBuilder.Entity<Event>()
                .Property(e => e.Name)
                .IsRequired();
            modelBuilder.Entity<Event>()
                .Property(e => e.DateEvent)
                .IsRequired();
            modelBuilder.Entity<Event>()
                .Ignore(e => e.AcademicYear);
            modelBuilder.Entity<Event>()
                .Property(e => e.Location)
                .HasConversion(converter);
            modelBuilder.Entity<Event>()
                .Property(e => e.Location)
                .IsRequired();
            modelBuilder.Entity<Event>()
                .Property(e => e.IsActivated)
                .HasDefaultValue(false);


            modelBuilder.Entity<EventCompany>()
                .HasKey(ec => ec.Id);
            modelBuilder.Entity<EventCompany>()
                .HasAlternateKey(ec => new {ec.EventId, ec.CompanyId});
            modelBuilder.Entity<EventCompany>()
                .Property(ec => ec.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<EventCompany>()
                .HasOne(ec => ec.Event)
                .WithMany(e => e.EventCompanies)
                .HasForeignKey(ec => ec.EventId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<EventCompany>()
                .Ignore(ec => ec.Event);
            modelBuilder.Entity<EventCompany>()
                .Ignore(ec => ec.Company);

            modelBuilder.Entity<Attendee>()
                .HasOne(ec => ec.EventCompany)
                .WithMany(a => a.Attendees)
                .HasForeignKey(ec => ec.EventCompanyId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Attendee>()
                .Ignore(a => a.EventCompany);

            var converterAppointmentStatus = new EnumToNumberConverter<AppointmentStatus, int>();

            modelBuilder.Entity<Appointment>()
                .HasKey(a => a.Id);
            //modelBuilder.Entity<Appointment>()
            //    .HasAlternateKey(a => new {a.EventId, a.CompanyId, a.AttendeeId, a.BeginHour});
            modelBuilder.Entity<Appointment>()
                .Property(ec => ec.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Appointment>()
                .Property(ec => ec.EventId)
                .IsRequired();
            modelBuilder.Entity<Appointment>()
                .Property(ec => ec.CompanyId)
                .IsRequired();
            modelBuilder.Entity<Appointment>()
                .Property(ec => ec.AttendeeId)
                .IsRequired();
            modelBuilder.Entity<Appointment>()
                .Property(ec => ec.BeginHour)
                .IsRequired();
            modelBuilder.Entity<Appointment>()
                .Property(ec => ec.EndHour)
                .IsRequired();
            modelBuilder.Entity<Appointment>()
                .Property(a => a.AppointmentStatus)
                .HasConversion(converterAppointmentStatus);
            modelBuilder.Entity<Appointment>()
                .Property(ec => ec.Disabled)
                .HasDefaultValue(false);
            modelBuilder.Entity<Appointment>()
                .HasOne(ec => ec.Event)
                .WithMany(e => e.Appointments)
                .HasForeignKey(ec => ec.EventId)
                .OnDelete(DeleteBehavior.ClientNoAction);
            modelBuilder.Entity<Appointment>()
                .HasOne(ec => ec.Attendee)
                .WithMany(e => e.Appointments)
                .HasForeignKey(ec => ec.AttendeeId)
                .OnDelete(DeleteBehavior.ClientNoAction);
            modelBuilder.Entity<Appointment>()
                .Ignore(a => a.Event);
            modelBuilder.Entity<Appointment>()
                .Ignore(a => a.Company);
            modelBuilder.Entity<Appointment>()
                .Ignore(a => a.Attendee);
            modelBuilder.Entity<Appointment>()
                .Ignore(a => a.Internship);

            base.OnModelCreating(modelBuilder);
        }
    }
}