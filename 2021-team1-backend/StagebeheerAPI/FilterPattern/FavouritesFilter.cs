using StagebeheerAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace StagebeheerAPI.FilterPattern
{
    public class FavouritesFilter : IFilter
    {
        private int studentId;
        public FavouritesFilter(int studentId)
        {
            this.studentId = studentId;

        }
        public List<Internship> meetFilter(List<Internship> internships)
    {
        List<Internship> studentFavourites = new List<Internship>();
            if (studentId == 0)
            {
                return internships;
            }
            else
            {
                foreach (Internship internship in internships)
                {
                    if (internship.UserFavourites != null && internship.UserFavourites.Any(e => e.UserId == studentId))
                    {
                        studentFavourites.Add(internship);
                    }
                }
                return studentFavourites;
            }
        }
        
    }
}

