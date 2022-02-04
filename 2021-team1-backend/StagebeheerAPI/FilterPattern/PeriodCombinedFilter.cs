using StagebeheerAPI.Models;
using System.Collections.Generic;

namespace StagebeheerAPI.FilterPattern
{
    public class PeriodCombinedFilter : IFilter
    {
        private List<InternshipPeriod> periodId;
        public PeriodCombinedFilter(List<InternshipPeriod> periodId)
        {
            this.periodId = periodId;
        }
        public List<Internship> meetFilter(List<Internship> internships)
        {
            List<IFilter> filters = new List<IFilter>();

            foreach (InternshipPeriod perid in periodId)
            {
                PeriodFilter filter = new PeriodFilter(perid.PeriodId);
                filters.Add(filter);
            }
            var periodCombinedFilter = new OrFilters(filters);
            return periodCombinedFilter.meetFilter(internships);
        }
    }
}

