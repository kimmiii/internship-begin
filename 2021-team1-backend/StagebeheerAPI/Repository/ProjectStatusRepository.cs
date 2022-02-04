using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;

namespace StagebeheerAPI.Repository
{
    public class ProjectStatusRepository : RepositoryBase<ProjectStatus>, IProjectStatusRepository
    {
        public ProjectStatusRepository(StagebeheerDBContext repositoryContext)
            : base(repositoryContext) { }
    }
}
