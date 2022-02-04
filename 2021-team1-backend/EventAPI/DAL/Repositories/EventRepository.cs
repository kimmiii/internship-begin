using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventAPI.DAL.Base;
using EventAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EventAPI.DAL.Repositories
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAsync();
        Task<Event> GetByIdAsync(Guid id);
        Task<Event> GetActiveAsync();
        Task<Event> CreateAsync(Event e);
        Task<Event> UpdateAsync(Event e);
    }

    public class EventRepository : RepositoryBase<Event>, IEventRepository
    {
        private readonly EventDBContext _context;
        private readonly DbSet<Event> _dbSet;

        public EventRepository(EventDBContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<Event>();
        }

        public async Task<Event> GetActiveAsync()
        {
            var entity = await _dbSet.FirstOrDefaultAsync(x => x.IsActivated);
            _context.Entry(entity).State = EntityState.Detached;
            return entity;
        }
    }
}