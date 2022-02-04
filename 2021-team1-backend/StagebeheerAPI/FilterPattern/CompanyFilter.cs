using StagebeheerAPI.Models;
using System.Collections.Generic;

namespace StagebeheerAPI.FilterPattern
{
    public class CompanyFilter : IFilter
    {
        private int companyId;

        public CompanyFilter(int companyId)
        {
            this.companyId = companyId;  
        }

        public List<Internship> meetFilter(List<Internship> internships)
        {
            if (companyId == 0)
            {
                return internships;
            } else
            {

                List<Internship> internshipByCompany = new List<Internship>();

                foreach (Internship internship in internships)
                {
                    if (internship.CompanyId == companyId)
                    {
                        internshipByCompany.Add(internship);
                    }
                }
                return internshipByCompany;
            }
        }
    }
}
