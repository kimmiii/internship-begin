using EventAPI.Domain.Models;
using EventAPI.Domain.ViewModels;

namespace EventAPI.Tests.Builders
{
    public class AppointmentWithoutStudentDataVMBuilder
    {
        private readonly AppointmentWithoutStudentDataVM _appointmentWithoutStudentDataVM;

        public AppointmentWithoutStudentDataVMBuilder()
        {
            _appointmentWithoutStudentDataVM = new AppointmentWithoutStudentDataVM();
        }

        public AppointmentWithoutStudentDataVMBuilder FromAppointment(Appointment appointment)
        {
            _appointmentWithoutStudentDataVM.Id = appointment.Id;
            _appointmentWithoutStudentDataVM.EventId = appointment.EventId;
            _appointmentWithoutStudentDataVM.CompanyId = appointment.CompanyId;
            _appointmentWithoutStudentDataVM.AttendeeId = appointment.AttendeeId;
            _appointmentWithoutStudentDataVM.BeginHour = appointment.BeginHour;
            _appointmentWithoutStudentDataVM.EndHour = appointment.EndHour;
            _appointmentWithoutStudentDataVM.Disabled = appointment.Disabled;
            return this;
        }

        public AppointmentWithoutStudentDataVM Build => _appointmentWithoutStudentDataVM;
    }
}
