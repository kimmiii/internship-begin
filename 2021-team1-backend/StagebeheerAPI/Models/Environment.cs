using System.Collections.Generic;

namespace StagebeheerAPI.Models
{
    public partial class Environment
    {
        public Environment()
        {
            InternshipEnvironment = new HashSet<InternshipEnvironment>();
        }
        public int EnvironmentId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public virtual ICollection<InternshipEnvironment> InternshipEnvironment { get; set; }

    }
}
