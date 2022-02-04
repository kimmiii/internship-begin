using StagebeheerAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace StagebeheerAPI.FilterPattern
{
    public class SpecialisationFilter : IFilter
    {
        private int specialisationId;
        public SpecialisationFilter(int specialisationId)
        {
            this.specialisationId = specialisationId;
        }
        public List<Internship> meetFilter(List<Internship> internships)
        {
        List<Internship> internshipBySpecialisation = new List<Internship>();

        foreach (Internship internship in internships)
        {
            if (internship.InternshipSpecialisation.Any(e => e.SpecialisationId == specialisationId))
            {
                internshipBySpecialisation.Add(internship);
            }
        }
        return internshipBySpecialisation;
    }
}
}
