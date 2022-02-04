using StagebeheerAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace StagebeheerAPI.FilterPattern
{
    public class UserInternshipsFilter : IFilter
    {
        private int studentId;
        public UserInternshipsFilter(int studentId)
        {
            this.studentId = studentId;
        }
        public List<Internship> meetFilter(List<Internship> internships)
        {
            List<Internship> userInternships = new List<Internship>();
            if (studentId == 0)
            {
                return internships;
            }
            else
            {
                foreach(Internship internship in internships)
                {
                    if (internship.UserInternships != null && internship.UserInternships.Any(e => e.UserId == studentId))
                    {
                        userInternships.Add(internship);
                    }
                }
                return userInternships;
            }
        }
    }
}
