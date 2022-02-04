using System.Collections.Generic;

namespace StagebeheerAPI.Models
{
    public class Specialisation
    {
        public Specialisation()
        {
            InternshipSpecialisation = new HashSet<InternshipSpecialisation>();
        }
        public int SpecialisationId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Hyperlink { get; set; }

        public virtual ICollection<InternshipSpecialisation> InternshipSpecialisation { get; set; }
    }
}
