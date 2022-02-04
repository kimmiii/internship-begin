using System;
using System.Collections.Generic;
using EventAPI.Domain.Models;

namespace EventAPI.Tests.Builders
{
    public class EventCompanyListBuilder
    {
        private readonly List<EventCompany> _eventCompanies;

        public EventCompanyListBuilder()
        {
            _eventCompanies = new List<EventCompany>();
        }

        public EventCompanyListBuilder WithCompanyId(int id)
        {
            for (var i = 0; i < 3; i++)
            {
                _eventCompanies.Add(new EventCompanyBuilder().WithCompanyId(id).Build);
            }
            return this;
        }
        public EventCompanyListBuilder WithEventId(Guid id)
        {
            for (var i = 0; i < 3; i++)
            {
                _eventCompanies.Add(new EventCompanyBuilder().WithEventId(id).Build);
            }
            return this;
        }

        public List<EventCompany> Build => _eventCompanies;

        
    }
}