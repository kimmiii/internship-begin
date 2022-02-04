using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EventAPI.DAL.Repositories;
using EventAPI.Domain.Models;

namespace EventAPI.BLL
{
    public interface IAttendeeBLL
    {
        Task<Attendee> GetByIdAsync(Guid id);
        Task<AttendeeVM> GetByIdAsyncVm(Guid id);
        Task<AttendeeVM> CreateAttendeeAsync(Attendee attendee);
    }
    public class AttendeeBLL : IAttendeeBLL
    {
        private readonly IMapper _mapper;
        private readonly IAttendeeRepository _attendeeRepository;

        public AttendeeBLL(
            IMapper mapper,
            IAttendeeRepository attendeeRepository)
        {
            _mapper = mapper;
            _attendeeRepository = attendeeRepository;
        }
        public async Task<Attendee> GetByIdAsync(Guid id)
        {
            var attendee = await _attendeeRepository.GetByIdAsync(id);
            return attendee;
        }

        public async Task<AttendeeVM> GetByIdAsyncVm(Guid id)
        {
            var attendee = await GetByIdAsync(id);
            return _mapper.Map<AttendeeVM>(attendee);
        }

        public async Task<AttendeeVM> CreateAttendeeAsync(Attendee attendee)
        {
            attendee = await _attendeeRepository.CreateAsync(attendee);
            return _mapper.Map<AttendeeVM>(attendee);
        }
    }
}
