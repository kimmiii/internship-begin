using EventAPI.Domain.Models;
using System;
using System.Collections.Generic;

namespace EventAPI.Domain.ViewModels
{
    public class EventCompanyVM
    {
        public Guid Id { get; set; }

        public Guid EventId { get; set; }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }

        public string Website { get; set; }
        public string CompanyDescription { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public int TimeSlot { get; set; }
        public int CreateAppointmentUntil { get; set; }
        public int CancelAppointmentUntil { get; set; }
        public IList<AttendeeVM> Attendees { get; set; }
    }
}