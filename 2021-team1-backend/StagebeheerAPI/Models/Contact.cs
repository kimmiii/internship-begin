namespace StagebeheerAPI.Models
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
        public bool Activated { get; set; } = true;

        public Company Company { get; set; }
    }
}
