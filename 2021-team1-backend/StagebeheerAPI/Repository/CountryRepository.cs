using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;

namespace StagebeheerAPI.Repository
{
    public class CountryRepository : RepositoryBase<Country>, ICountryRepository
    {
        public CountryRepository(StagebeheerDBContext repositoryContext)
            : base(repositoryContext) { }
    }
}
