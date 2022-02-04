using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventAPI.DAL.Base;
using EventAPI.Domain.Models;

namespace EventAPI.DAL.Repositories
{
    public interface IAttendeeRepository
    {
        Task<Attendee> GetByIdAsync(Guid id);
        Task<Attendee> CreateAsync(Attendee attendee);
    }
    public class AttendeeRepository : RepositoryBase<Attendee>, IAttendeeRepository
    {
        public AttendeeRepository(EventDBContext context) : base(context) { }
    }
}
