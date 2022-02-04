using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventAPI.Domain.Models;

namespace EventAPI.Domain.ViewModels
{
    public class UserVM
    {
        public int UserId { get; set; }
        public String UserFirstName { get; set; }
        public String UserSurname { get; set; }
        public string UserEmailAddress { get; set; }
        public Role Role { get; set; }
        public int RoleId { get; set; }
    }
}
