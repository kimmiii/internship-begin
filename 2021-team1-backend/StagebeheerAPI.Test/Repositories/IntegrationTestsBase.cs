using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using StagebeheerAPI.Models;
using StagebeheerAPI.Repository;

namespace StagebeheerAPI.Test.Repositories
{
    class IntegrationTestsBase
    {
        private StagebeheerDBContext _StagebeheerDbContext;
        protected RepositoryWrapper RepositoryWrapper;

        [OneTimeSetUp]
        public void Init()
        {
            // Integration tests will always be run on test database.
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.Test.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration config = configBuilder.Build();

            var connectionstring = config["ConnectionStrings:StagebeheerDB"];

            // Configure seperate DbContext because the actual DbContext is in another project.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<StagebeheerDBContext>();

            builder.UseSqlServer(connectionstring)
                .UseInternalServiceProvider(serviceProvider);

            _StagebeheerDbContext = new StagebeheerDBContext(builder.Options);
            RepositoryWrapper = new RepositoryWrapper(_StagebeheerDbContext);
        }
    }
}

