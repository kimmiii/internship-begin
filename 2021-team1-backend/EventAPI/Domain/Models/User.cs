using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventAPI.Domain.Models
{
    public class User
    {
        public int UserId { get; set; }
        public String UserFirstName { get; set; }
        public String UserSurname { get; set; }

        public String UserPass { get; set; }

        public String Salt { get; set; }

        public string UserEmailAddress { get; set; }

        public DateTime RegistrationDate { get; set; }

        public Boolean Activated { get; set; }

        public Boolean CvPresent { get; set; }
        
        public Role Role { get; set; }

        public int RoleId { get; set; }
        public Company Company { get; set; }
    }
}
