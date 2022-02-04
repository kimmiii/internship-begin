using System.Collections.Generic;
using EventAPI.Domain.Models;
using EventAPI.Domain.ViewModels;

namespace EventAPI.Tests.Builders
{
    public class InternshipVMListBuilder
    {
        private readonly List<InternshipVM> _internships;

        public InternshipVMListBuilder()
        {
            _internships = new List<InternshipVM>();
        }

        public InternshipVMListBuilder WithCompanyId(int id)
        {
            for (var i = 0; i < 3; i++)
            {
                _internships.Add(new InternshipVmBuilder().WithCompanyId(id).Build);
            }
            return this;
        }

        public InternshipVMListBuilder WithAcademicYear(string academicYear)
        {
            for (var i = 0; i < 3; i++)
            {
                _internships.Add(new InternshipVmBuilder().WithAcademicYear(academicYear).Build);
            }
            return this;
        }

        public List<InternshipVM>  Build => _internships;
    }
}