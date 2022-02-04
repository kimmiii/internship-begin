using System;
using System.Collections.Generic;
using System.Linq;
using EventAPI.DAL;
using EventAPI.Domain.Models;

namespace EventAPI.Domain.DataSeeder
{
    public interface IDatabaseSeeder
    {
        void SeedData();
    }

    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly EventDBContext _context;

        public DatabaseSeeder(EventDBContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            SeedAcademicYear();
        }

        private void SeedAcademicYear()
        {
            var academicYears = _context.AcademicYears.ToList();

            if (academicYears.Count < 1)
            {
                academicYears.AddRange(new List<AcademicYear>
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            StartYear = 2020
                        },
                        new()
                        {
                            Id = Guid.NewGuid(),
                            StartYear = 2021
                        },
                        new()
                        {
                            Id = Guid.NewGuid(),
                            StartYear = 2022
                        },
                        new()
                        {
                            Id = Guid.NewGuid(),
                            StartYear = 2023
                        },
                        new()
                        {
                            Id = Guid.NewGuid(),
                            StartYear = 2024
                        },
                        new()
                        {
                            Id = Guid.NewGuid(),
                            StartYear = 2025
                        },
                        new()
                        {
                            Id = Guid.NewGuid(),
                            StartYear = 2026
                        },
                        new()
                        {
                            Id = Guid.NewGuid(),
                            StartYear = 2027
                        },
                        new()
                        {
                            Id = Guid.NewGuid(),
                            StartYear = 2028
                        },
                        new()
                        {
                            Id = Guid.NewGuid(),
                            StartYear = 2029
                        }
                    }
                );

                _context.Set<AcademicYear>().AddRange(academicYears);
                _context.SaveChanges();
            }
        }
    }
}