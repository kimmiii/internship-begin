using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EventAPI.DAL.Repositories;
using EventAPI.Domain.Models;

namespace EventAPI.BLL
{
    public interface IAppointmentBll
    {
        Task<Appointment> GetById(Guid id);
        Task<IList<Appointment>> GetAllAsync(Guid eventId);
        Task<IList<Appointment>> GetAllConfirmedAsync(Guid eId);
        Task<IList<Appointment>> GetAllByStatusAsync(Guid eId, List<AppointmentStatus> allStatus);
        Task<IList<Appointment>> GetByCompanyAsync(Guid eventId, int companyId);
        Task<IList<Appointment>> GetByCompanyAsyncByStatus(Guid eId, int companyId, List<AppointmentStatus> status);
        Task<IList<Appointment>> GetByStudentAsync(Guid eventId, int studentId);
        Task<IList<Appointment>> GetByStudentAsyncByStatus(Guid eId, int studentId, List<AppointmentStatus> status);
        Task<Appointment> CreateAppointmentAsync(Appointment appointment);
        Task<Appointment> UpdateAppointmentAsync(Appointment appointment);
        Task<Appointment> Delete(Guid id);
        Task<Appointment> CheckOnExistingAppointmentAsync(Guid eventId, int companyId, Guid attendeeId, TimeSpan beginHour);
    }

