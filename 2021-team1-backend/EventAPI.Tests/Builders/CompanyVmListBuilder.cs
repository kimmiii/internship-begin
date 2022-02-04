using System.Collections.Generic;
using EventAPI.Domain.Models;
using EventAPI.Domain.ViewModels;

namespace EventAPI.Tests.Builders
{
    public class CompanyVmListBuilder
    {
        private readonly IList<CompanyVM> _companyVms;

        public CompanyVmListBuilder()
        {
            _companyVms = new List<CompanyVM>();
        }

        public CompanyVmListBuilder FromCompany(Company company)
        {
            for (var i = 0; i < 3; i++)
            {
                _companyVms.Add(new CompanyVmBuilder().FromCompany(company).Build);
            }
            return this;
        }

        public IList<CompanyVM> Build => _companyVms;
    }
}
