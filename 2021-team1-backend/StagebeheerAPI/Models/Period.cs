using System.Collections.Generic;

namespace StagebeheerAPI.Models
{
    public partial class Period
    {
        public Period()
        {
            InternshipPeriod = new HashSet<InternshipPeriod>();
        }

        public int PeriodId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public virtual ICollection<InternshipPeriod> InternshipPeriod { get; set; }
    }
}
