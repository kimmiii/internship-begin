using EventAPI.Domain.Models;
using System;

namespace EventAPI.Tests.Builders
{
    public class EventCompanyBuilder
    {
        private readonly EventCompany _eventCompany;

        public EventCompanyBuilder()
        {
            _eventCompany = new EventCompany
            {
                EventId = Guid.NewGuid(),
                CompanyId = new Random().Next()
            };
        }

        public EventCompanyBuilder WithoutEventId
        {
            get
            {
                _eventCompany.EventId = Guid.Empty;
                return this;
            }
        }

        public EventCompanyBuilder WithEventId(Guid id)
        {
            _eventCompany.EventId = id;
            return this;
        }

        public EventCompanyBuilder WithCompanyId(int id)
        {
            _eventCompany.CompanyId = id;
            return this;
        }

        public EventCompany Build =>_eventCompany;
    }
}
