namespace StagebeheerAPI.Models
{
    public partial class InternshipSpecialisation
    {
        public int InternshipId { get; set; }
        public int SpecialisationId { get; set; }

        public virtual Internship Internship { get; set; }
        public virtual Specialisation Specialisation { get; set; }
    }
}
