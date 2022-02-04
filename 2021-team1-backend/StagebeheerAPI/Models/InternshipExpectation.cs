namespace StagebeheerAPI.Models
{
    public partial class InternshipExpectation
    {
        public int InternshipId { get; set; }
        public int ExpectationId { get; set; }

        public virtual Internship Internship { get; set; }
        public virtual Expectation Expectation { get; set; }
    }
}
