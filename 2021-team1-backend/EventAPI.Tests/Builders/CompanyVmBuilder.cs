using EventAPI.Domain.Models;
using EventAPI.Domain.ViewModels;

namespace EventAPI.Tests.Builders
{
    public class CompanyVmBuilder
    {
        private readonly CompanyVM _companyVm;

        public CompanyVmBuilder()
        {
            _companyVm = new CompanyVM();
        }

        public CompanyVmBuilder FromCompany(Company company)
        {
            _companyVm.CompanyId = company.CompanyId;
            _companyVm.Name = company.Name;
            return this;
        }

        public CompanyVM Build => _companyVm;
    }
}
