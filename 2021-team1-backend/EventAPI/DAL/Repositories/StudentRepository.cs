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
    public interface IStudentRepository
    {
        Task<User> GetByIdAsync(int id);
    }
    public class StudentRepository : ServiceBase<User>, IStudentRepository
    {
        public StudentRepository(
            ILogger<ServiceBase<User>> logger, 
            IHttpClientFactory httpClientFactory, 
            IConfiguration configuration) : base(logger, httpClientFactory, configuration)
        {
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await GetByUrlSingleAsync($"{id}");
        }
    }
}
