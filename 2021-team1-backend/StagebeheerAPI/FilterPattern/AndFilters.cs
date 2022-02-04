using StagebeheerAPI.Models;
using System.Collections.Generic;

namespace StagebeheerAPI.FilterPattern
{
    public class AndFilters : IFilter
    {
        private List<IFilter> filters;
        public AndFilters(List<IFilter> filters)
        {
            this.filters = filters;
            //this.otherFilter = otherFilter;
            //return meetFilter(internships);
        }
        public List<Internship> meetFilter(List<Internship> internships)
        { 
            List<Internship> internshipsFirstFiltering = new List<Internship>();

            foreach(IFilter filter in filters)
            {
                //if (filter is FavouritesFilter)
                //{
                //    internshipsFirstFiltering = filter.meetFilter(internships);
                //}
                //else
                //{
                //    internships = filter.meetFilter(internships);
                //}
                internships = filter.meetFilter(internships);
            }

            //if (internshipsFirstFiltering != null)
            //{
            //    bool internshipsExistsInList = false;

            //    foreach (Internship internshipFirstFiltering in internshipsFirstFiltering)
            //    {
            //        foreach (Internship internship in internships)
            //        {
            //            if (internshipFirstFiltering.InternshipId == internship.InternshipId)
            //            {
            //                internshipsExistsInList = true;
            //            }
            //        }

            //        if (!internshipsExistsInList)
            //        {
            //            internships.Add(internshipFirstFiltering);
            //        }

            //        internshipsExistsInList = false;
            //    }
            //}

            return internships;
        }
    }
}
