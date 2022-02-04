namespace StagebeheerAPI.Models.ApiModels
{
    public class Feedback
    {
        public string MessageDT { get; set; }
        public string MessageBody { get; set; }
        public int UserFrom { get; set; }
        public string UserFromName { get; set; }
        public int UserTo { get; set; }
        public string UserToName { get; set; }
    }
}
