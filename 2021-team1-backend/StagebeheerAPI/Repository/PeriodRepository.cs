using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;

namespace StagebeheerAPI.Repository
{
    public class PeriodRepository : RepositoryBase<Period>, IPeriodRepository
    {
        public PeriodRepository(StagebeheerDBContext repositoryContext)
            : base(repositoryContext) { }
    }
}
