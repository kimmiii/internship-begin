using StagebeheerAPI.Models;
using StagebeheerAPI.Models.ApiModels;
using System;
using System.Collections.Generic;

namespace StagebeheerAPI.Contracts
{
    public interface IInternshipRepository : IRepositoryBase<Internship>
    {
        public bool CompanyCanEditInternshipRequest(int internshipId);
        public bool AllowedToChangeProjectStatus(int internshipId, string projecStatusCode);

        public List<InternshipAssignedUser> GetAssignedUsers(int internshipId);

        public int GetCountTotalAssignedReviewers(int internshipId);
        public DateTime? GetSentToReviewersAt(int internshipId);
        public List<InternshipReviewer> GetHistoryOfAllReviewers(int internshipId);

        public string SerializeFeedback(List<Feedback> feedback);
        public List<Feedback> DeserializeFeedbackMessage(String feedbackJSON);

    }
}
