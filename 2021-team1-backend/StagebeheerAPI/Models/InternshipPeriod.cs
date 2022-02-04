namespace StagebeheerAPI.Models
{
    public partial class InternshipPeriod
    {
        public int InternshipId { get; set; }
        public int PeriodId { get; set; }

        public virtual Internship Internship { get; set; }
        public virtual Period Period { get; set; }
    }
}
