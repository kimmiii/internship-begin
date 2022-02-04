using System.Collections.Generic;

namespace StagebeheerAPI.Models
{
    public class ProjectStatus
    {
        public int ProjectStatusId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public ICollection<Internship> Internships { get; set; }
    }
}
