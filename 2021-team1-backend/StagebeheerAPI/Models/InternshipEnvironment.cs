namespace StagebeheerAPI.Models
{
    public partial class InternshipEnvironment
    {
        public int InternshipId { get; set; }
        public int EnvironmentId { get; set; }

        public virtual Internship Internship { get; set; }
        public virtual Environment Environment { get; set; }
    }
}
