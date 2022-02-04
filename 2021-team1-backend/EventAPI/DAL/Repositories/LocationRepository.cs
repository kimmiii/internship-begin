using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using EventAPI.DAL.Base;
using EventAPI.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EventAPI.DAL.Repositories
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Location>> GetAsync();
    }

    public class LocationRepository : ServiceBase<Location>, ILocationRepository
    {
        public LocationRepository(
            ILogger<ServiceBase<Location>> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration) : base(logger, httpClientFactory, configuration)
        {
        }

        public async Task<IEnumerable<Location>> GetAsync()
        {
            return await GetByUrlListAsync("locations");
        }
    }
}