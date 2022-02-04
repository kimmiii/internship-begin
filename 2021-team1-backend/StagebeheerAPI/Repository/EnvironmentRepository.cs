using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;

namespace StagebeheerAPI.Repository
{
    public class EnvironmentRepository : RepositoryBase<Models.Environment>, IEnvironmentRepository
    {
        public EnvironmentRepository(StagebeheerDBContext repositoryContext)
            : base(repositoryContext) { }
    }
}
