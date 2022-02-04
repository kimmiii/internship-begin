using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;

namespace StagebeheerAPI.Repository
{
    public class InternshipExpectationRepository : RepositoryBase<InternshipExpectation>, IInternshipExpectationRepository
    {
        public InternshipExpectationRepository(StagebeheerDBContext repositoryContext)
            : base(repositoryContext) { }
    }
}