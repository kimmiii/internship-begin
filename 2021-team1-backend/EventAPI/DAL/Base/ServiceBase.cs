using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EventAPI.DAL.Base
{
    public interface IServiceBase<TEntity> where TEntity : class
    {
        Task<TEntity> GetByUrlSingleAsync(string url = "");
        Task<IEnumerable<TEntity>> GetByUrlListAsync(string url = "");
        Task UpdateUrlAsync(string url, TEntity entity);
    }

    public class ServiceBase<TEntity> : IServiceBase<TEntity> where TEntity : class
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ServiceBase<TEntity>> _logger;

        public ServiceBase(
            ILogger<ServiceBase<TEntity>> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration
        )
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("StagebeheerAPI");
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public async Task<TEntity> GetByUrlSingleAsync(string url = "")
        {
            try
            {
                var json = await SendCallAsync(url);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var items = JsonSerializer.Deserialize<TEntity>(json, options);
                return items;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<TEntity>> GetByUrlListAsync(string url = "")
        {
            try
            {
                var json = await SendCallAsync(url);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var items = JsonSerializer.Deserialize<IEnumerable<TEntity>>(json, options);
                return items;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateUrlAsync(string url, TEntity entity)
        {
            try
            {
                var className = typeof(TEntity).Name.ToLower();
                var uriValue = Configuration.GetSection(className).GetValue<string>("API");

                var entityToUpdate = new StringContent(
                    JsonSerializer.Serialize(entity),
                    Encoding.UTF8,
                    "application/json");

                using var httpResponse =
                    await _httpClient.PutAsync($"{uriValue}", entityToUpdate);

                httpResponse.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task<string> SendCallAsync(string url)
        {
            try
            {
                var className = typeof(TEntity).Name.ToLower();
                var uriValue = Configuration.GetSection(className).GetValue<string>("API");
                var request = new HttpRequestMessage(HttpMethod.Get, $"{uriValue}/{url}");
                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}