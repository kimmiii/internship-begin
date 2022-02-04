using System;
using EventAPI.Domain.Models;
using EventAPI.Domain.ViewModels;

namespace EventAPI.Tests.Builders
{
    public class EventVmBuilder
    {
        private readonly EventVM _eventVm;

        public EventVmBuilder()
        {
            _eventVm = new EventVM
            {
                Name = Guid.NewGuid().ToString(),
            };
        }

        public EventVmBuilder WithId
        {
            get
            {
                _eventVm.Id = Guid.NewGuid();
                return this;
            }
        }

        public EventVmBuilder IsActivated
        {
            get
            {
                _eventVm.IsActivated = true;
                return this;
            }
        }

        public EventVmBuilder FromEvent(Event e)
        {
            _eventVm.Id = e.Id;
            _eventVm.Name = e.Name;
            _eventVm.DateEvent = e.DateEvent;
            _eventVm.StartHour = e.StartHour;
            _eventVm.EndHour = e.EndHour;
            _eventVm.Location = e.Location;
            _eventVm.AcademicYearId = e.AcademicYearId;
            _eventVm.IsActivated = e.IsActivated;

            return this;
        }

        public EventVmBuilder WithEmptyName
        {
            get
            {
                _eventVm.Name = null;
                return this;
            }
        }

        public EventVM Build =>_eventVm;
    }
}
