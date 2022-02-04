using System;
using System.Collections.Generic;

namespace StagebeheerAPI.Models
{
    public class User
    {

        public User()
        {
            InternshipAssignedUser = new HashSet<InternshipAssignedUser>();
            InternshipReviewer = new HashSet<InternshipReviewer>();
        }

        public int UserId { get; set; }
        public String UserFirstName { get; set; }
        public String UserSurname { get; set; }

        public String UserPass { get; set; }

        public String Salt { get; set; }

        public string UserEmailAddress { get; set; }

        public DateTime RegistrationDate { get; set; }

        public Boolean Activated { get; set; }

        public Boolean CvPresent { get; set; }

        public virtual ICollection<InternshipAssignedUser> InternshipAssignedUser { get; set; }
        public virtual ICollection<InternshipReviewer> InternshipReviewer { get; set; }
        public virtual ICollection<UserFavourites> UserFavourites { get; set; }
        public virtual ICollection<UserInternships> UserInternships { get; set; }

        public Role Role { get; set; }

        public int RoleId { get; set; }
        public Company Company { get; set; }
    }
}