    public class AppointmentBll : IAppointmentBll
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public AppointmentBll(
            IMapper mapper,
            IAppointmentRepository appointmentRepository
        )
        {
            _mapper = mapper;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<IList<Appointment>> GetAllAsync(Guid eventId)
        {
            var appointments =
                await _appointmentRepository.FindByCondition(a => a.EventId == eventId);
            return appointments.ToList();
        }

        public async Task<IList<Appointment>> GetAllConfirmedAsync(Guid eventId)
        {
            var appointments =
                await _appointmentRepository.FindByCondition(a => a.EventId == eventId && a.AppointmentStatus == AppointmentStatus.CONFIRMED);
            return appointments.ToList();
        }

        public async Task<IList<Appointment>> GetAllByStatusAsync(Guid eventId, List<AppointmentStatus> allStatus)
        {
            Expression<Func<Appointment, bool>> expression = null;
            switch (allStatus.Count)
            {
                case 1:
                    if (allStatus.Contains(AppointmentStatus.CONFIRMED))
                    {
                        expression = x => x.EventId == eventId && x.AppointmentStatus == AppointmentStatus.CONFIRMED;
                        break;
                    };
                    if (allStatus.Contains(AppointmentStatus.RESERVED))
                    {
                        expression = x => x.EventId == eventId && x.AppointmentStatus == AppointmentStatus.RESERVED;
                        break;
                    };
                    if (allStatus.Contains(AppointmentStatus.CANCELLED))
                    {
                        expression = x => x.EventId == eventId && x.AppointmentStatus == AppointmentStatus.CANCELLED;
                        break;
                    };
                    break;
                case 2:
                    if (!allStatus.Contains(AppointmentStatus.CONFIRMED)) {
                        expression = x => x.EventId == eventId && (x.AppointmentStatus == AppointmentStatus.RESERVED || x.AppointmentStatus == AppointmentStatus.CANCELLED);
                        break;
                    };
                    if (!allStatus.Contains(AppointmentStatus.RESERVED))
                    {
                        expression = x => x.EventId == eventId && (x.AppointmentStatus == AppointmentStatus.CONFIRMED || x.AppointmentStatus == AppointmentStatus.CANCELLED);
                        break;
                    };
                    if (!allStatus.Contains(AppointmentStatus.CANCELLED))
                    {
                        expression = x => x.EventId == eventId && (x.AppointmentStatus == AppointmentStatus.CONFIRMED || x.AppointmentStatus == AppointmentStatus.RESERVED);
                        break;
                    };
                    break;
                default:
                    expression = x => x.EventId == eventId;
                    break;
            }
            var appointments =
                await _appointmentRepository.FindByCondition(expression);
            return appointments.ToList();
        }

        public async Task<Appointment> GetById(Guid id)
        {
            var appointment =
                await _appointmentRepository.GetByIdAsync(id);
            return appointment;
        }

        public async Task<IList<Appointment>> GetByCompanyAsync(Guid eventId, int companyId)
        {
            var appointments =
                await _appointmentRepository.FindByCondition(a => a.EventId == eventId && a.CompanyId == companyId);
            return appointments.ToList();
        }

        public async Task<IList<Appointment>> GetByCompanyAsyncByStatus(Guid eventId, int companyId, List<AppointmentStatus> status)
        {
            Expression<Func<Appointment, bool>> expression = null;
            switch (status.Count)
            {
                case 1:
                    if (status.Contains(AppointmentStatus.CONFIRMED))
                    {
                        expression = x => x.EventId == eventId && x.CompanyId == companyId && x.AppointmentStatus == AppointmentStatus.CONFIRMED;
                        break;
                    };
                    if (status.Contains(AppointmentStatus.RESERVED))
                    {
                        expression = x => x.EventId == eventId && x.CompanyId == companyId && x.AppointmentStatus == AppointmentStatus.RESERVED;
                        break;
                    };
                    if (status.Contains(AppointmentStatus.CANCELLED))
                    {
                        expression = x => x.EventId == eventId && x.CompanyId == companyId && x.AppointmentStatus == AppointmentStatus.CANCELLED;
                        break;
                    };
                    break;
                case 2:
                    if (!status.Contains(AppointmentStatus.CONFIRMED))
                    {
                        expression = x => x.EventId == eventId && x.CompanyId == companyId && (x.AppointmentStatus == AppointmentStatus.RESERVED || x.AppointmentStatus == AppointmentStatus.CANCELLED);
                        break;
                    };
                    if (!status.Contains(AppointmentStatus.RESERVED))
                    {
                        expression = x => x.EventId == eventId && x.CompanyId == companyId && (x.AppointmentStatus == AppointmentStatus.CONFIRMED || x.AppointmentStatus == AppointmentStatus.CANCELLED);
                        break;
                    };
                    if (!status.Contains(AppointmentStatus.CANCELLED))
                    {
                        expression = x => x.EventId == eventId && x.CompanyId == companyId && (x.AppointmentStatus == AppointmentStatus.CONFIRMED || x.AppointmentStatus == AppointmentStatus.RESERVED);
                        break;
                    };
                    break;
                default:
                    expression = x => x.EventId == eventId;
                    break;
            }
            var appointments =
                await _appointmentRepository.FindByCondition(expression);
            return appointments.ToList();
        }

        public async Task<IList<Appointment>> GetByStudentAsync(Guid eventId, int studentId)
        {
            var appointments =
                await _appointmentRepository.FindByCondition(a => a.EventId == eventId && a.StudentId == studentId);
            return appointments.ToList();
        }

        public async Task<IList<Appointment>> GetByStudentAsyncByStatus(Guid eventId, int studentId, List<AppointmentStatus> status)
        {
            Expression<Func<Appointment, bool>> expression = null;
            switch (status.Count)
            {
                case 1:
                    if (status.Contains(AppointmentStatus.CONFIRMED))
                    {
                        expression = x => x.EventId == eventId && x.StudentId == studentId && x.AppointmentStatus == AppointmentStatus.CONFIRMED;
                        break;
                    };
                    if (status.Contains(AppointmentStatus.RESERVED))
                    {
                        expression = x => x.EventId == eventId && x.StudentId == studentId && x.AppointmentStatus == AppointmentStatus.RESERVED;
                        break;
                    };
                    if (status.Contains(AppointmentStatus.CANCELLED))
                    {
                        expression = x => x.EventId == eventId && x.StudentId == studentId && x.AppointmentStatus == AppointmentStatus.CANCELLED;
                        break;
                    };
                    break;
                case 2:
                    if (!status.Contains(AppointmentStatus.CONFIRMED))
                    {
                        expression = x => x.EventId == eventId && x.StudentId == studentId && (x.AppointmentStatus == AppointmentStatus.RESERVED || x.AppointmentStatus == AppointmentStatus.CANCELLED);
                        break;
                    };
                    if (!status.Contains(AppointmentStatus.RESERVED))
                    {
                        expression = x => x.EventId == eventId && x.StudentId == studentId && (x.AppointmentStatus == AppointmentStatus.CONFIRMED || x.AppointmentStatus == AppointmentStatus.CANCELLED);
                        break;
                    };
                    if (!status.Contains(AppointmentStatus.CANCELLED))
                    {
                        expression = x => x.EventId == eventId && x.StudentId == studentId && (x.AppointmentStatus == AppointmentStatus.CONFIRMED || x.AppointmentStatus == AppointmentStatus.RESERVED);
                        break;
                    };
                    break;
                default:
                    expression = x => x.EventId == eventId;
                    break;
            }
            var appointments =
                await _appointmentRepository.FindByCondition(expression);
            return appointments.ToList();
        }


        public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
        {
            appointment = await _appointmentRepository.CreateAsync(appointment);
            return appointment;
        }

        public async Task<Appointment> UpdateAppointmentAsync(Appointment appointment)
        {
            appointment = await _appointmentRepository.UpdateAsync(appointment);
            return appointment;
        }

        public async Task<Appointment> Delete(Guid id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            await _appointmentRepository.DeleteAsync(appointment);
            return appointment;
        }

        public async Task<Appointment> CheckOnExistingAppointmentAsync(Guid eventId, int companyId, Guid attendeeId,
            TimeSpan beginHour)
        {
            var appointment =
                await _appointmentRepository.FindByCondition(x =>
                    x.EventId == eventId &&
                    x.CompanyId == companyId && 
                    x.AttendeeId == attendeeId &&
                    x.BeginHour == beginHour &&
                    x.AppointmentStatus != AppointmentStatus.CANCELLED);
            return appointment?.FirstOrDefault();
        }
    }
}