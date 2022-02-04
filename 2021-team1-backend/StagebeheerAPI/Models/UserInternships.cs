using System;

namespace StagebeheerAPI.Models
{
    public class UserInternships
    {
        public int InternshipId { get; set; }
        public int UserId { get; set; }
        public bool HireRequested { get; set; }
        public bool HireConfirmed { get; set; }
        public bool HireApproved { get; set; }
        public DateTime? EvaluatedAt { get; set; }
        public string RejectionFeedback { get; set; }
        public bool Interesting { get; set; }

        public virtual Internship Internship { get; set; }
        public virtual User User { get; set; }
    }
}
