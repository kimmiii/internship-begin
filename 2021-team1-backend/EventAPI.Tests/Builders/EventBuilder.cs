using EventAPI.Domain.Models;
using System;

namespace EventAPI.Tests.Builders
{
    public class EventBuilder
    {
        private readonly Event _event;

        public EventBuilder()
        {
            _event = new Event
            {
                Name = Guid.NewGuid().ToString(),
            };
        }

        public EventBuilder WithId
        {
            get
            {
                _event.Id = Guid.NewGuid();
                return this;
            }
        }

        public EventBuilder IsActivated
        {
            get
            {
                _event.IsActivated = true;
                return this;
            }
        }

        public EventBuilder WithAcademicYear(Guid academicYearId)
        {
            _event.AcademicYearId = academicYearId;
            return this;
        }

        public Event Build => _event;
    }
}
