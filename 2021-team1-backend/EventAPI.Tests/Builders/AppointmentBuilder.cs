using EventAPI.Domain.Models;
using System;

namespace EventAPI.Tests.Builders
{
    public class AppointmentBuilder
    {
        private readonly Appointment _appointment;

        public AppointmentBuilder()
        {
            _appointment = new Appointment
            {
                AttendeeId = Guid.NewGuid()
            };
        }

        public AppointmentBuilder WithId
        {
            get
            {
                _appointment.Id = Guid.NewGuid();
                return this;
            }
        }

        public AppointmentBuilder WithCompanyId(int id)
        {
            _appointment.CompanyId = id;
            return this;
        }

        public AppointmentBuilder WithEventId(Guid id)
        {
            _appointment.EventId = id;
            return this;
        }

        public AppointmentBuilder WithStudentId(int studentId)
        {
            _appointment.StudentId = studentId;
            return this;
        }

        public Appointment Build => _appointment;

        
    }
}
