using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StagebeheerAPI.Configuration.Constants;
using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;
using StagebeheerAPI.ViewModels;

namespace StagebeheerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;
        private readonly IEmailSender _emailSender;

        public CompanyController(IRepositoryWrapper repoWrapper, IEmailSender emailSender)
        {
            _repoWrapper = repoWrapper;
            _emailSender = emailSender;
        }

        [HttpPost]
        public async Task<ActionResult<Company>> PostCompany(Company company)
        {
                try
                {
                    if (company != null)
                    {
                        _repoWrapper.Company.Create(company);
                        await Task.Run(() => _repoWrapper.Save());
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException);
                }

            return CreatedAtAction(
                actionName: "GetCompany",
                routeValues: new { id = company.CompanyId },
                value: company);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {

            var totalOverviewCompanies = _repoWrapper.Company.FindAll();

            return await totalOverviewCompanies.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            var user = await _repoWrapper.Company.FindByCondition(x => x.CompanyId == id)
                .FirstOrDefaultAsync(i => i.CompanyId == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // GET api/company/Active
        [HttpGet("Active")]
        public async Task<ActionResult<IEnumerable<Company>>> GetActiveCompany()
        {
            var activeCompanies = _repoWrapper.Company.FindByCondition(x => x.Activated == true)
                .Include(c => c.Contacts);

            return await activeCompanies.ToListAsync();
        }

        // GET api/company/new
        [HttpGet("New")]
        public async Task<ActionResult<IEnumerable<Company>>> GetNewCompany()
        {
            var newCompanies = _repoWrapper.Company.FindByCondition(x => x.Activated == false)
                .Where(c => c.EvaluatedAt == null)
                .Include(c => c.Contacts);

            return await newCompanies.ToListAsync();
        }

        // GET api/company/evaluatedInactive
        [HttpGet("evaluatedInactive")]
        public async Task<ActionResult<IEnumerable<Company>>> GetEvaluatedInactiveCompany()
        {
            var newCompanies = _repoWrapper.Company.FindByCondition(x => x.Activated == false)
                .Where(c => c.EvaluatedAt != null)
                .Include(c => c.Contacts);

            return await newCompanies.ToListAsync();
        }

        // GET api/company/getCounters/{companyId}
        [HttpGet("getCounters/{companyId}")]
        public ActionResult<IEnumerable<int>> GetCounters(int companyId)
        {
            /* 
             * Positions from each counter in List counters
             * 0 => count total internships
             * 1 => count total approved internships
             * 2 => count approved internships sem 1
             * 3 => count approved internships sem 2
             * 4 => count total approved internships for specialisation EICT
             * 5 => count total trainees
             * 6 => count total EICT trainees
             */
            List<int> counters = new List<int>();

            if (!_CompanyExists(companyId))
            {
                NotFound();
            }
            else
            {
                var projectStatusId = _repoWrapper.ProjectStatus.FindByCondition(x => x.Code == "APP").FirstOrDefault().ProjectStatusId;
                var periodSemOneId = _repoWrapper.Period.FindByCondition(x => x.Code == "S1").FirstOrDefault().PeriodId;
                var periodSemTwoId = _repoWrapper.Period.FindByCondition(x => x.Code == "S2").FirstOrDefault().PeriodId;
                var specialisationECITId = _repoWrapper.Specialisation.FindByCondition(x => x.Code == "EICT").FirstOrDefault().SpecialisationId;

                int countTotalInternships = _repoWrapper.Internship.FindByCondition(x => x.CompanyId == companyId).ToList().Count;
                int countTotalApprovedInternships = _repoWrapper.Internship.
                    FindByCondition(x => x.CompanyId == companyId && x.ProjectStatusId == projectStatusId).ToList().Count;
                int countTotalApprovedInternshipsSemOne = _repoWrapper.Internship.
                    FindByCondition(x => x.CompanyId == companyId && x.ProjectStatusId == projectStatusId &&
                    x.InternshipPeriod.Any(y => y.PeriodId == periodSemOneId))
                    .ToList().Count;
                int countTotalApprovedInternshipsSemTwo = _repoWrapper.Internship.
                    FindByCondition(x => x.CompanyId == companyId && x.ProjectStatusId == projectStatusId &&
                    x.InternshipPeriod.Any(y => y.PeriodId == periodSemTwoId))
                    .ToList().Count;
                int countTotalApprovedInternshipsEICT = _repoWrapper.Internship.
                    FindByCondition(x => x.CompanyId == companyId && x.ProjectStatusId == projectStatusId &&
                    x.InternshipSpecialisation.Any(y => y.SpecialisationId == specialisationECITId))
                    .ToList().Count;
                int countTotalTrainees = _repoWrapper.InternshipAssignedUser
                    .FindByCondition(x => x.Internship.CompanyId == companyId && x.Internship.Completed == true)
                    .ToList().Count;
                int countTotalEICTTrainees = _repoWrapper.InternshipAssignedUser
                    .FindByCondition(x => x.Internship.CompanyId == companyId && x.Internship.Completed == true &&
                    x.Internship.InternshipSpecialisation.Any(y => y.SpecialisationId == specialisationECITId))
                    .ToList().Count;

                // Fill list
                counters.Add(countTotalInternships);
                counters.Add(countTotalApprovedInternships);
                counters.Add(countTotalApprovedInternshipsSemOne);
                counters.Add(countTotalApprovedInternshipsSemTwo);
                counters.Add(countTotalApprovedInternshipsEICT);
                counters.Add(countTotalTrainees);
                counters.Add(countTotalEICTTrainees);
            }

            return counters;
            
        }

        // PUT: api/Company/5/Approve
        [HttpPut("{id}/Approve")]
        public async Task<IActionResult> ApproveCompany(int id)
        {
            List<string> mailTo = new List<string>();
            var result = new Result();
            
            var existingCompany = _repoWrapper.Company.FindByCondition(x => x.CompanyId == id)
                .FirstOrDefault();

            existingCompany.Activated = true;
            existingCompany.EvaluatedAt = DateTime.Now;

            if (!_CompanyExists(id))
            {
                return NotFound();
            }

            try
            {
                _repoWrapper.Company.Update(existingCompany);
                await Task.Run(() => _repoWrapper.Save());
            }
            catch (DbUpdateConcurrencyException)
            {             
                    throw;            
            }
            
            mailTo.Add(existingCompany.Email);

            try
            {
                var subject = EmailMessages.companyActivedSubject;
                var body = EmailMessages.companyActivedBody;
                var message = new Message(mailTo, subject, body, null);
                _emailSender.SendEmail(message);
            }
            catch (Exception e)
            {
                result.Status = Status.Error;
                result.Message = e.InnerException.ToString();
                return BadRequest(result);
            }

            return NoContent();
        }


        // PUT: api/Company/5/Reject
        [HttpPut("{id}/Reject")]
        public async Task<IActionResult> RejectCompany(int id, string evaluationFeedback)
        {
            List<string> mailTo = new List<string>();
            var result = new Result();
            var existingCompany = _repoWrapper.Company.FindByCondition(x => x.CompanyId == id)
                .FirstOrDefault();

            existingCompany.EvaluationFeedback = evaluationFeedback;
            existingCompany.EvaluatedAt = DateTime.Now;
            existingCompany.Activated = false;

            if (!_CompanyExists(id))
            {
                return NotFound();
            }

            try
            {
                _repoWrapper.Company.Update(existingCompany);
                await Task.Run(() => _repoWrapper.Save());
            }
            catch (DbUpdateConcurrencyException)
            {               
                    throw;
            }
            
            mailTo.Add(existingCompany.Email);
        
            try
            {
                if (existingCompany.EvaluationFeedback == null)
                {
                    existingCompany.EvaluationFeedback = "[De stagecoördinator heeft geen feedback gegeven.]";
                }

                var subject = EmailMessages.companyRejectedSubject;
                var body = EmailMessages.companyRejectedBody(existingCompany.EvaluationFeedback);
                var message = new Message(mailTo, subject, body, null);
                _emailSender.SendEmail(message);
            }
            catch (Exception e)
            {
                //TODO
                //CODE CLEANUP
                result.Status = Status.Error;
                result.Message = e.InnerException.ToString();
                return BadRequest(result);
            }

            return NoContent();
        }

        private bool _CompanyExists(int id)
        {
            return _repoWrapper.Company.FindByCondition(x => x.CompanyId == id).Any(e => e.CompanyId == id);
        }
    }
}