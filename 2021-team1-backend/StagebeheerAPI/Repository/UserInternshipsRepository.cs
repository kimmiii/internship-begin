using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;

namespace StagebeheerAPI.Repository
{
    public class UserInternshipsRepository : RepositoryBase<UserInternships>, IUserInternshipsRepository
    {
        public UserInternshipsRepository(StagebeheerDBContext repositoryContext)
        : base(repositoryContext) { }
    }
}
