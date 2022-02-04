namespace StagebeheerAPI.Models.ApiModels
{
    public class GetApprovedInternshipCriteria
    {
        public PageCriteria PageCriteria { get; set; }
        public Internship FilterCriteria { get; set; }
    }
}
