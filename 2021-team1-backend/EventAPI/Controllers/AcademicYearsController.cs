using System;
using System.Threading.Tasks;
using EventAPI.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/academic-years")]
    public class AcademicYearsController : Controller
    {
        private readonly IAcademicYearBll _academicYearBll;

        public AcademicYearsController(IAcademicYearBll academicYearBll)
        {
            _academicYearBll = academicYearBll;
        }

        /// <summary>
        ///     Get all academic years
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAcademicYears()
        {
            var academicYears = await _academicYearBll.GetAsyncVm();
            return Ok(academicYears);
        }

        /// <summary>
        ///     Get an academic year by id
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAcademicYearById([FromRoute] Guid id)
        {
            var academicYearsVm = await _academicYearBll.GetByIdAsyncVm(id);
            return Ok(academicYearsVm);
        }

        /// <summary>
        ///     Create a new academic year
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateAcademicYear()
        {
            var academicYearVm = await _academicYearBll.CreateAsyncVm();
            return Ok(academicYearVm);
        }
    }
}