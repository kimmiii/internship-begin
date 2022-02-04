using StagebeheerAPI.Models;

namespace StagebeheerAPI.Contracts
{
    public interface ICompanyRepository : IRepositoryBase<Company>
    {
        public bool IsValidVatNumber(string companyId);        
    }
}
