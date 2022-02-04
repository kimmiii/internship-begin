using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;
using StagebeheerAPI.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StagebeheerAPI.Repository
{
    public class InternshipRepository : RepositoryBase<Internship>, IInternshipRepository
    {
        public InternshipRepository(StagebeheerDBContext repositoryContext)
            : base(repositoryContext) 
        { 
            
        
        }


        //Checks whether Company is still able to modify request
        //POST company only possible when status in (NEW,FEE)
        public bool CompanyCanEditInternshipRequest(int internshipId)
        {

            var internship = FindByCondition(x => x.InternshipId == internshipId)
            .Include(ps => ps.ProjectStatus).FirstOrDefault();

            if ((internship.ProjectStatus.Code.ToUpper().Equals("NEW")) || (internship.ProjectStatus.Code.ToUpper().Equals("FEE")))
            {
                return true;
            }
            else
            {
                return false;
            }    
        }

        public bool AllowedToChangeProjectStatus(int internshipId, string projecStatusCode)
        {
            return true;
        }

        public List<InternshipAssignedUser> GetAssignedUsers(int internshipId)
        {
            Internship internship = FindByCondition(x => x.InternshipId == internshipId)
            .Include(ia => ia.InternshipAssignedUser).FirstOrDefault();

            return internship.InternshipAssignedUser.ToList();
        }

        public int GetCountTotalAssignedReviewers(int internshipId)
        {
            Internship internship = FindByCondition(x => x.InternshipId == internshipId).FirstOrDefault();

            return internship.CountTotalAssignedReviewers;
        }

        public DateTime? GetSentToReviewersAt(int internshipId)
        {
            Internship internship = FindByCondition(x => x.InternshipId == internshipId).FirstOrDefault();

            return internship.SentToReviewersAt;
        }

        public List<InternshipReviewer> GetHistoryOfAllReviewers(int internshipId)
        {
            Internship internship = FindByCondition(x => x.InternshipId == internshipId)
            .Include(rev => rev.InternshipReviewer).FirstOrDefault();

            return internship.InternshipReviewer.ToList();
        }


        public string SerializeFeedback(List<Feedback> feedback)
        {
            return JsonConvert.SerializeObject(feedback);
        }

        public List<Feedback> DeserializeFeedbackMessage(String feedbackJSON)
        {
            return JsonConvert.DeserializeObject<List<Feedback>>(feedbackJSON);
        }



    }
}
