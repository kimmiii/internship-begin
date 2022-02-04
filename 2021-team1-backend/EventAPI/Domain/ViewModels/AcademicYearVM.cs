using System;

namespace EventAPI.Domain.ViewModels
{
    public class AcademicYearVM
    {
        public Guid Id { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string Description { get; set; }
    }
}