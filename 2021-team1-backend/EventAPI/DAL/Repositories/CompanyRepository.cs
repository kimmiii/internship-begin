using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using EventAPI.DAL.Base;
using EventAPI.Domain.Models;
using EventAPI.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EventAPI.DAL.Repositories
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetAsync();
        Task<Company> GetByIdAsync();
        Task<Company> GetByIdAsync(int id);
    }

    public class CompanyRepository : ServiceBase<Company>, ICompanyRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompanyRepository(
            ILogger<ServiceBase<Company>> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) : base(logger, httpClientFactory, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Company>> GetAsync()
        {
            return await GetByUrlListAsync();
        }

        public async Task<Company> GetByIdAsync()
        {
            var id = Helper.GetClaimFromToken<int>(_httpContextAccessor, "companyId");
            return await GetByUrlSingleAsync($"{id}");
        }

        public async Task<Company> GetByIdAsync(int id)
        {
            return await GetByUrlSingleAsync($"{id}");
        }
    }
}