using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventAPI.Domain.Models
{
    public class Contact
    {
        public int ContactId { get; set; }
        public int CompanyId { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Function { get; set; }
        public bool Activated { get; set; }
    }
}
