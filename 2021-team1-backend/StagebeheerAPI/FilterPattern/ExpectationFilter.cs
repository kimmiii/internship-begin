using StagebeheerAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace StagebeheerAPI.FilterPattern
{
    public class ExpectationFilter : IFilter
    {
        private int expectationId;
        public ExpectationFilter(int expectationId)
        {
            this.expectationId = expectationId;
        }

        public List<Internship> meetFilter(List<Internship> internships)
        {
        List<Internship> internshipByExpectation = new List<Internship>();

        foreach (Internship internship in internships)
        {
            if (internship.InternshipExpectation != null && internship.InternshipExpectation.Any(e => e.ExpectationId == expectationId))
            {
                internshipByExpectation.Add(internship);
            }
        }
        return internshipByExpectation;
    }
}
}
