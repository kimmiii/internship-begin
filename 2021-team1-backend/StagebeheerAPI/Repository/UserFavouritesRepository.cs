using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;

namespace StagebeheerAPI.Repository
{
    public class UserFavouritesRepository : RepositoryBase<UserFavourites>, IUserFavouritesRepository
    {
        public UserFavouritesRepository(StagebeheerDBContext repositoryContext)
            : base(repositoryContext) { }
    }
}
