using System;
using System.Collections.Generic;

namespace EventAPI.Domain.Models
{
    public class Internship
    {
        public int InternshipId { get; set; }
        public int CompanyId { get; set; }
        public int ProjectStatusId { get; set; }
        public string WpStreet { get; set; }
        public string WpHouseNr { get; set; }
        public string WpBusNr { get; set; }
        public string WpZipCode { get; set; }
        public string WpCity { get; set; }
        public string WpCountry { get; set; }
        public int ContactPersonId { get; set; }
        public string PromotorFirstname { get; set; }
        public string PromotorSurname { get; set; }
        public string PromotorFunction { get; set; }
        public string PromotorEmail { get; set; }
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
        public string ExternalFeedback { get; set; }
        public string InternalFeedback { get; set; }
        public int CountTotalAssignedReviewers { get; set; } = 0;
        public DateTime? SentToReviewersAt { get; set; }
        public bool Completed { get; set; }
        public bool ShowInEvent { get; set; }
        public ICollection<Specialisation> InternshipSpecialisation { get; set; }
        public ICollection<Environment> InternshipEnvironment { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}