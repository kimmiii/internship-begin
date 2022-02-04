using System.Collections.Generic;

namespace StagebeheerAPI.Models
{
    public partial class Expectation
    {
        public Expectation()
        {
            InternshipExpectation = new HashSet<InternshipExpectation>();
        }

        public int ExpectationId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public virtual ICollection<InternshipExpectation> InternshipExpectation { get; set; }
    }
}
