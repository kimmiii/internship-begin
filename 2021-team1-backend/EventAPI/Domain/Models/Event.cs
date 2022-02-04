using System;
using System.Collections.Generic;

namespace EventAPI.Domain.Models
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateEvent { get; set; }
        public TimeSpan StartHour { get; set; }
        public TimeSpan EndHour { get; set; }
        public EventLocation Location { get; set; }
        public bool IsActivated { get; set; }
        public Guid AcademicYearId { get; set; }
        public AcademicYear AcademicYear { get; set; }
        public ICollection<EventCompany> EventCompanies { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }

    public enum EventLocation
    {
        ONLINE = 0,
        ON_CAMPUS = 1
    }
}