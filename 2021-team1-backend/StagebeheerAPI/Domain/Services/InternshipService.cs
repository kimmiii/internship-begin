using EmailService;
using Microsoft.EntityFrameworkCore;
using StagebeheerAPI.Configuration.Constants;
using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;
using StagebeheerAPI.Models.ApiModels;
using StagebeheerAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StagebeheerAPI.Domain.Services
{
    public class InternshipService
    {
        private IRepositoryWrapper _repoWrapper;
        private IEmailSender emailSender;

        public InternshipService(IRepositoryWrapper repositoryWrapper, IEmailSender emailSender) //(IInternshipRepository internshipRepository)
        {
            _repoWrapper = repositoryWrapper;
            this.emailSender = emailSender;
        }

        public async Task<Result> ChangeChangeProjectStatus(Internship internship)
        {
            List<string> mailTo = new List<string>();
            var result = new Result();
            int userTo = 0;

            string toRoleCode = "";
            string fromRoleCode = "";
            List<InternshipAssignedUser> internshipAssignedUser = null;

            fromRoleCode = _repoWrapper.User.returnRoleCode(internship.ContactPersonId);

            //special case
            //IF REV->REV and sender=REV
            if (fromRoleCode.Equals("REV"))
            {
                //assignedTo=empty
                //remove him from assignedTo
                //if assignedTo>0
                //then stay with 
                //reviewer
                //else
                //push to COO

                List<InternshipAssignedUser> currentAssignedUsers = new List<InternshipAssignedUser>();
                currentAssignedUsers = _repoWrapper.Internship.GetAssignedUsers(internship.InternshipId);
                currentAssignedUsers.Remove(currentAssignedUsers.Find(currUser => currUser.UserId == internship.ContactPersonId));

                if (currentAssignedUsers.Count() > 0)
                {
                    internship.InternshipAssignedUser = currentAssignedUsers;
                }
                else
                {
                    //TO COO
                    Role roleCOO = _repoWrapper.Role.FindByCondition(ro => ro.Code == "COO").FirstOrDefault();
                    int userCOOId = _repoWrapper.User.FindByCondition(us => us.RoleId == roleCOO.RoleId).FirstOrDefault().UserId;
                    InternshipAssignedUser au = new InternshipAssignedUser();
                    au.UserId = userCOOId;
                    internship.InternshipAssignedUser.Add(au);
                }
            }

            if (internship.InternshipAssignedUser.Count > 0)
            {
                internshipAssignedUser = internship.InternshipAssignedUser.ToList();
                toRoleCode = _repoWrapper.User.returnRoleCode(internshipAssignedUser[0].UserId);
                userTo = internshipAssignedUser[0].UserId;
            }

            if (fromRoleCode.Equals("COO") && toRoleCode.Equals("REV"))
            {
                internship.CountTotalAssignedReviewers = internship.InternshipAssignedUser.Count;
                internship.SentToReviewersAt = DateTime.Now;
            }

            if (fromRoleCode.Equals("REV") && toRoleCode.Equals("REV"))
            {
                int countTotalAssignedReviewers = _repoWrapper.Internship.GetCountTotalAssignedReviewers(internship.InternshipId);
                internship.CountTotalAssignedReviewers = countTotalAssignedReviewers;

                DateTime? sentToReviewersAt = _repoWrapper.Internship.GetSentToReviewersAt(internship.InternshipId);
                internship.SentToReviewersAt = sentToReviewersAt;
            }

            var processFlow = CheckProcessFlow(internship, fromRoleCode, toRoleCode);
            if (processFlow != null)
            {
                if (!processFlow.FlowAllowed)
                {
                    result.Status = Status.Error;
                    result.Message = "Status wijziging mislukt (ERR_BLOCKED)";
                    return result;
                }
            }
            else
            {
                result.Status = Status.Error;
                result.Message = "Status wijziging mislukt (ERR_NOTDEFINED)";

                StringBuilder mailBody = new StringBuilder(result.Message);
                mailBody.Append("<br> STATUS TO   :" + internship.ProjectStatus.Code);
                mailBody.Append("<br> ROLE FROM :" + fromRoleCode);
                mailBody.Append("<br> STATUS FROM :" + internship.ContactPersonId);

                var message = new Message(new string[] { "" }, "ERROR" + " (PXL ID:" + internship.InternshipId + ")", mailBody.ToString(), null);
                emailSender.SendEmail(message);

                return result;
            }

            List<InternshipReviewer> InternshipReviewers = new List<InternshipReviewer>();
            if (processFlow.RoleFrom.Equals("COO") && processFlow.RoleTo.Equals("REV"))
            {
                //the reviewers
                //add if not exists
                InternshipReviewers = _repoWrapper.Internship.GetHistoryOfAllReviewers(internship.InternshipId);

                foreach (var mentor in internship.InternshipAssignedUser)
                {
                    if (InternshipReviewers.FindIndex(x => x.UserId == mentor.UserId) == -1)
                    {
                        //add
                        InternshipReviewer internshipReviewer = new InternshipReviewer();
                        internshipReviewer.UserId = mentor.UserId;
                        InternshipReviewers.Add(internshipReviewer);
                    }
                }
            }

            var existingInternship = _repoWrapper.Internship.FindByCondition(x => x.InternshipId == internship.InternshipId).FirstOrDefault();
            var projectStatus = _repoWrapper.ProjectStatus.FindByCondition(ps => ps.Code == internship.ProjectStatus.Code);
            existingInternship.ProjectStatus = projectStatus.FirstOrDefault();

            existingInternship.InternshipReviewer = InternshipReviewers;

            List<Feedback> listFeedback = new List<Feedback>();
            if (internship.InternalFeedback != null)
            {
                string storedInternalFeedback = existingInternship.InternalFeedback;
                if (!string.IsNullOrEmpty(storedInternalFeedback))
                {
                    listFeedback = _repoWrapper.Internship.DeserializeFeedbackMessage(storedInternalFeedback);
                }
                listFeedback.Add
                (
                    new Feedback
                    {
                        MessageDT = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        MessageBody = internship.InternalFeedback,
                        UserFrom = internship.ContactPersonId,
                        UserFromName = _repoWrapper.User.ReturnUserName(internship.ContactPersonId),
                        UserTo = userTo,
                        UserToName = userTo > 0 ? _repoWrapper.User.ReturnUserName(userTo) : ""
                    }
                );

                existingInternship.InternalFeedback = _repoWrapper.Internship.SerializeFeedback(listFeedback);
            }
            if (internship.ExternalFeedback != null)
            {
                listFeedback.Clear();

                string storedExternalFeedback = existingInternship.ExternalFeedback;
                if (!string.IsNullOrEmpty(storedExternalFeedback))
                {
                    listFeedback = _repoWrapper.Internship.DeserializeFeedbackMessage(storedExternalFeedback);
                }


                listFeedback.Add
                (
                    new Feedback
                    {
                        MessageDT = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        MessageBody = internship.ExternalFeedback,
                        UserFrom = internship.ContactPersonId,
                        UserFromName = _repoWrapper.User.ReturnUserName(internship.ContactPersonId),
                        UserTo = userTo,
                        UserToName = userTo > 0 ? _repoWrapper.User.ReturnUserName(userTo) : ""
                    }
                );

                existingInternship.ExternalFeedback = _repoWrapper.Internship.SerializeFeedback(listFeedback);
            }

            var oldAssignedUsers = _repoWrapper.InternshipAssignedUser.FindByCondition(item => item.InternshipId == internship.InternshipId).ToList();
            _repoWrapper.InternshipAssignedUser.DeleteRange(oldAssignedUsers);

            if (internship.InternshipAssignedUser.Count == 0)
            {
                if (processFlow.RoleTo.Equals("COO"))
                {
                    //need to set it manually to the COO
                    Role roleCOO = _repoWrapper.Role.FindByCondition(ro => ro.Code == "COO").FirstOrDefault();
                    int userCOOId = _repoWrapper.User.FindByCondition(us => us.RoleId == roleCOO.RoleId).FirstOrDefault().UserId;
                    InternshipAssignedUser au = new InternshipAssignedUser();
                    au.UserId = userCOOId;
                    existingInternship.InternshipAssignedUser.Add(au);

                }
            }
            //COO Assign to me
            else if ((internship.InternshipAssignedUser.Count == 1) && (processFlow.RoleTo.Equals("COO")) && (processFlow.RoleFrom.Equals("COO")))
            {
                //Role roleCOO = _repoWrapper.Role.FindByCondition(ro => ro.Code == "COO").FirstOrDefault();
                //int userCOOId = _repoWrapper.User.FindByCondition(us => us.RoleId == roleCOO.RoleId).FirstOrDefault().UserId;
                
                InternshipAssignedUser au = new InternshipAssignedUser();
                au.UserId = internship.ContactPersonId;
                existingInternship.InternshipAssignedUser.Add(au);
            }
            else
            {
                existingInternship.InternshipAssignedUser = internship.InternshipAssignedUser;
                existingInternship.CountTotalAssignedReviewers = internship.CountTotalAssignedReviewers;
                existingInternship.SentToReviewersAt = internship.SentToReviewersAt;
            }

            _repoWrapper.Internship.Update(existingInternship);
            _repoWrapper.Save();

            if (processFlow.SendMail)
            {
                Contact contact = null;
                Models.User user = null; //ambiguity between Model & APIModel

                if (processFlow.RoleTo.Equals("COM"))
                {
                    int currContantId = existingInternship.ContactPersonId;
                    contact = _repoWrapper.Contact.FindByCondition(ct => ct.ContactId == currContantId).FirstOrDefault();

                    if (contact != null)
                    {
                        mailTo.Add(contact.Email);
                    }
                }
                else
                {
                    foreach (InternshipAssignedUser auser in internship.InternshipAssignedUser)
                    {
                        user = _repoWrapper.User.FindByCondition(ct => ct.UserId == auser.UserId).FirstOrDefault();
                        mailTo.Add(user.UserEmailAddress);
                    }
                }

                try
                {
                    var body = "";

                    var textReview = EmailMessages.internshipReviewAskedBody(existingInternship.ResearchTopicTitle, internship.InternalFeedback);

                    if (processFlow.RoleTo.Equals("COM"))
                    {
                        var textApproved = EmailMessages.internshipApproved(existingInternship.ResearchTopicTitle);
                        var textRejected = EmailMessages.internshipRejected(existingInternship.ResearchTopicTitle);
                        var textMoreInfo = EmailMessages.internshipMoreInfo(existingInternship.ResearchTopicTitle);

                        if (internship.ProjectStatus.Code == "APP")
                        {
                            body = textApproved;
                        } else if (internship.ProjectStatus.Code == "REJ")
                        {
                            body = textRejected + $"<span style=\"font-style: italic;\">{internship.ExternalFeedback.Replace("\n", "<br>")}</span>";
                        } else if (internship.ProjectStatus.Code == "FEE")
                        {
                            body = textMoreInfo + $"<span style=\"font-style: italic;\">{internship.ExternalFeedback.Replace("\n", "<br>")}</span>";
                        }
                    }
                    else if (processFlow.RoleTo.Equals("REV"))
                    {
                        body = textReview;
                    }

                    var message = new Message(mailTo, processFlow.MailSubject + " (PXL ID:" + internship.InternshipId + ")", body, null);
                    emailSender.SendEmail(message);
                }
                catch (Exception e)
                {
                    result.Status = Status.Error;
                    result.Message = e.InnerException.ToString();
                    return result;
                }
            }

            //4 Return value
            result.Status = Status.Success;
            string mailRecipients = "";
            foreach (var mailrec in mailTo)
            {
                mailRecipients += mailrec + "\r\n";
            }

            result.Message = mailRecipients.Equals("") ? "OK" : string.Format("E-mail verstuurd naar {0}", mailRecipients);
            return result;
        }


        //Method which will replace the AllowedToChangeProjectStatus
        //Checks whether the status-move in process is allowed, and if we need to send an email
        public ProcessFlow CheckProcessFlow(Internship internship, string fromRoleCode, string toRoleCode)
        {
            ProcessFlow ProcessFlow;
            IList<ProcessFlow> ProcessFlows = GetCurrentProcessFlows();

            string StatusFrom = "";
            string RoleFrom = fromRoleCode;
            string RoleTo = toRoleCode.Equals("") ? "COM" : toRoleCode;
            string StatusTo = "";

            var currInternship = _repoWrapper.Internship.FindByCondition(ish => ish.InternshipId == internship.InternshipId)
                .Include(ps => ps.ProjectStatus)
                //.Include(ato => ato.InternshipAssignedUser).ThenInclude(u => u.User).ThenInclude(r => r.Role)
                .FirstOrDefault();

            StatusFrom = currInternship.ProjectStatus.Code;
            StatusTo = internship.ProjectStatus.Code;

            ProcessFlow = ProcessFlows.FirstOrDefault(
                            p => p.StatusFrom == StatusFrom &&
                                 p.RoleFrom == RoleFrom &&
                                 p.StatusTo == StatusTo &&
                                 p.RoleTo == RoleTo);

            return ProcessFlow;

        }

        public IList<ProcessFlow> GetCurrentProcessFlows()
        {
            IList<ProcessFlow> ProcessFlows = new List<ProcessFlow>();
            ProcessFlow ProcessFlow = new ProcessFlow
            { StatusFrom = "NEW", StatusTo = "REJ", RoleFrom = "COO", RoleTo = "COM", FlowAllowed = true, MailSubject = "Stageaanvraag geweigerd", SendMail = true, AssignUserRequired = false };
            ProcessFlows.Add(ProcessFlow);
            ProcessFlow = new ProcessFlow
            { StatusFrom = "NEW", StatusTo = "FEE", RoleFrom = "COO", RoleTo = "COM", FlowAllowed = true, MailSubject = "Stageaanvraag: opmerking", SendMail = true, AssignUserRequired = false };
            ProcessFlows.Add(ProcessFlow);
            ProcessFlow = new ProcessFlow
            { StatusFrom = "NEW", StatusTo = "REV", RoleFrom = "COO", RoleTo = "REV", FlowAllowed = true, MailSubject = "Stageaanvraag te beoordelen", SendMail = true, AssignUserRequired = false };
            ProcessFlows.Add(ProcessFlow);
            ProcessFlow = new ProcessFlow
            { StatusFrom = "NEW", StatusTo = "APP", RoleFrom = "COO", RoleTo = "COM", FlowAllowed = true, MailSubject = "Stageaanvraag goedgekeurd", SendMail = true, AssignUserRequired = false };
            ProcessFlows.Add(ProcessFlow);
            ProcessFlow = new ProcessFlow
            { StatusFrom = "REV", StatusTo = "REJ", RoleFrom = "COO", RoleTo = "COM", FlowAllowed = true, MailSubject = "Stageaanvraag geweigerd", SendMail = true, AssignUserRequired = false };
            ProcessFlows.Add(ProcessFlow);
            ProcessFlow = new ProcessFlow
            { StatusFrom = "REV", StatusTo = "FEE", RoleFrom = "COO", RoleTo = "COM", FlowAllowed = true, MailSubject = "Stageaanvraag: opmerking", SendMail = true, AssignUserRequired = false };
            ProcessFlows.Add(ProcessFlow);
            ProcessFlow = new ProcessFlow
            { StatusFrom = "REV", StatusTo = "APP", RoleFrom = "COO", RoleTo = "COM", FlowAllowed = true, MailSubject = "Stageaanvraag goedgekeurd", SendMail = true, AssignUserRequired = false };
            ProcessFlows.Add(ProcessFlow);
            ProcessFlow = new ProcessFlow
            { StatusFrom = "REV", StatusTo = "REV", RoleFrom = "REV", RoleTo = "COO", FlowAllowed = true, SendMail = false, AssignUserRequired = false };
            ProcessFlows.Add(ProcessFlow);
            ProcessFlow = new ProcessFlow
            { StatusFrom = "REV", StatusTo = "REV", RoleFrom = "REV", RoleTo = "REV", FlowAllowed = true, SendMail = false, AssignUserRequired = false };
            ProcessFlows.Add(ProcessFlow);
            ProcessFlow = new ProcessFlow
            { StatusFrom = "REV", StatusTo = "REV", RoleFrom = "COO", RoleTo = "REV", FlowAllowed = true, MailSubject = "Stageaanvraag te beoordelen", SendMail = true, AssignUserRequired = false };
            ProcessFlows.Add(ProcessFlow);

            //COO : Assing to me
            ProcessFlow = new ProcessFlow
            { StatusFrom = "REV", StatusTo = "REV", RoleFrom = "COO", RoleTo = "COO", FlowAllowed = true, MailSubject = "", SendMail = false, AssignUserRequired = false };
            ProcessFlows.Add(ProcessFlow);

            return ProcessFlows;
        }



    }
}
