using StagebeheerAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace StagebeheerAPI.FilterPattern
{
    public class PeriodFilter : IFilter
    {
        private int periodId;
        public PeriodFilter(int periodId)
        {
            this.periodId = periodId;
        }
        public List<Internship> meetFilter(List<Internship> internships)
        {
        List<Internship> internshipByPeriod = new List<Internship>();

        foreach (Internship internship in internships)
        {
            if (internship.InternshipPeriod.Any(e => e.PeriodId == periodId))
            {
                internshipByPeriod.Add(internship);
            }
        }
        return internshipByPeriod;
    }
}
}
