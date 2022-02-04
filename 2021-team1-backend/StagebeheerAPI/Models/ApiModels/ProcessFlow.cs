namespace StagebeheerAPI.Models.ApiModels
{
    public class ProcessFlow
    {
        public string StatusFrom { get; set; }
        public string RoleFrom { get; set; }
        public string StatusTo { get; set; }
        public string RoleTo { get; set; }
        public bool FlowAllowed { get; set; }
        public bool SendMail { get; set; }

        public string MailSubject { get; set; }
        public bool AssignUserRequired { get; set; } 
        

    }
}
