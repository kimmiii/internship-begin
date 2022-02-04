using System.Collections.Generic;
using EventAPI.Domain.Models;

namespace EventAPI.Tests.Builders
{
    public class InternshipListBuilder
    {
        private readonly List<Internship> _internships;

        public InternshipListBuilder()
        {
            _internships = new List<Internship>();
        }

        public InternshipListBuilder WithCompanyId(int id)
        {
            for (var i = 0; i < 3; i++)
            {
                _internships.Add(new InternshipBuilder().WithCompanyId(id).Build);
            }
            return this;
        }

        public InternshipListBuilder WithAcademicYear(string academicYear)
        {
            for (var i = 0; i < 3; i++)
            {
                _internships.Add(new InternshipBuilder().WithAcademicYear(academicYear).Build);
            }
            return this;
        }

        public List<Internship>  Build => _internships;
    }
}