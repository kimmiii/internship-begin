using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using EventAPI.DAL.Base;
using EventAPI.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EventAPI.DAL.Repositories
{
    public interface IEnvironmentRepository
    {
        Task<IEnumerable<Environment>> GetAsync();
    }
    public class EnvironmentRepository : ServiceBase<Environment>, IEnvironmentRepository
    {
        public EnvironmentRepository(
            ILogger<ServiceBase<Environment>> logger, 
            IHttpClientFactory httpClientFactory, 
            IConfiguration configuration) : base(logger, httpClientFactory, configuration)
        {
        }

        public async Task<IEnumerable<Environment>> GetAsync()
        {
            return await GetByUrlListAsync("environments");
        }
    }
}
