using EventAPI.Domain.ViewModels;
using System;
using EventAPI.Domain.Models;

namespace EventAPI.Tests.Builders
{
    public class InternshipVmBuilder
    {
        private readonly InternshipVM _internshipVM;

        public InternshipVmBuilder()
        {
            _internshipVM = new InternshipVM
            {
                WpStreet = Guid.NewGuid().ToString(),
            };
        }

        public InternshipVmBuilder WithCompanyId(int id)
        {
            _internshipVM.CompanyId = id;
            return this;
        }

        public InternshipVmBuilder WithAcademicYear(string academicYear)
        {
            _internshipVM.AcademicYear = academicYear;
            return this;
        }


        public InternshipVmBuilder FromInternship(Internship internship)
        {
            _internshipVM.InternshipId = internship.InternshipId;
            _internshipVM.CompanyId = internship.CompanyId;
            _internshipVM.ProjectStatusId = internship.ProjectStatusId;
            _internshipVM.WpStreet = internship.WpStreet;
            _internshipVM.WpHouseNr = internship.WpHouseNr;
            _internshipVM.WpBusNr = internship.WpBusNr;
            _internshipVM.WpZipCode = internship.WpZipCode;
            _internshipVM.WpCity = internship.WpCity;
            _internshipVM.WpCountry = internship.WpCountry;

            return this;
        }


        public InternshipVM Build => _internshipVM;
    }
}
