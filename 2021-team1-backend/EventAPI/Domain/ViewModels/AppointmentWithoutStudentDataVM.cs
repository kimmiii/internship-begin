using System;

namespace EventAPI.Domain.ViewModels
{
    public class AppointmentWithoutStudentDataVM
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public Guid AttendeeId { get; set; }
        public TimeSpan BeginHour { get; set; }
        public TimeSpan EndHour { get; set; }
        public string AppointmentStatus { get; set; }
        public bool Disabled { get; set; }
    }
}