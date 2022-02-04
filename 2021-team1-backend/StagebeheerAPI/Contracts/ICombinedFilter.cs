using StagebeheerAPI.Models;
using System.Collections.Generic;

namespace StagebeheerAPI.Contracts
{
    public interface ICombinedFilter
    {
        public List<Internship> comboFiltering(List<Internship> internships, Internship criteria);

    }
}
