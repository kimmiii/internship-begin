using System;
using System.Collections.Generic;

namespace EventAPI.Domain.Models
{
    public class EventCompany
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public string Website { get; set; }
        public string CompanyDescription { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public int TimeSlot { get; set; }
        public int CreateAppointmentUntil { get; set; }
        public int CancelAppointmentUntil { get; set; }
        public ICollection<Attendee> Attendees { get; set; }
    }
}