using EventAPI.Domain.Models;
using System;

namespace EventAPI.Tests.Builders
{
    public class InternshipBuilder
    {
        private readonly Internship _internship;

        public InternshipBuilder()
        {
            _internship = new Internship
            {
                InternshipId = new Random().Next(),
                WpStreet = Guid.NewGuid().ToString(),
            };
        }

        public InternshipBuilder WithCompanyId(int id)
        {
            _internship.CompanyId = id;
            return this;
        }

        public InternshipBuilder WithAcademicYear(string academicYear)
        {
            _internship.AcademicYear = academicYear;
            return this;
        }

        public Internship Build => _internship;
        
    }
}
