using System;

namespace EventAPI.Domain.ViewModels
{
    public class CompanyVM
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string HouseNr { get; set; }
        public string BusNr { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string VATNumber { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int TotalEmployees { get; set; }
        public int TotalITEmployees { get; set; }
        public int TotalITEmployeesActive { get; set; }
        public DateTime? EvaluatedAt { get; set; }
        public string EvaluationFeedback { get; set; }
        public bool Activated { get; set; }
        public int UserId { get; set; }
    }
}