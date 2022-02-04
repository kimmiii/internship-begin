using System;
using System.Collections.Generic;

namespace StagebeheerAPI.Models.ApiModels
{
    public class User
    {
        public int UserId { get; set; }
        public String UserFirstName { get; set; }
        public String UserSurname { get; set; }
        public string UserEmailAddress { get; set; }
        public int RoleId { get; set; }
        public string RoleCode { get; set; }
        public string RoleDescription { get; set; }
        public bool IsUserActivated { get; set; }
        public bool CvPresent { get; set; }
        public int CompanyId { get; set; }
        public bool IsCompanyActivated { get; set; }
        public virtual ICollection<InternshipAssignedUser> InternshipAssignedUser { get; set; }
        public string Token { get; set; }

    }
}
