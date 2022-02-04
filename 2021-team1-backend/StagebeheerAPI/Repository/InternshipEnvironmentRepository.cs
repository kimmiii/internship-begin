using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;

namespace StagebeheerAPI.Repository
{
    public class InternshipEnvironmentRepository : RepositoryBase<InternshipEnvironment>, IInternshipEnvironmentRepository
    {
        public InternshipEnvironmentRepository(StagebeheerDBContext repositoryContext)
            : base(repositoryContext) { }
    }
}
