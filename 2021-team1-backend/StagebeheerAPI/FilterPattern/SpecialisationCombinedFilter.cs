using StagebeheerAPI.Models;
using System.Collections.Generic;

namespace StagebeheerAPI.FilterPattern
{
    public class SpecialisationCombinedFilter : IFilter
    {
        private List<InternshipSpecialisation> specialisationId;
        public SpecialisationCombinedFilter(List<InternshipSpecialisation> specialisationId)
        {
            this.specialisationId = specialisationId;
        }
        public List<Internship> meetFilter(List<Internship> internships)
        {
            List<IFilter> filters = new List<IFilter>();

            foreach (InternshipSpecialisation specid in specialisationId)
            {
                SpecialisationFilter filter = new SpecialisationFilter(specid.SpecialisationId);
                filters.Add(filter);
            }
            var specialisationCombinedFilter = new OrFilters(filters);
            return specialisationCombinedFilter.meetFilter(internships);
        }
    }
}

