using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;

namespace StagebeheerAPI.Repository
{
    public class InternshipSpecialisationRepository : RepositoryBase<InternshipSpecialisation>, IInternshipSpecialisationRepository
    {
        public InternshipSpecialisationRepository(StagebeheerDBContext repositoryContext)
            : base(repositoryContext) { }
    }
}
