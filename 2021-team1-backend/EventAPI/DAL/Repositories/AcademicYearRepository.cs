using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventAPI.DAL.Base;
using EventAPI.Domain.Models;

namespace EventAPI.DAL.Repositories
{
    public interface IAcademicYearRepository
    {
        Task<IEnumerable<AcademicYear>> GetAsync();
        Task<AcademicYear> GetByIdAsync(Guid id);
        Task<AcademicYear> CreateAsync();
        Task<AcademicYear> CreateAsync(AcademicYear academicYear);
        Task<AcademicYear> UpdateAsync(AcademicYear academicYear);
    }

    public class AcademicYearRepository : RepositoryBase<AcademicYear>, IAcademicYearRepository
    {
        private readonly EventDBContext _context;

        public AcademicYearRepository(EventDBContext context) : base(context)
        {
            _context = context;
        }

        public Task<AcademicYear> CreateAsync()
        {
            var academicYear = GetNextAcademicYear();
            return CreateAsync(academicYear);
        }

        public AcademicYear GetNextAcademicYear()
        {
            var academicYear = new AcademicYear
            {
                StartYear = _context.AcademicYears.Max(x => x.StartYear) + 1
            };
            return academicYear;
        }
    }
}