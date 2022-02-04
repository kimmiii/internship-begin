using StagebeheerAPI.Models;
using System.Collections.Generic;

namespace StagebeheerAPI.FilterPattern
{
    public class OtherEnvironmentsFilter : IFilter
    {
        private string otherEnvironments;

        public OtherEnvironmentsFilter(string otherEnvironments)
        {
            this.otherEnvironments = otherEnvironments;
        }

        public List<Internship> meetFilter(List<Internship> internships)
        {
            if (otherEnvironments == "")
            {
                return internships;
            } else
            {

                List<Internship> internshipByOtherEnvironment = new List<Internship>();

                foreach (Internship internship in internships)
                {
                    if (internship.InternshipEnvironmentOthers!= null && internship.InternshipEnvironmentOthers.ToUpper().Contains(otherEnvironments.ToUpper()))
                    {
                        internshipByOtherEnvironment.Add(internship);
                    }
                }
                return internshipByOtherEnvironment;
            }
        }
    }
}
