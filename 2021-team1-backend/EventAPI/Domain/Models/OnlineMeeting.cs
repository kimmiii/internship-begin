using System;

namespace EventAPI.Domain.Models
{
    public class OnlineMeeting
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string JoinWebUrl { get; set; }
    }
}
