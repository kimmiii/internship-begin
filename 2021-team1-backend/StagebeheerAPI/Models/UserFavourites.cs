namespace StagebeheerAPI.Models
{
    public partial class UserFavourites
    {
        public int InternshipId { get; set; }
        public int UserId { get; set; }

        public virtual Internship Internship { get; set; }
        public virtual User User { get; set; }
    }
}
