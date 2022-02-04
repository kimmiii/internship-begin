using System;
using System.Collections.Generic;

namespace StagebeheerAPI.Models
{
    public partial class Internship
    {
        public Internship()
        {
            InternshipPeriod = new HashSet<InternshipPeriod>();
            InternshipSpecialisation = new HashSet<InternshipSpecialisation>();
            InternshipEnvironment = new HashSet<InternshipEnvironment>();
            InternshipExpectation = new HashSet<InternshipExpectation>();
            InternshipAssignedUser = new HashSet<InternshipAssignedUser>();
            InternshipReviewer = new HashSet<InternshipReviewer>();
        }

        public int InternshipId { get; set; }

        public Company Company { get; set; }
        public int CompanyId { get; set; }

        public ProjectStatus ProjectStatus { get; set; }
        public int ProjectStatusId { get; set; }
        //public User AssignedTo { get; set; }

        // Internship location
        public string WpStreet { get; set; }
        public string WpHouseNr { get; set; }
        public string WpBusNr { get; set; }
        public string WpZipCode { get; set; }
        public string WpCity { get; set; }
        public string WpCountry { get; set; }

        // Internship contact
        public int ContactPersonId { get; set; }

        // Internship Promotor
        public string PromotorFirstname { get; set; }
        public string PromotorSurname { get; set; }
        public string PromotorFunction { get; set; }
        public string PromotorEmail { get; set; }

        // Internship topic
        public string InternshipEnvironmentOthers { get; set; }
        public string AssignmentDescription { get; set; }
        public string TechnicalDetails { get; set; }
        public string Conditions { get; set; }
        public int? TotalInternsRequired { get; set; }
        public string ContactStudentName { get; set; }
        public string Remark { get; set; }
        public string ResearchTopicTitle { get; set; }
        public string ResearchTopicDescription { get; set; }
        public string AcademicYear { get; set; }

        public String ExternalFeedback { get; set; }
        public String InternalFeedback { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public int CountTotalAssignedReviewers { get; set; } = 0;
        public DateTime? SentToReviewersAt { get; set; }
        public bool Completed { get; set; }
        public bool ShowInEvent { get; set; }


        public virtual ICollection<InternshipPeriod> InternshipPeriod { get; set; }
        public virtual ICollection<InternshipSpecialisation> InternshipSpecialisation { get; set; }
        public virtual ICollection<InternshipEnvironment> InternshipEnvironment { get; set; }
        public virtual ICollection<InternshipExpectation> InternshipExpectation { get; set; }
        public virtual ICollection<InternshipAssignedUser> InternshipAssignedUser { get; set; }
        public virtual ICollection<InternshipReviewer> InternshipReviewer { get; set; }
        public virtual ICollection<UserFavourites> UserFavourites { get; set; }
        public virtual ICollection<UserInternships> UserInternships { get; set; }
    }
}
