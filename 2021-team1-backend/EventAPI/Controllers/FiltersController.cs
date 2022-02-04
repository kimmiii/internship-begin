using System.Threading.Tasks;
using EventAPI.BLL;
using Microsoft.AspNetCore.Mvc;

namespace EventAPI.Controllers
{
    [Route("api/filters")]
    [ApiController]
    public class FiltersController : Controller
    {
        private readonly ICompanyBll _companyBll;
        private readonly IEventBll _eventBll;
        private readonly IInternshipBll _internshipBll;

        public FiltersController(
            ICompanyBll companyBll,
            IEventBll eventBll,
            IInternshipBll internshipBll)
        {
            _companyBll = companyBll;
            _eventBll = eventBll;
            _internshipBll = internshipBll;
        }

        [HttpGet]
        [Route("companies")]
        public async Task<IActionResult> GetCompaniesAsync()
        {
            var companies = await _eventBll.GetCompaniesFromActiveEventForFilterAsyncVm();
            return Ok(companies);
        }

        [HttpGet]
        [Route("specialisations")]
        public async Task<IActionResult> GetSpecialisationsAsync()
        {
            var specialisations = await _internshipBll.GetSpecializationsForFilterAsyncVm();
            return Ok(specialisations);
        }

        [HttpGet]
        [Route("locations")]
        public async Task<IActionResult> GetLocationsAsync()
        {
            var locations = await _internshipBll.GetLocationsForFilterAsyncVm();
            return Ok(locations);
        }

        [HttpGet]
        [Route("environments")]
        public async Task<IActionResult> GetEnvironmentsAsync()
        {
            var environments = await _internshipBll.GetEnvironmentsForFilterAsyncVm();
            return Ok(environments);
        }
    }
}