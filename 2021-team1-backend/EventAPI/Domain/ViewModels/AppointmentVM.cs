using System;
using EventAPI.Domain.Models;

namespace EventAPI.Domain.ViewModels
{
    public class AppointmentVM
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public Guid AttendeeId { get; set; }
        public string AttendeeName { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int InternshipId { get; set; }
        public string ResearchTopicTitle { get; set; }
        public TimeSpan BeginHour { get; set; }
        public TimeSpan EndHour { get; set; }
        public string Comment { get; set; }
        public string AppointmentStatus { get; set; }
        public string CancelMotivation { get; set; }
        public bool Disabled { get; set; }
        public string OnlineMeetingLink { get; set; }
    }
}