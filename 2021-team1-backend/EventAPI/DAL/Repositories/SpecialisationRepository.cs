using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using EventAPI.DAL.Base;
using EventAPI.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EventAPI.DAL.Repositories
{
    public interface ISpecializationRepository
    {
        Task<IEnumerable<Specialisation>> GetAsync();
    }

    public class SpecializationRepository : ServiceBase<Specialisation>, ISpecializationRepository
    {
        public SpecializationRepository(
            ILogger<ServiceBase<Specialisation>> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration) : base(logger, httpClientFactory, configuration)
        {
        }

        public async Task<IEnumerable<Specialisation>> GetAsync()
        {
            return await GetByUrlListAsync();
        }
    }
}