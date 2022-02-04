using StagebeheerAPI.Models;
using System.Collections.Generic;

namespace StagebeheerAPI.FilterPattern
{
    public class DescriptionFilter : IFilter
    {
        private string description;

        public DescriptionFilter(string description)
        {
            this.description = description;
        }

        public List<Internship> meetFilter(List<Internship> internships)
        {
            if (description == "")
            {
                return internships;
            } else
            {

                List<Internship> internshipByDescription = new List<Internship>();

                foreach (Internship internship in internships)
                {
                    if (internship.AssignmentDescription.ToUpper().Contains(description.ToUpper()) || internship.ResearchTopicTitle.ToUpper().Contains(description.ToUpper()))
                    {
                        internshipByDescription.Add(internship);
                    }
                }
                return internshipByDescription;
            }
        }
    }
}
