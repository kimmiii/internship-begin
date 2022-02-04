using System;

namespace EventAPI.Domain.Models
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public Guid AttendeeId { get; set; }
        public Attendee Attendee { get; set; }
        public int StudentId { get; set; }
        public int InternshipId { get; set; }
        public Internship Internship { get; set; }
        public TimeSpan BeginHour { get; set; }
        public TimeSpan EndHour { get; set; }
        public string Comment { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }
        public string CancelMotivation { get; set; }
        public bool Disabled { get; set; }
        public string OnlineMeetingLink { get; set; }
    }

    public enum AppointmentStatus
    {
        RESERVED = 0,
        CONFIRMED = 1,
        CANCELLED = 2
    }
}