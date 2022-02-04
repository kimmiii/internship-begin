using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using EventAPI.DAL.Base;
using EventAPI.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EventAPI.DAL.Repositories
{
    public interface IInternshipRepository
    {
        Task<IEnumerable<Internship>> GetAsync();
        Task<Internship> GetByIdAsync(int id);
        Task<IEnumerable<Internship>> GetByCompanyAsync(int id);
        Task<IEnumerable<Internship>> GetByAcademicYearAsync(string academicYear);
        Task<IEnumerable<Internship>> GetInternshipsFromEventByCompanyAsync(string academicYear, int companyId);
        Task UpdateAsync(Internship internship);
    }

    public class InternshipRepository : ServiceBase<Internship>, IInternshipRepository
    {
        public InternshipRepository(
            ILogger<ServiceBase<Internship>> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration) : base(logger, httpClientFactory, configuration)
        {
        }

        public Task<IEnumerable<Internship>> GetAsync()
        {
            return GetByUrlListAsync();
        }

        public async Task<Internship> GetByIdAsync(int id)
        {
            return await GetByUrlSingleAsync($"{id}");
        }

        public async Task<IEnumerable<Internship>> GetByCompanyAsync(int id)
        {
            return await GetByUrlListAsync($"company/{id}");
        }

        public async Task<IEnumerable<Internship>> GetByAcademicYearAsync(string academicYear)
        {
            return await GetByUrlListAsync($"academic-year/{academicYear}");
        }

        public async Task<IEnumerable<Internship>> GetInternshipsFromEventByCompanyAsync(string academicYear,
            int companyId)
        {
            return await GetByUrlListAsync($"academic-year/{academicYear}/company-id/{companyId}");
        }

        public async Task UpdateAsync(Internship internship)
        {
            await UpdateUrlAsync("internships", internship);
        }
    }
}