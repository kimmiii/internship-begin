using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EventAPI.DAL.Base;
using EventAPI.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EventAPI.DAL.Repositories
{
    public interface IContactRepository
    {
        Task<IEnumerable<Contact>> GetAsync();
    }
    public class ContactRepository : ServiceBase<Contact>, IContactRepository
    {
        public ContactRepository(ILogger<ServiceBase<Contact>> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(logger, httpClientFactory, configuration)
        {
        }

        public async Task<IEnumerable<Contact>> GetAsync()
        {
            return await GetByUrlListAsync("contacts");
        }
    }
}
