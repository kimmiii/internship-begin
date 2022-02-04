using StagebeheerAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace StagebeheerAPI.FilterPattern
{
    public class EnvironmentFilter : IFilter
    {
        private int environmentId;
        public EnvironmentFilter(int environmentId)
        {
            this.environmentId = environmentId;

        }
        public List<Internship> meetFilter(List<Internship> internships)
    {
        List<Internship> internshipByEnvironment = new List<Internship>();

            foreach (Internship internship in internships)
                {
                    if (internship.InternshipEnvironment.Any(e => e.EnvironmentId == environmentId))
                    {
                        internshipByEnvironment.Add(internship);
                    }
                }
            return internshipByEnvironment;
        }
        
    }
}

