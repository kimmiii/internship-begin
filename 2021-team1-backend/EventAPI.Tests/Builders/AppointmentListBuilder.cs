using System;
using EventAPI.Domain.Models;
using System.Collections.Generic;

namespace EventAPI.Tests.Builders
{
    public class AppointmentListBuilder
    {
        private readonly List<Appointment> _appointments;

        public AppointmentListBuilder()
        {
            _appointments = new List<Appointment>();
        }

        public AppointmentListBuilder WithCompanyIdAndEventId(int companyId, Guid eventId)
        {
            for (var i = 0; i < 3; i++)
            {
                _appointments.Add(new AppointmentBuilder().WithCompanyId(companyId).WithEventId(eventId).Build);
            }
            return this;
        }

        public AppointmentListBuilder WithStudentIdAndEventId(int studentId, Guid eventId)
        {
            for (var i = 0; i < 3; i++)
            {
                _appointments.Add(new AppointmentBuilder().WithStudentId(studentId).WithEventId(eventId).Build);
            }
            return this;
        }


        public List<Appointment> Build => _appointments;

        
    }
}