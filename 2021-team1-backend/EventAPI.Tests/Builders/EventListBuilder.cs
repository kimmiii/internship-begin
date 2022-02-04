using EventAPI.Domain.Models;
using System.Collections.Generic;

namespace EventAPI.Tests.Builders
{
    public class EventListBuilder
    {
        private readonly List<Event> _events;

        public EventListBuilder()
        {
            _events = new List<Event>();
        }
      
        public List<Event> Build => _events;
    }
}