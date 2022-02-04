using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;

namespace StagebeheerAPI.Repository
{
    public class InternshipAssignedUserRepository : RepositoryBase<InternshipAssignedUser>, IInternshipAssignedUserRepository
    {
        public InternshipAssignedUserRepository(StagebeheerDBContext repositoryContext)
            : base(repositoryContext) { }
    }
}
