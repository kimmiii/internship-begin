using EventAPI.Domain.Models;
using EventAPI.Domain.ViewModels;

namespace EventAPI.Tests.Builders
{
    public class AcademicYearVmBuilder
    {
        private readonly AcademicYearVM _academicYearVm;

        public AcademicYearVmBuilder()
        {
            _academicYearVm = new AcademicYearVM();
        }

        public AcademicYearVmBuilder FromAcademicYear(AcademicYear academicYear)
        {
            _academicYearVm.Id = academicYear.Id;
            _academicYearVm.Description = academicYear.Description;
            _academicYearVm.StartYear = academicYear.StartYear;
            _academicYearVm.EndYear = academicYear.EndYear;
            return this;
        }

        public AcademicYearVM Build => _academicYearVm;
    }
}
