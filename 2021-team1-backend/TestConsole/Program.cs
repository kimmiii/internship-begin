using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StagebeheerAPI.Contracts;
using StagebeheerAPI.Domain.DatabaseSeeder;
using StagebeheerAPI.Models;
using StagebeheerAPI.Repository;
using System;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Stagebeheer;Integrated Security=True;";

            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddDbContext<StagebeheerDBContext>(
                    opt => opt.UseSqlServer(connectionString))
                .AddTransient<IRepositoryWrapper, RepositoryWrapper>()
                .BuildServiceProvider();

            var repositoryWrapper = serviceProvider.GetService<IRepositoryWrapper>();


            Console.WriteLine("Welcome to the Seeder!");
            Console.WriteLine("----------------------");
            Console.WriteLine("");
            Console.WriteLine("Your connectionstring is: " + connectionString);
            Console.WriteLine("If this is not correct, please change it in Program.cs.");
            Console.WriteLine("We currently do not support reading your connection string from your settings. :(");
            Console.WriteLine("");
            Console.WriteLine("Options: ");
            Console.WriteLine("1. Seed all");
            //Console.WriteLine("2. Clean Database and seed all");
            Console.WriteLine("");
            Console.WriteLine("3. Seed static tables");
            //Console.WriteLine("4. Remove data from static tables and seed");
            Console.WriteLine("5. Seed test data (static tables need to be seeded first!)");
            //Console.WriteLine("6. Clean db and seed test data (static tables need to be seeded first!)");
            Console.WriteLine("");
            Console.WriteLine("Please enter number you wish to run:");

            var selectedOption = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("");

            switch (selectedOption)
            {
                case 1:
                    new DatabaseSeeder(repositoryWrapper).SeedAllTables();
                    Console.WriteLine("Done!");
                    break;
                case 2:
                    new DatabaseSeeder(repositoryWrapper).SeedAllTables(true);
                    Console.WriteLine("Done!");
                    break;
                case 3:
                    new DatabaseSeeder(repositoryWrapper).SeedStaticTables();
                    Console.WriteLine("Done!");
                    break;
                case 4:
                    new DatabaseSeeder(repositoryWrapper).SeedStaticTables(true);
                    Console.WriteLine("Done!");
                    break;
                case 5:
                    new DatabaseSeeder(repositoryWrapper).SeedTestData();
                    Console.WriteLine("Done!");
                    break;
                case 6:
                    new DatabaseSeeder(repositoryWrapper).SeedTestData(true);
                    Console.WriteLine("Done!");
                    break;
                default:
                    Console.WriteLine("Selected option not available. Please rerun the application");
                    break;
            }

            Console.WriteLine("");
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }
    }
}
