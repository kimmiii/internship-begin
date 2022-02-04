using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace StagebeheerAPI.Models
{
    [ExcludeFromCodeCoverage]
    public class StagebeheerDBContext : DbContext
    {
        public StagebeheerDBContext()
        {
        }

        public StagebeheerDBContext(DbContextOptions<StagebeheerDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<Country> Country { get; set; }

        public virtual DbSet<Role> Role { get; set; }


        public virtual DbSet<Internship> Internship { get; set; }
        public virtual DbSet<InternshipPeriod> InternshipPeriod { get; set; }
        public virtual DbSet<Period> Period { get; set; }

        public virtual DbSet<User> User { get; set; }

        public virtual DbSet<InternshipSpecialisation> InternshipSpecialisation { get; set; }
        public virtual DbSet<Specialisation> Specialisation { get; set; }

        public virtual DbSet<InternshipEnvironment> InternshipEnvironment { get; set; }
        public virtual DbSet<Environment> Environment { get; set; }

        public virtual DbSet<InternshipExpectation> InternshipExpectation { get; set; }
        public virtual DbSet<Expectation> Expectation { get; set; }
        public virtual DbSet<InternshipAssignedUser> InternshipAssignedUser { get; set; }
        public virtual DbSet<InternshipReviewer> InternshipReviewer { get; set; }
        public virtual DbSet<UserFavourites> UserFavourites { get; set; }
        public virtual DbSet<UserInternships> UserInternships { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Internship
            modelBuilder.Entity<Internship>(entity =>
            {
                entity.Property(e => e.AssignmentDescription).IsUnicode(false);
                entity.Property(e => e.Conditions).IsUnicode(false);
                entity.Property(e => e.ContactStudentName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.SentToReviewersAt).HasColumnType("datetime");

                entity.Property(e => e.CountTotalAssignedReviewers)
                   .HasColumnName("CountTotalAssignedReviewers")
                   .HasMaxLength(50)
                   .IsUnicode(false);

                entity.Property(e => e.InternshipEnvironmentOthers)
                    .HasMaxLength(1500)
                    .IsUnicode(false);

                entity.Property(e => e.Remark).IsUnicode(false);

                entity.Property(e => e.TechnicalDetails).IsUnicode(false);

                entity.Property(e => e.WpBusNr)
                    .HasColumnName("WP_BusNr")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WpCity)
                    .HasColumnName("WP_City")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WpHouseNr)
                    .HasColumnName("WP_HouseNr")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WpStreet)
                    .HasColumnName("WP_Street")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WpZipCode)
                    .HasColumnName("WP_ZipCode")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AcademicYear)
                    .HasColumnName("AcademicYear")
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.ExternalFeedback)
                    .HasColumnName("ExternalFeedback")
                    .HasColumnType("nvarchar(max)")
                    .IsUnicode(false);

                entity.Property(e => e.InternalFeedback)
                    .HasColumnName("InternalFeedback")
                    .HasColumnType("nvarchar(max)")
                    .IsUnicode(false);

                entity.Property(e => e.ResearchTopicTitle)
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.ResearchTopicDescription)
                    .HasColumnType("nvarchar(max)")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Internship>()
                .HasOne(i => i.ProjectStatus)
                .WithMany(ps => ps.Internships);

            modelBuilder.Entity<InternshipPeriod>().HasKey(sc => new { sc.InternshipId, sc.PeriodId });

            modelBuilder.Entity<InternshipPeriod>()
                .HasOne<Internship>(sc => sc.Internship)
                .WithMany(s => s.InternshipPeriod)
                .HasForeignKey(sc => sc.InternshipId);


            modelBuilder.Entity<InternshipPeriod>()
                .HasOne<Period>(sc => sc.Period)
                .WithMany(s => s.InternshipPeriod)
                .HasForeignKey(sc => sc.PeriodId);


            modelBuilder.Entity<InternshipSpecialisation>().HasKey(sc => new { sc.InternshipId, sc.SpecialisationId });

            modelBuilder.Entity<InternshipSpecialisation>()
                .HasOne<Internship>(sc => sc.Internship)
                .WithMany(s => s.InternshipSpecialisation)
                .HasForeignKey(sc => sc.InternshipId);


            modelBuilder.Entity<InternshipSpecialisation>()
                .HasOne<Specialisation>(sc => sc.Specialisation)
                .WithMany(s => s.InternshipSpecialisation)
                .HasForeignKey(sc => sc.SpecialisationId);


            modelBuilder.Entity<InternshipEnvironment>().HasKey(sc => new { sc.InternshipId, sc.EnvironmentId });

            modelBuilder.Entity<InternshipEnvironment>()
                .HasOne<Internship>(sc => sc.Internship)
                .WithMany(s => s.InternshipEnvironment)
                .HasForeignKey(sc => sc.InternshipId);


            modelBuilder.Entity<InternshipEnvironment>()
                .HasOne<Environment>(sc => sc.Environment)
                .WithMany(s => s.InternshipEnvironment)
                .HasForeignKey(sc => sc.EnvironmentId);


            modelBuilder.Entity<InternshipExpectation>().HasKey(sc => new { sc.InternshipId, sc.ExpectationId });

            modelBuilder.Entity<InternshipExpectation>()
                .HasOne<Internship>(sc => sc.Internship)
                .WithMany(s => s.InternshipExpectation)
                .HasForeignKey(sc => sc.InternshipId);


            modelBuilder.Entity<InternshipExpectation>()
                .HasOne<Expectation>(sc => sc.Expectation)
                .WithMany(s => s.InternshipExpectation)
                .HasForeignKey(sc => sc.ExpectationId);


            modelBuilder.Entity<InternshipAssignedUser>().HasKey(sc => new { sc.InternshipId, sc.UserId });

            modelBuilder.Entity<InternshipAssignedUser>()
                .HasOne<Internship>(sc => sc.Internship)
                .WithMany(s => s.InternshipAssignedUser)
                .HasForeignKey(sc => sc.InternshipId);

            modelBuilder.Entity<InternshipAssignedUser>()
                .HasOne<User>(sc => sc.User)
                .WithMany(s => s.InternshipAssignedUser)
                .HasForeignKey(sc => sc.UserId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InternshipReviewer>().HasKey(sc => new { sc.InternshipId, sc.UserId });

            modelBuilder.Entity<InternshipReviewer>()
                .HasOne<Internship>(sc => sc.Internship)
                .WithMany(s => s.InternshipReviewer)
                .HasForeignKey(sc => sc.InternshipId);

            modelBuilder.Entity<InternshipReviewer>()
                .HasOne<User>(sc => sc.User)
                .WithMany(s => s.InternshipReviewer)
                .HasForeignKey(sc => sc.UserId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserFavourites>().HasKey(sc => new { sc.InternshipId, sc.UserId });

            modelBuilder.Entity<UserFavourites>()
                .HasOne<Internship>(sc => sc.Internship)
                .WithMany(s => s.UserFavourites)
                .HasForeignKey(sc => sc.InternshipId);

            modelBuilder.Entity<UserFavourites>()
                .HasOne<User>(sc => sc.User)
                .WithMany(s => s.UserFavourites)
                .HasForeignKey(sc => sc.UserId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserInternships>().HasKey(sc => new { sc.InternshipId, sc.UserId });

            modelBuilder.Entity<UserInternships>()
                .HasOne<Internship>(sc => sc.Internship)
                .WithMany(s => s.UserInternships)
                .HasForeignKey(sc => sc.InternshipId);

            modelBuilder.Entity<UserInternships>()
                .HasOne<User>(sc => sc.User)
                .WithMany(s => s.UserInternships)
                .HasForeignKey(sc => sc.UserId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserInternships>(entity =>
            {
                entity.Property(e => e.RejectionFeedback)
                       .HasColumnName("RejectionFeedback")
                       .HasColumnType("nvarchar(max)")
                       .IsUnicode(false);
            });

            // Key Relations between Entities
            // User - Role
            modelBuilder.Entity<User>()
                .HasOne(i => i.Role)
                .WithMany(c => c.Users);


            // User - Company
            modelBuilder.Entity<User>()
                .HasOne(user => user.Company)
                .WithOne(company => company.User);


            // Company - Contact
            modelBuilder.Entity<Company>()
                .HasMany(c => c.Contacts)
                .WithOne(e => e.Company);


            // Company - Internship
            modelBuilder.Entity<Company>()
                .HasMany(c => c.Internships)
                .WithOne(i => i.Company);
        }
    }
}
