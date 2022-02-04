using StagebeheerAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace StagebeheerAPI.FilterPattern
{
    public class OrFilters : IFilter
    {
        private List<IFilter> filters;
        
        public OrFilters(List<IFilter> filters)
        {
            this.filters = filters;
        }
        public List<Internship> meetFilter(List<Internship> internships)
        {
            if(filters.Count() < 1)
            {
                return internships;
            }
            
            List<Internship> internshipsFirstFiltering = new List<Internship>();
            List<Internship> internshipsOrFiltering = new List<Internship>();

            for(int i = 0; i < filters.Count(); i++)
            {
                internshipsFirstFiltering = filters[i].meetFilter(internships);
                
                foreach (Internship internship in internshipsFirstFiltering)
                {
                    if (!internshipsOrFiltering.Contains(internship))
                    {
                        internshipsOrFiltering.Add(internship);
                    }
                }
            }           
            return internshipsOrFiltering;
        }
    }
}
