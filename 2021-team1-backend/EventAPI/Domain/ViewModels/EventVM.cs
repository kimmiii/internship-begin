using System;
using System.ComponentModel.DataAnnotations;
using EventAPI.Domain.Models;

namespace EventAPI.Domain.ViewModels
{
    public class EventVM
    {
        public Guid Id { get; set; }

        [Required] public string Name { get; set; }

        [Required] public DateTime DateEvent { get; set; }

        public TimeSpan StartHour { get; set; }
        public TimeSpan EndHour { get; set; }

        [Required] public EventLocation Location { get; set; }

        [Required] public Guid AcademicYearId { get; set; }

        public bool IsActivated { get; set; }
    }
}