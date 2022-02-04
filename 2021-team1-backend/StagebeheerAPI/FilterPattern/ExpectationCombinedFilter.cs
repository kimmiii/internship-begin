using StagebeheerAPI.Models;
using System.Collections.Generic;

namespace StagebeheerAPI.FilterPattern
{
    public class ExpectationCombinedFilter : IFilter
    {
        private List<InternshipExpectation> expectationId;
        public ExpectationCombinedFilter(List<InternshipExpectation> expectationId)
        {
            this.expectationId = expectationId;

        }
        public List<Internship> meetFilter(List<Internship> internships)
        {
            List<IFilter> filters = new List<IFilter>();

            foreach (InternshipExpectation expid in expectationId)
            {
                ExpectationFilter filter = new ExpectationFilter(expid.ExpectationId);
                filters.Add(filter);
            }
            var expectationCombinedFilter = new OrFilters(filters);
            return expectationCombinedFilter.meetFilter(internships);
        }
    }
}

