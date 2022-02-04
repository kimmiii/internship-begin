using Microsoft.Extensions.DependencyInjection;
using StagebeheerAPI.Contracts;
using StagebeheerAPI.Repository;

namespace StagebeheerAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }
    }
}
