using System;
using EventAPI.Domain.Models;
using EventAPI.Domain.ViewModels;

namespace EventAPI.Tests.Builders
{
    public class EventCompanyVMBuilder
    {
        private readonly EventCompanyVM _eventCompanyVm;

        public EventCompanyVMBuilder()
        {
            _eventCompanyVm = new EventCompanyVM
            {
                EventId = Guid.NewGuid(),
                CompanyId = new Random().Next(1,10)
            };
        }

        public EventCompanyVMBuilder WithoutEventId
        {
            get
            {
                _eventCompanyVm.EventId = Guid.Empty;
                return this;
            }
        }

        public EventCompanyVMBuilder FromEventCompany(EventCompany eventCompany)
        {
            _eventCompanyVm.EventId = eventCompany.EventId;
            _eventCompanyVm.CompanyId = eventCompany.CompanyId;
            _eventCompanyVm.ArrivalTime = eventCompany.ArrivalTime;
            _eventCompanyVm.CancelAppointmentUntil = eventCompany.CancelAppointmentUntil;
            _eventCompanyVm.CreateAppointmentUntil = eventCompany.CreateAppointmentUntil;
            _eventCompanyVm.DepartureTime = eventCompany.DepartureTime;
            _eventCompanyVm.TimeSlot = eventCompany.TimeSlot;
            _eventCompanyVm.Website = eventCompany.Website;

            return this;
        }

        public EventCompanyVM Build =>_eventCompanyVm;
    }
}
