using System;

namespace EventAPI.Domain.Models
{
    public class AttendeeVM
    {
        public Guid Id { get; set; }
        public Guid EventCompanyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}