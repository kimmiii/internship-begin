using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;

namespace StagebeheerAPI.Repository
{
    public class ExpectationRepository : RepositoryBase<Expectation>, IExpectationRepository
    {
        public ExpectationRepository(StagebeheerDBContext repositoryContext)
            : base(repositoryContext) { }
    }
}
