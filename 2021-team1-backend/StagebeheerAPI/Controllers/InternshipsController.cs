using EmailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StagebeheerAPI.Configuration.Constants;
using StagebeheerAPI.Contracts;
using StagebeheerAPI.Domain.Services;
using StagebeheerAPI.Models;
using StagebeheerAPI.Models.ApiModels;
using StagebeheerAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wkhtmltopdf.NetCore;
using Constants = StagebeheerAPI.Configuration.Constants.Constants;
using User = StagebeheerAPI.Models.User;

namespace StagebeheerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InternshipsController : ControllerBase
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly IEmailSender _emailSender;
        private readonly IGeneratePdf _generatePdf;
        private readonly IInternshipPaginator _internshipPaginator;
        private readonly ICombinedFilter _comboFilter;

        public InternshipsController(IRepositoryWrapper repoWrapper, IEmailSender emailSender, IGeneratePdf generatePdf, IInternshipPaginator internshipPaginator, ICombinedFilter combofilter)
        {
            _repoWrapper = repoWrapper;
            _emailSender = emailSender;
            _generatePdf = generatePdf;
            _internshipPaginator = internshipPaginator;
            _comboFilter = combofilter;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Internship>>> GetInternship()
        {
            var totalOverviewInternships = _repoWrapper.Internship.FindAll()
            .Include(c => c.Company)
            .Include(ass => ass.InternshipAssignedUser).ThenInclude(u => u.User).ThenInclude(r => r.Role)
            .Include(ps => ps.ProjectStatus)
            .Include(ip => ip.InternshipPeriod)
                ;

            return await totalOverviewInternships.ToListAsync();
        }

        [HttpPost("Approved")]
        public async Task<ActionResult<PagedInternshipsResult>> GetApprovedInternship([FromBody] GetApprovedInternshipCriteria criteria)
        {
            var allApprovedInternships = _repoWrapper.Internship.FindByCondition(x => x.ProjectStatus.Code == "APP")
                .Include(c => c.Company)
                .Include(i => i.InternshipSpecialisation).ThenInclude(s => s.Specialisation)
                .Include(e => e.InternshipEnvironment).ThenInclude(en => en.Environment)
                .Include(p => p.InternshipPeriod).ThenInclude(ep => ep.Period)
                .Include(x => x.InternshipExpectation).ThenInclude(ex => ex.Expectation)
                .Include(ass => ass.InternshipAssignedUser).ThenInclude(u => u.User).ThenInclude(r => r.Role)
                .Include(ps => ps.ProjectStatus)
                .Include(uf => uf.UserFavourites)
                .Include(ui => ui.UserInternships)
                .ToList();

            var filteredInternships = criteria.FilterCriteria != null ?
                _comboFilter.comboFiltering(allApprovedInternships, criteria.FilterCriteria) :
                allApprovedInternships;

            var pagedInternshipsResult = Task.Run(() => _internshipPaginator.GetInternshipsAndPages(filteredInternships, criteria.PageCriteria));

            return await pagedInternshipsResult;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Internship>> GetInternship(int id)
        {
            var internship = await _repoWrapper.Internship.FindByCondition(x => x.InternshipId == id)
                .Include(c => c.Company).ThenInclude(cs => cs.Contacts)
                .Include(ps => ps.ProjectStatus)
                .Include(ass => ass.InternshipAssignedUser).ThenInclude(us => us.User).ThenInclude(ro => ro.Role)
                .Include(i => i.InternshipPeriod).ThenInclude(i => i.Period)
                .Include(isp => isp.InternshipSpecialisation).ThenInclude(isp => isp.Specialisation)
                .Include(ise => ise.InternshipEnvironment).ThenInclude(ise => ise.Environment)
                .Include(ise => ise.InternshipExpectation).ThenInclude(ise => ise.Expectation)
                .Include(uf => uf.UserFavourites)
                .Include(ui => ui.UserInternships)
                .FirstOrDefaultAsync(i => i.InternshipId == id);

            if (internship == null)
            {
                return NotFound();
            }

            return internship;
        }

        [HttpGet("{companyId}/getByCompany")]
        public async Task<ActionResult<IEnumerable<Internship>>> GetInternshipByCompany(int companyId)
        {
            var totalOverviewInternships = _repoWrapper.Internship.FindByCondition(x => x.CompanyId == companyId)
            .Include(ass => ass.InternshipAssignedUser).ThenInclude(u => u.User).ThenInclude(r => r.Role)
            .Include(ps => ps.ProjectStatus);

            return await totalOverviewInternships.ToListAsync();
        }

        [HttpGet("{reviewerId}/getByReviewer")]
        public async Task<ActionResult<IEnumerable<Internship>>> GetInternshipByReviewer(int reviewerId)
        {
            var totalOverviewInternships = _repoWrapper.Internship.FindByCondition(x => x.InternshipReviewer.Any(y => y.UserId == reviewerId))
            .Include(ass => ass.InternshipAssignedUser)
            .Include(ps => ps.ProjectStatus);

            return await totalOverviewInternships.ToListAsync();
        }

        [HttpGet("getBySpecialisation/{specialisationCode}")]
        public async Task<ActionResult<IEnumerable<Internship>>> getBySpecialisation(string specialisationCode)
        {
            int specialisationId = _repoWrapper.Specialisation.FindByCondition(spe => spe.Code == specialisationCode).FirstOrDefault().SpecialisationId;

            var internships = _repoWrapper.Internship.FindByCondition(x => x.InternshipSpecialisation.Any(y => y.SpecialisationId == specialisationId))
            .Include(ass => ass.InternshipSpecialisation)
            .Include(c => c.Company)
            .Include(ass => ass.InternshipAssignedUser).ThenInclude(u => u.User).ThenInclude(r => r.Role)
            .Include(ps => ps.ProjectStatus)
            .Include(ip => ip.InternshipPeriod);

            return await internships.ToListAsync();
        }

        [HttpGet("getNotBySpecialisationEICT")]
        public async Task<ActionResult<IEnumerable<Internship>>> getNotBySpecialisationEICT()
        {
            int specialisationId = _repoWrapper.Specialisation.FindByCondition(spe => spe.Code == "EICT").FirstOrDefault().SpecialisationId;

            var tempInternships = _repoWrapper.Internship.FindAll().ToList();

            var internships = _repoWrapper.Internship.FindByCondition(x => x.InternshipSpecialisation.Any(y => y.SpecialisationId != specialisationId))
            .Include(ass => ass.InternshipSpecialisation)
            .Include(c => c.Company)
            .Include(ass => ass.InternshipAssignedUser).ThenInclude(u => u.User).ThenInclude(r => r.Role)
            .Include(ps => ps.ProjectStatus)
            .Include(ip => ip.InternshipPeriod);

            return await internships.ToListAsync();
        }

        [HttpGet("getAppliedStudents/{internshipId}")]
        public async Task<ActionResult<IEnumerable<User>>> GetAppliedStudents(int internshipId)
        {
            var students = _repoWrapper.User.FindByCondition(x => x.UserInternships.Any(y => y.InternshipId == internshipId))
                .Include(stu => stu.UserInternships);

            return await students.ToListAsync();
        }

        [HttpGet("getCountAppliedStudents/{internshipId}")]
        public ActionResult<int> GetCountAppliedStudents(int internshipId)
        {
            var students = _repoWrapper.User.FindByCondition(x => x.UserInternships.Any(y => y.InternshipId == internshipId))
                .Include(stu => stu.UserInternships);

            return students.Count();
        }

        [HttpGet]
        [Route("{id}/pdf")]
        public async Task<IActionResult> DownloadInternshipPDF(int id)
        {
            var internship = await _repoWrapper.Internship.FindByCondition(x => x.InternshipId == id)
             .Include(c => c.Company).ThenInclude(cs => cs.Contacts)
             .Include(ps => ps.ProjectStatus)
             .Include(at => at.InternshipAssignedUser)
             .Include(i => i.InternshipPeriod).ThenInclude(i => i.Period)
             .Include(isp => isp.InternshipSpecialisation).ThenInclude(isp => isp.Specialisation)
             .Include(ise => ise.InternshipEnvironment).ThenInclude(ise => ise.Environment)
             .Include(ise => ise.InternshipExpectation).ThenInclude(ise => ise.Expectation)
             .FirstOrDefaultAsync(i => i.InternshipId == id);

            if (internship == null)
            {
                var result = new Result();
                result.Status = Status.Error;
                result.Message = "Pdf ID bestaad niet.";
                return BadRequest(result);
            }
            else
            {
                var pdf = await _generatePdf.GetByteArray("ApplicationForm", internship);
                var pdfStream = new System.IO.MemoryStream();
                pdfStream.Write(pdf, 0, pdf.Length);
                pdfStream.Position = 0;
                return new FileStreamResult(pdfStream, "application/pdf")
                {
                    FileDownloadName = "ApplicationForm" + id + ".pdf"
                };
            }
        }

        [HttpPut("ProjectStatus")]
        public async Task<IActionResult> ChangeProjectStatus(Internship internship)
        {
            Result result = new Result();
            InternshipService internshipService = new InternshipService(_repoWrapper, _emailSender);
            result = await internshipService.ChangeChangeProjectStatus(internship);
            if (result.Status == Status.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutInternship(int id, Internship internship)
        {
            if (id != internship.InternshipId)
            {
                return BadRequest();
            }

            //if (!_repoWrapper.Internship.CompanyCanEditInternshipRequest(id))
            //{
            //    var result = new Result();
            //    result.Status = Status.Error;
            //    result.Message = "Deze aanvraag kan niet meer gewijzigd worden.";
            //    return BadRequest(result);
            //}

            var existingInternship = _repoWrapper.Internship.FindByCondition(x => x.InternshipId == id).FirstOrDefault();
            existingInternship = internship;
            existingInternship.Company = internship.Company;
            existingInternship.ProjectStatus = internship.ProjectStatus;

            existingInternship.InternshipPeriod = internship.InternshipPeriod;
            existingInternship.InternshipSpecialisation = internship.InternshipSpecialisation;
            existingInternship.InternshipEnvironment = internship.InternshipEnvironment;
            existingInternship.InternshipExpectation = internship.InternshipExpectation;

            existingInternship.InternshipAssignedUser = internship.InternshipAssignedUser;

            // Get specialisation(s)
            //ICollection<InternshipSpecialisation> specialisations = internship.InternshipSpecialisation;

            // Get Elektronica-ICT specialisation
            Specialisation specialisationEICT = _repoWrapper.Specialisation.FindByCondition(spe => spe.Code == "EICT").FirstOrDefault();

            bool containsEICT = false;
            bool containsOtherSpecialisation = false;

            foreach (InternshipSpecialisation internshipSpecialisation in existingInternship.InternshipSpecialisation)
            {
                if (internshipSpecialisation.SpecialisationId == specialisationEICT.SpecialisationId)
                {
                    containsEICT = true;
                }
                else
                {
                    containsOtherSpecialisation = true;
                }
            }

            //Get Role object
            Role roleCOO = _repoWrapper.Role.FindByCondition(ro => ro.Code == "COO").FirstOrDefault();

            //Get single User object with Role COO, firstname and surname
            Models.User userCOOOther = _repoWrapper.User.FindByCondition(us =>
            (us.RoleId == us.RoleId) &&
            (us.UserFirstName == Constants.stagecoordinatorFirstname && us.UserSurname == Constants.stagecoordinatorSurname)).FirstOrDefault();

            Models.User userCOOEICT = _repoWrapper.User.FindByCondition(us =>
            (us.RoleId == roleCOO.RoleId) &&
            (us.UserFirstName == Constants.stagecoordinatorEICTFirstname && us.UserSurname == Constants.stagecoordinatorEICTSurname)).FirstOrDefault();

            //Create assignedUser child object
            InternshipAssignedUser assignedUserOther = new InternshipAssignedUser();
            InternshipAssignedUser assignedUserEICT = new InternshipAssignedUser();
            assignedUserOther.UserId = userCOOOther.UserId;
            assignedUserEICT.UserId = userCOOEICT.UserId;

            //Assign to current parent object
            existingInternship.InternshipAssignedUser = new List<InternshipAssignedUser>();

            if (containsOtherSpecialisation)
            {
                existingInternship.InternshipAssignedUser.Add(assignedUserOther);
            }

            if (containsEICT)
            {
                existingInternship.InternshipAssignedUser.Add(assignedUserEICT);
            }

            var oldEnvironments = await _repoWrapper.InternshipEnvironment.FindByCondition(x => x.InternshipId == id).ToListAsync();
            var oldPeriods = await _repoWrapper.InternshipPeriod.FindByCondition(x => x.InternshipId == id).ToListAsync();
            var oldExpectations = await _repoWrapper.InternshipExpectation.FindByCondition(x => x.InternshipId == id).ToListAsync();
            var oldSpecialisations = await _repoWrapper.InternshipSpecialisation.FindByCondition(x => x.InternshipId == id).ToListAsync();
            var oldAssignedUsers = await _repoWrapper.InternshipAssignedUser.FindByCondition(x => x.InternshipId == id).ToListAsync();

            _repoWrapper.InternshipEnvironment.DeleteRange(oldEnvironments);
            _repoWrapper.InternshipPeriod.DeleteRange(oldPeriods);
            _repoWrapper.InternshipExpectation.DeleteRange(oldExpectations);
            _repoWrapper.InternshipSpecialisation.DeleteRange(oldSpecialisations);
            _repoWrapper.InternshipAssignedUser.DeleteRange(oldAssignedUsers);

            try
            {
                _repoWrapper.Internship.Update(existingInternship);
                await Task.Run(() => _repoWrapper.Save());

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InternshipExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Internship>> PostInternship(Internship internship)
        {
            try
            {
                if (internship != null)
                {
                    //Get Role object
                    Role roleCOO = _repoWrapper.Role.FindByCondition(ro => ro.Code == "COO").FirstOrDefault();

                    //Get single User object with Role COO
                    Models.User userCOO = _repoWrapper.User.FindByCondition(us => us.RoleId == roleCOO.RoleId).FirstOrDefault();

                    //Create assignedUser child object
                    InternshipAssignedUser au = new InternshipAssignedUser();
                    au.UserId = userCOO.UserId;

                    //Assign to current parent object
                    internship.InternshipAssignedUser = new List<InternshipAssignedUser>();
                    internship.InternshipAssignedUser.Add(au);

                    // Define academic year
                    //DateTime currentDate = DateTime.Now;
                    DateTime currentDate = DateTime.Now;
                    int currentYear = currentDate.Year;
                    DateTime startDate;
                    DateTime endDate;
                    string academicYear;

                    // Define start and end date
                    // 1 SEP - 30 DEC
                    if (currentDate >= new DateTime(currentYear, 9, 1, 0, 0, 0) && currentDate < new DateTime(currentYear+1, 1, 1, 0, 0, 0))
                    {
                        startDate = new DateTime(currentYear, 9, 1, 0, 0, 0); // 1 SEP this year
                        endDate = new DateTime(currentYear + 1, 6, 30, 0, 0, 0); // 30 JUN next year
                    }
                    // 1 JAN - 30 JUN
                    else
                    {
                        startDate = new DateTime(currentYear-1, 9, 1, 0, 0, 0); // 1 SEP last year
                        endDate = new DateTime(currentYear, 6, 30, 0, 0, 0); // 30 JUN this year
                    }

                    // Define academic year
                    if (currentDate > startDate && currentDate < endDate)
                    {
                        if (currentYear > startDate.Year) // This academic year
                        {
                            academicYear = $"{currentYear - 1}-{currentYear}";
                        }
                        else
                        {
                            academicYear = $"{currentYear}-{currentYear + 1}"; 
                        }
                    }
                    else
                    {
                        academicYear = $"{currentYear}-{currentYear+1}"; // Next academic year
                    }

                    internship.AcademicYear = academicYear;

                    _repoWrapper.Internship.Create(internship);

                    await Task.Run(() => _repoWrapper.Save());
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.InnerException);
            }

            return CreatedAtAction("GetInternship", new { id = internship.InternshipId }, internship);
        }


        [HttpPost("SetFavourite")]
        public async Task<ActionResult<Internship>> PostFavourite([FromBody] UserFavourites favourite)
        {
            try
            {
                if (favourite != null && InternshipExists(favourite.InternshipId))
                {
                    var lookupFavourite = _repoWrapper.Internship.FindByCondition(x => x.InternshipId == favourite.InternshipId)
                        .Include(f => f.UserFavourites)
                        .FirstOrDefault();
                    //if (!lookupFavourite.UserFavourites.Any(u => u.InternshipId == favourite.InternshipId))
                    //{
                    var newFavourites = new List<UserFavourites>();
                    newFavourites = lookupFavourite.UserFavourites.ToList();
                    newFavourites.Add(new UserFavourites { UserId = favourite.UserId });
                    lookupFavourite.UserFavourites = newFavourites;

                    var oldFavourites = await _repoWrapper.UserFavourites.FindByCondition(x => x.InternshipId == favourite.InternshipId).ToListAsync();
                    _repoWrapper.UserFavourites.DeleteRange(oldFavourites);

                    _repoWrapper.Internship.Update(lookupFavourite);
                    await Task.Run(() => _repoWrapper.Save());
                    //}

                }

            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.InnerException);
            }

            return Ok(favourite.InternshipId);
        }

        [HttpPost("DeleteFavourite")]
        public async Task<ActionResult<Internship>> DeleteFavourite([FromBody] UserFavourites favourite)
        {
            try
            {
                if (favourite != null && InternshipExists(favourite.InternshipId))
                {
                    var lookupFavourite = _repoWrapper.Internship.FindByCondition(x => x.InternshipId == favourite.InternshipId)
                        .Include(f => f.UserFavourites)
                        .FirstOrDefault();
                    var newFavourites = new List<UserFavourites>();
                    foreach (UserFavourites fav in lookupFavourite.UserFavourites.ToList())
                    {
                        if (fav.UserId != favourite.UserId)
                        {
                            newFavourites.Add(fav);
                        }
                    }
                    lookupFavourite.UserFavourites = newFavourites;

                    var oldFavourites = await _repoWrapper.UserFavourites.FindByCondition(x => x.InternshipId == favourite.InternshipId).ToListAsync();
                    _repoWrapper.UserFavourites.DeleteRange(oldFavourites);

                    _repoWrapper.Internship.Update(lookupFavourite);
                    await Task.Run(() => _repoWrapper.Save());
                }
            }

            catch (Exception e)
            {
                System.Console.WriteLine(e.InnerException);
            }

            return Ok(favourite.InternshipId);
        }

        [HttpGet("{id}/getCountFavourites")]
        public async Task<ActionResult<int>> GetCountFavourites(int id)
        {
            var internships = await _repoWrapper.UserFavourites.FindByCondition(x => x.InternshipId == id)
                .ToListAsync();

            return internships.Count;
        }

        [HttpGet("{id}/getCountFavouritesByStudentId")]
        public async Task<ActionResult<int>> GetCountFavouritesByStudentId(int id)
        {
            var internships = await _repoWrapper.UserFavourites.FindByCondition(x => x.UserId == id)
                .ToListAsync();

            return internships.Count;
        }

        [HttpGet("{id}/getApplicationsByStudentId")]
        public async Task<ActionResult<int>> GetApplicationsByStudentId(int id)
        {
            var internships = await _repoWrapper.UserInternships.FindByCondition(x => x.UserId == id)
                .ToListAsync();

            return internships.Count;
        }

        [HttpGet("getUserInternshipByInternshipIdAndStudentID/{internshipId}/{studentId}")]
        public async Task<ActionResult<UserInternships>> getUserInternshipByInternshipIdAndStudentID(int internshipId, int studentId)
        {
            var userInternship = await _repoWrapper.UserInternships.FindByCondition(x => x.InternshipId == internshipId && x.UserId == studentId).FirstOrDefaultAsync();

            if (userInternship == null)
            {
                userInternship = new UserInternships
                {
                    InternshipId = 0,
                    UserId = 0
                };
            }

            return userInternship;
        }

        [HttpGet("getHireRequestedInternshipsByStudentId/{studentId}")]
        public async Task<ActionResult<List<UserInternships>>> getHireRequestedInternshipsByStudentId(int studentId)
        {
            var internships = await _repoWrapper.UserInternships.FindByCondition(x => x.UserId == studentId && x.HireRequested == true)
                .Include(ui => ui.Internship).ThenInclude(i => i.Company).ToListAsync();

            return internships;
        }

        [HttpGet("studentConfirmedHireRequest/{studentId}")]
        public async Task<ActionResult<bool>> StudentConfirmedHireRequest(int studentId) 
        {
            var userInternships = await _repoWrapper.UserInternships.FindByCondition(x => x.UserId == studentId).ToListAsync();
            bool studentConfirmedHireRequest = false;

            foreach(UserInternships userInternship in userInternships) 
            {
                if (userInternship.HireConfirmed)
                {
                    studentConfirmedHireRequest = true;
                }
            }

            return studentConfirmedHireRequest;
        }

        [HttpGet("studentApprovedHireRequest/{studentId}")]
        public async Task<ActionResult<bool>> StudentApprovedHireRequest(int studentId)
        {
            var userInternships = await _repoWrapper.UserInternships.FindByCondition(x => x.UserId == studentId).ToListAsync();
            bool studentApprovedHireRequest = false;

            foreach (UserInternships userInternship in userInternships)
            {
                if (userInternship.HireApproved)
                {
                    studentApprovedHireRequest = true;
                }
            }

            return studentApprovedHireRequest;
        }

        [HttpGet("getInternshipIdByInternshipAssignedUser/{userId}")]
        public async Task<ActionResult<int>> GetInternshipIdByInternshipAssignedUser(int userId)
        {
            var internshipAssignedUser = await _repoWrapper.InternshipAssignedUser.FindByCondition(x => x.UserId == userId).FirstOrDefaultAsync();
            
            if (internshipAssignedUser == null)
            {
                return NotFound();
            }

            return internshipAssignedUser.InternshipId;
        }

        [HttpPost("ApplyInternship")]
        public async Task<ActionResult<Internship>> ApplyInternship([FromBody] UserInternships internshipApplication)
        {
            try
            {
                if (internshipApplication != null && InternshipExists(internshipApplication.InternshipId))
                {
                    var lookupApplication = _repoWrapper.Internship.FindByCondition(x => x.InternshipId == internshipApplication.InternshipId)
                        .Include(f => f.UserInternships)
                        .FirstOrDefault();
                    var newApplications = new List<UserInternships>();
                    newApplications = lookupApplication.UserInternships.ToList();
                    newApplications.Add(new UserInternships { UserId = internshipApplication.UserId });
                    lookupApplication.UserInternships = newApplications;

                    var oldApplications = await _repoWrapper.UserInternships.FindByCondition(x => x.InternshipId == internshipApplication.InternshipId).ToListAsync();
                    _repoWrapper.UserInternships.DeleteRange(oldApplications);

                    _repoWrapper.Internship.Update(lookupApplication);
                    await Task.Run(() => _repoWrapper.Save());
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.InnerException);
            }

            return Ok(internshipApplication.InternshipId);
        }

        [HttpPost("RemoveApplication")]
        public async Task<ActionResult<Internship>> RemoveApplication([FromBody] UserInternships userInternships)
        {
            try
            {
                if (userInternships != null && InternshipExists(userInternships.InternshipId))
                {
                    var lookupApplication = _repoWrapper.Internship.FindByCondition(x => x.InternshipId == userInternships.InternshipId)
                        .Include(f => f.UserInternships)
                        .FirstOrDefault();
                    var newApplication = new List<UserInternships>();
                    foreach (UserInternships ui in lookupApplication.UserInternships.ToList())
                    {
                        if (ui.UserId != userInternships.UserId)
                        {
                            newApplication.Add(ui);
                        }
                    }
                    lookupApplication.UserInternships = newApplication;

                    var oldApplication = await _repoWrapper.UserInternships.FindByCondition(x => x.InternshipId == userInternships.InternshipId).ToListAsync();
                    _repoWrapper.UserInternships.DeleteRange(oldApplication);

                    _repoWrapper.Internship.Update(lookupApplication);
                    await Task.Run(() => _repoWrapper.Save());
                }
            }

            catch (Exception e)
            {
                System.Console.WriteLine(e.InnerException);
            }

            return Ok(userInternships.InternshipId);
        }

        [HttpGet("GetUserInternships")]
        public async Task<ActionResult<IEnumerable<UserInternships>>> GetUserInternships()
        {
            var userInternships = _repoWrapper.UserInternships.FindAll()
                .Include(ui => ui.Internship)
                .Include(ui => ui.User)
                .Include(ui => ui.Internship.Company);

            return await userInternships.ToListAsync();
        }

        [HttpGet("SetHireRequestedToTrue/{internshipId}/{userId}")]
        public async Task<ActionResult<UserInternships>> SetHireRequestedToTrue(int internshipId, int userId)
        {
            var existingUserInternship = _repoWrapper.UserInternships.FindByCondition(x => x.InternshipId == internshipId && x.UserId == userId).FirstOrDefault();
            existingUserInternship.HireRequested = true;

            try
            {
                _repoWrapper.UserInternships.Update(existingUserInternship);
                await Task.Run(() => _repoWrapper.Save());
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!InternshipExists(internshipId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Send mail
            var result = new Result();
            var toStudent = _repoWrapper.User.FindByCondition(x => x.UserId == userId).FirstOrDefault();
            var internship = _repoWrapper.Internship.FindByCondition(x => x.InternshipId == internshipId).FirstOrDefault();
            var company = _repoWrapper.Company.FindByCondition(x => x.CompanyId == internship.CompanyId).FirstOrDefault();

            if (toStudent == null || internship == null || company == null)
            {
                return NotFound();
            }

            List<string> mailTo = new List<string>();
            mailTo.Add(toStudent.UserEmailAddress);

            
            var companyName = company.Name;

            try
            {
                var subject = EmailMessages.hireRequestedSubject;
                var body = EmailMessages.hireRequestedBody(companyName, internship.ResearchTopicTitle);
                var message = new Message(mailTo, subject, body, null);
                _emailSender.SendEmail(message);
            }
            catch (Exception e)
            {
                result.Status = Status.Error;
                result.Message = e.InnerException.ToString();
                return BadRequest(result);
            }

            return existingUserInternship;
        }

        [HttpGet("SetHireRequestedToFalse/{internshipId}/{userId}")]
        public async Task<IActionResult> SetHireRequestedToFalse(int internshipId, int userId)
        {
            var existingUserInternship = _repoWrapper.UserInternships.FindByCondition(x => x.InternshipId == internshipId && x.UserId == userId).FirstOrDefault();
            existingUserInternship.HireRequested = false;

            try
            {
                _repoWrapper.UserInternships.Update(existingUserInternship);
                await Task.Run(() => _repoWrapper.Save());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InternshipExists(internshipId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        [HttpGet("SetHireConfirmedToTrue/{internshipId}/{userId}")]
        public async Task<IActionResult> SetHireConfirmedToTrue(int internshipId, int userId)
        {
            var existingUserInternship = _repoWrapper.UserInternships.FindByCondition(x => x.InternshipId == internshipId && x.UserId == userId).FirstOrDefault();
            existingUserInternship.HireConfirmed = true;

            try
            {
                _repoWrapper.UserInternships.Update(existingUserInternship);
                await Task.Run(() => _repoWrapper.Save());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InternshipExists(internshipId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        [HttpGet("SetHireConfirmedToFalse/{internshipId}/{userId}")]
        public async Task<IActionResult> SetHireConfirmedToFalse(int internshipId, int userId)
        {
            var existingUserInternship = _repoWrapper.UserInternships.FindByCondition(x => x.InternshipId == internshipId && x.UserId == userId).FirstOrDefault();
            existingUserInternship.HireConfirmed = false;

            try
            {
                _repoWrapper.UserInternships.Update(existingUserInternship);
                await Task.Run(() => _repoWrapper.Save());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InternshipExists(internshipId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        [HttpGet("SetHireApprovedToTrue/{internshipId}/{userId}")]
        public async Task<IActionResult> SetHireApprovedToTrue(int internshipId, int userId)
        {
            var internship = _repoWrapper.Internship.FindByCondition(x => x.InternshipId == internshipId).FirstOrDefault();

            if (internship == null)
            {
                return NotFound("Internship not found");
            }
            else
            {
                var internshipAssignedUser = _repoWrapper.InternshipAssignedUser.FindByCondition(x => x.InternshipId == internshipId).FirstOrDefault();

                // Decide what to do on the base of total interns that are required for the internship
                int? totalInternsRequired = internship.TotalInternsRequired;

                if (totalInternsRequired == 1)
                {
                    // new InternshipAssignedUser
                    if (internshipAssignedUser == null)
                    {
                        internshipAssignedUser = new InternshipAssignedUser
                        {
                            InternshipId = internshipId,
                            UserId = userId
                        };
                       
                        internship.Completed = true;  
                    }
                    else // existing InternshipAssignedUser
                    {
                        BadRequest("InternshipAssignedUser already exists.");
                    }
                }
                else // totalInternsRequired = 2
                {
                    var internshipAssignedUserList = _repoWrapper.InternshipAssignedUser.FindByCondition(x => x.InternshipId == internshipId).ToList();

                    if (internshipAssignedUserList.Count == 0 || internshipAssignedUserList.Count == 1)
                    {
                        internshipAssignedUser = new InternshipAssignedUser
                        {
                            InternshipId = internshipId,
                            UserId = userId
                        };

                        internshipAssignedUserList.Add(internshipAssignedUser);

                        if (internshipAssignedUserList.Count == 2)
                        {
                            internship.Completed = true;
                        }
                    }
                    else // Internship has already 2 assignedUsers
                    {
                        return BadRequest("Internship has already 2 assignedUsers.");
                    }
                }

                var existingUserInternship = _repoWrapper.UserInternships.FindByCondition(x => x.InternshipId == internshipId && x.UserId == userId).FirstOrDefault();
                existingUserInternship.EvaluatedAt = DateTime.Now;
                existingUserInternship.HireApproved = true;

                try
                {
                    _repoWrapper.InternshipAssignedUser.Create(internshipAssignedUser);
                    _repoWrapper.UserInternships.Update(existingUserInternship);
                    _repoWrapper.Internship.Update(internship);

                    await Task.Run(() => _repoWrapper.Save());
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InternshipExists(internshipId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                var toCompany = _repoWrapper.Company.FindByCondition(x => x.CompanyId == internship.CompanyId).FirstOrDefault();
                var toStudent = _repoWrapper.User.FindByCondition(x => x.UserId == userId).FirstOrDefault();
                if (toCompany == null || toStudent == null)
                {
                    return NotFound();
                }

                // Send email to company
                var result = new Result();

                List<string> mailTo = new List<string>();
                mailTo.Add(toCompany.Email);

                try
                {
                    var subject = EmailMessages.hireApprovedToCompanySubject;
                    var body = EmailMessages.hireApprovedToCompanyBody(toStudent.UserFirstName, toStudent.UserSurname, internship.ResearchTopicTitle);
                    var message = new Message(mailTo, subject, body, null);
                    _emailSender.SendEmail(message);
                }
                catch (Exception e)
                {
                    result.Status = Status.Error;
                    result.Message = e.InnerException.ToString();
                    return BadRequest(result);
                }


                // Send email to student
                mailTo = new List<string>();
                mailTo.Add(toStudent.UserEmailAddress);

                try
                {
                    var subject = EmailMessages.hireApprovedToStudentSubject;
                    var body = EmailMessages.hireApprovedToStudentBody(internship.ResearchTopicTitle, toCompany.Name);
                    var message = new Message(mailTo, subject, body, null);
                    _emailSender.SendEmail(message);
                }
                catch (Exception e)
                {
                    result.Status = Status.Error;
                    result.Message = e.InnerException.ToString();
                    return BadRequest(result);
                }

                return Ok();
            } 
        }

        [HttpPut("SetHireApprovedToFalse/{internshipId}/{userId}")]
        public async Task<IActionResult> SetHireApprovedToFalse(int internshipId, int userId, UserInternships userInternships)
        {
            var existingUserInternship = _repoWrapper.UserInternships.FindByCondition(x => x.InternshipId == internshipId && x.UserId == userId).FirstOrDefault();
            existingUserInternship.HireApproved = false;
            existingUserInternship.HireConfirmed = false;
            existingUserInternship.EvaluatedAt = DateTime.Now;
            existingUserInternship.RejectionFeedback = userInternships.RejectionFeedback;

            var internship = _repoWrapper.Internship.FindByCondition(x => x.InternshipId == internshipId).FirstOrDefault();
            var student = _repoWrapper.User.FindByCondition(x => x.UserId == userId).FirstOrDefault();
            var company = _repoWrapper.Company.FindByCondition(x => x.CompanyId == internship.CompanyId).FirstOrDefault(); 

            if (internship == null || student == null || company == null)
            {
                return NotFound();
            }

            try
            {
                _repoWrapper.UserInternships.Update(existingUserInternship);
                await Task.Run(() => _repoWrapper.Save());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InternshipExists(internshipId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Send email to student
            var result = new Result();
            List<string> mailTo = new List<string>();
            mailTo.Add(student.UserEmailAddress);

            try
            {
                var subject = EmailMessages.hireRejectedSubject;
                var body = EmailMessages.hireRejectedBody(internship.ResearchTopicTitle, company.Name, userInternships.RejectionFeedback);
                var message = new Message(mailTo, subject, body, null);
                _emailSender.SendEmail(message);
            }
            catch (Exception e)
            {
                result.Status = Status.Error;
                result.Message = e.InnerException.ToString();
                return BadRequest(result);
            }

            return Ok();
        }

        [HttpPut("ToggleInterestingUserInternship/{internshipId}/{userId}")]
        public async Task<IActionResult> ToggleInterestingUserInternship(int internshipId, int userId)
        {
            var existingUserInternship = _repoWrapper.UserInternships.FindByCondition(x => x.InternshipId == internshipId && x.UserId == userId).FirstOrDefault();
            existingUserInternship.Interesting = !existingUserInternship.Interesting;

            try
            {
                _repoWrapper.UserInternships.Update(existingUserInternship);
                await Task.Run(() => _repoWrapper.Save());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InternshipExists(internshipId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        [Route("Environment")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Environment>>> GetEnvironment()
        {
            return await _repoWrapper.Environment.FindAll().ToListAsync();
        }

        [Route("Expectation")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expectation>>> GetExpectation()
        {
            return await _repoWrapper.Expectation.FindAll().ToListAsync();
        }

        [Route("Period")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Period>>> GetPeriod()
        {
            return await _repoWrapper.Period.FindAll().ToListAsync();
        }

        [Route("Specialisaton")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Specialisation>>> GetSpecialisaton()
        {
            return await _repoWrapper.Specialisation.FindAll().ToListAsync();
        }

        [Route("ProjectStatus")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectStatus>>> GetStatus()
        {
            return await _repoWrapper.ProjectStatus.FindAll().ToListAsync();
        }

        [Route("ProjectStatus/GetStatusById")]
        [HttpGet]
        public async Task<ActionResult<ProjectStatus>> GetCode(int statusId)
        {
            var projectStatus = await _repoWrapper.ProjectStatus.FindByCondition(x => x.ProjectStatusId == statusId)
                .FirstOrDefaultAsync(s => s.ProjectStatusId == statusId);

            if (projectStatus == null)
            {
                return NotFound();
            }
            return projectStatus;
        }

        [HttpGet("GetAcademicYears")]
        public async Task<ActionResult<List<string>>> GetAcademicYears()
        {
            var internships = _repoWrapper.Internship.FindAll().ToList();
            List<string> academicYears = new List<string>();
            string academicYear;
            int minYear = 9999;
            int maxYear = 0;

            for(int i = 0; i < internships.Count; i++)
            {
                int startYear = Convert.ToInt32(internships[i].AcademicYear.Substring(0, 4));
                int endYear = Convert.ToInt32(internships[i].AcademicYear.Substring(5, 4));

                if (startYear < minYear)
                {
                    minYear = startYear;
                }
                
                if (endYear > maxYear)
                {
                    maxYear = endYear;
                }
            }

            for(int i = minYear; i < maxYear; i++)
            {
                academicYear = $"{i}-{i + 1}";
                academicYears.Add(academicYear);
            }

            return academicYears;
        }

        private bool InternshipExists(int id)
        {
            return _repoWrapper.Internship.FindByCondition(x => x.InternshipId == id).Any(e => e.InternshipId == id);
        }
    }
}