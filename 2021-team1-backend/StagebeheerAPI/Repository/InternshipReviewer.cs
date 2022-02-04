using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;

namespace StagebeheerAPI.Repository
{
    public class InternshipReviewerRepository : RepositoryBase<InternshipReviewer>, IInternshipReviewerRepository
    {
        public InternshipReviewerRepository(StagebeheerDBContext repositoryContext)
            : base(repositoryContext) { }
    }
}
