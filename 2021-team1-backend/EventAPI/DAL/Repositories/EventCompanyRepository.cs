using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventAPI.DAL.Base;
using EventAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EventAPI.DAL.Repositories
{
    public interface IEventCompanyRepository
    {
        Task<IEnumerable<EventCompany>> GetAsync();
        Task<EventCompany> GetByIdAsync(Guid id);
        Task<EventCompany> GetByEventAndCompanyAsync(Guid id, int companyId);
        Task<List<EventCompany>> GetByEventIdAsync(Guid id);
        Task<EventCompany> CreateAsync(EventCompany eventCompany);
        Task<EventCompany> UpdateAsync(EventCompany eventCompany);
    }

    public class EventCompanyRepository : RepositoryBase<EventCompany>, IEventCompanyRepository
    {
        private readonly DbSet<EventCompany> _dbSet;

        public EventCompanyRepository(EventDBContext context) : base(context)
        {
            _dbSet = context.Set<EventCompany>();
        }

        public async Task<EventCompany> GetByEventAndCompanyAsync(Guid id, int companyId)
        {
            return await _dbSet
                .Where(e => e.EventId == id && e.CompanyId == companyId)
                .Include(x => x.Attendees)
                .FirstOrDefaultAsync();
        }

        public async Task<List<EventCompany>> GetByEventIdAsync(Guid id)
        {
            return await _dbSet
                .Where(e => e.EventId == id)
                .Include(e => e.Attendees)
                .ToListAsync();
        }
    }
}