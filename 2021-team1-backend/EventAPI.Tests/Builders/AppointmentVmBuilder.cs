using System;
using EventAPI.Domain.Models;
using EventAPI.Domain.ViewModels;

namespace EventAPI.Tests.Builders
{
    public class AppointmentVmBuilder
    {
        private readonly AppointmentVM _appointmentVm;

        public AppointmentVmBuilder()
        {
            _appointmentVm = new AppointmentVM();
        }

        public AppointmentVmBuilder FromAppointment(Appointment appointment)
        {
            _appointmentVm.Id = appointment.Id;
            _appointmentVm.EventId = appointment.EventId;
            _appointmentVm.CompanyId = appointment.CompanyId;
            _appointmentVm.AttendeeId = appointment.AttendeeId;
            _appointmentVm.BeginHour = appointment.BeginHour;
            _appointmentVm.EndHour = appointment.EndHour;
            _appointmentVm.StudentId = appointment.StudentId;
            _appointmentVm.Disabled = appointment.Disabled;
            _appointmentVm.Comment = appointment.Comment;
            return this;
        }

        public AppointmentVmBuilder WithInvalidBeginHour
        {
            get
            {
                _appointmentVm.BeginHour = TimeSpan.Zero;
                return this;
            }
        }

        public AppointmentVM Build => _appointmentVm;
    }
}
