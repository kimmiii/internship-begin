using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;
using System.Linq;

namespace StagebeheerAPI.Repository
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(StagebeheerDBContext repositoryContext)
            : base(repositoryContext) { }

        public bool IsValidVatNumber(string companyVATNumber)
        {
            Company company = FindByCondition(x => x.VATNumber == companyVATNumber).FirstOrDefault();
            return company == null ? true : false;
        }
    }
}
