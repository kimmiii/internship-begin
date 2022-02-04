using StagebeheerAPI.Models;
using System.Collections.Generic;

namespace StagebeheerAPI.FilterPattern
{
    public class EnvironmentCombinedFilter : IFilter
    {
        private List<InternshipEnvironment> environmentId;
        public EnvironmentCombinedFilter(List<InternshipEnvironment> environmentId)
        {
            this.environmentId = environmentId;

        }
        public List<Internship> meetFilter(List<Internship> internships)
        {
            List<IFilter> filters = new List<IFilter>();

            foreach (InternshipEnvironment envid in environmentId)
            {
                EnvironmentFilter filter = new EnvironmentFilter(envid.EnvironmentId);
                filters.Add(filter);
            }
            var environmentCombinedFilter = new OrFilters(filters);
            return environmentCombinedFilter.meetFilter(internships);
        }
    }
}

