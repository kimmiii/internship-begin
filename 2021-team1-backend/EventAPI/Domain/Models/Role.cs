using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventAPI.Domain.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
