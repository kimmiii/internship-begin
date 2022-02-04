using System.Collections.Generic;

namespace StagebeheerAPI.Models
{
    public class PagedInternshipsResult
    {
        public List<Internship> Internships { get; set; }
        public int Pages { get; set; }
    }
}
