using System;
using System.Collections.Generic;

namespace EventAPI.Domain.Models
{
    public class Attendee
    {
        public Guid Id { get; set; }
        public Guid EventCompanyId { get; set; }
        public EventCompany EventCompany { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Appointment> Appointments { get; set; }
    }
}