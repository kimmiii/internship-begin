using System;

namespace EventAPI.Domain.Models
{
    public class AcademicYear
    {
        public Guid Id { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string Description { get; set; }
        public Event Event { get; set; }
    }
}