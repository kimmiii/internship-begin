using System.Collections.Generic;

namespace StagebeheerAPI.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public ICollection<User> Users { get; set; }

    }
}
