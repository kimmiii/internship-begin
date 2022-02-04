using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;


namespace StagebeheerAPI.Repository
{
    public class InternshipPeriodRepository : RepositoryBase<InternshipPeriod>, IInternshipPeriodRepository
    {
        public InternshipPeriodRepository(StagebeheerDBContext repositoryContext)
            : base(repositoryContext) { }
    }
}
