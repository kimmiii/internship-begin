using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;

namespace StagebeheerAPI.Controllers
{
    [Route("api/event-communications")]
    [ApiController]
    [Authorize]
    public class EventCommunicationsController : Controller
    {

        private readonly IRepositoryWrapper _repoWrapper;

        public EventCommunicationsController(
            IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [HttpGet]
        [Route("internships")]
        public ActionResult<List<Internship>> GetApprovedInternship()
        {
            var internships = _repoWrapper.Internship
                .FindByCondition(x => x.ProjectStatusId == 4)
                .Include(s => s.InternshipSpecialisation)
                .Include(s => s.InternshipEnvironment)
                .ToList();
         
            return  Ok(internships);
        }

        [HttpGet]
        [Route("internships/{id}")]
        public ActionResult<Internship> GetInternshipById([FromRoute] int id)
        {
            var internship = _repoWrapper.Internship
                .FindByCondition(x => x.InternshipId == id)
                .Include(s => s.InternshipSpecialisation)
                .Include(s => s.InternshipEnvironment)
                .FirstOrDefault();
               
            return Ok(internship); 
        }

        [HttpPut]
        [Route("internships")]
        public async Task<ActionResult> UpdateInternship([FromBody] Internship internship)
        {
            try
            {
                _repoWrapper.Internship.Update(internship);
                await Task.Run(() => _repoWrapper.Save());
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("internships/company/{id}")]
        public ActionResult<List<Internship>> GetApprovedInternshipByCompany([FromRoute] int id)
        {
            var internships = _repoWrapper.Internship
                .FindByCondition(x => x.ProjectStatusId == 4 && x.CompanyId == id)
                .Include(s => s.InternshipSpecialisation)
                .Include(s => s.InternshipEnvironment)
                .ToList();

            return Ok(internships);
        }

        [HttpGet]
        [Route("internships/academic-year/{academicYear}")]
        public ActionResult<List<Internship>> GetApprovedInternshipByAcademicYear([FromRoute] string academicYear)
        {
            var internships = _repoWrapper.Internship
                .FindByCondition(x => x.ProjectStatusId == 4 && x.AcademicYear == academicYear && x.ShowInEvent)
                .Include(s => s.InternshipSpecialisation)
                .Include(s => s.InternshipEnvironment)
                .ToList();

            return Ok(internships);
        }

        [HttpGet]
        [Route("internships/academic-year/{academicYear}/company-id/{companyId}")]
        public ActionResult<List<Internship>> GetApprovedInternshipByAcademicYearAndCompanyId([FromRoute] string academicYear, [FromRoute] int companyId)
        {
            var internships = _repoWrapper.Internship
                .FindByCondition(x => x.ProjectStatusId == 4 && x.AcademicYear == academicYear && x.CompanyId == companyId)
                .ToList();

            return Ok(internships);
        }

        [HttpGet]
        [Route("specialisations")]
        public IActionResult GetSpecialisations()
        {
            var specialisations = _repoWrapper.Specialisation.FindAll().ToList();
            
            return Ok(specialisations);
        }

        [HttpGet]
        [Route("locations")]
        public ActionResult GetLocations()
        {
            var locations = _repoWrapper.Internship.FindAll()
                .Where(y => y.WpCity != null)
                .Select(y => new { WpCity = y.WpCity, WpZipCode = y.WpZipCode })
                .Distinct()
                .ToList();

            return Ok(locations);
        }

        [HttpGet]
        [Route("environments")]
        public IActionResult GetEnvironments()
        {
            var environments = _repoWrapper.Environment.FindAll().ToList();

            return Ok(environments);
        }

        [HttpGet]
        [Route("contacts")]
        public IActionResult GetContacts()
        {
            var contacts = _repoWrapper.Contact.FindAll().ToList();

            return Ok(contacts);
        }

        [HttpGet]
        [Route("users/{id}")]
        public IActionResult GetStudents([FromRoute] int id)
        {

            var student = _repoWrapper.User
                .FindByCondition(x => x.UserId == id)
                .FirstOrDefault();

            return Ok(student);
        }
    }
}
