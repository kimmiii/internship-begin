using StagebeheerAPI.Models;
using System.Collections.Generic;

namespace StagebeheerAPI.FilterPattern
{
    public interface IFilter
    {
        public List<Internship> meetFilter(List<Internship> internships);
    }
}
