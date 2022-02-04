using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;

namespace StagebeheerAPI.Repository
{
    public class SpecialisationRepository : RepositoryBase<Specialisation>, ISpecialisationRepository
    {
        public SpecialisationRepository(StagebeheerDBContext repositoryContext)
            : base(repositoryContext) { }
    }
}
