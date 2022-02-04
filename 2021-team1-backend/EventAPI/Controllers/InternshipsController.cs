using System.Collections.Generic;
using System.Threading.Tasks;
using EventAPI.BLL;
using EventAPI.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventAPI.Controllers
{
    [Route("api/internships")]
    [ApiController]
    [Authorize]
    public class InternshipsController : Controller
    {
        private readonly IInternshipBll _internshipBll;

        public InternshipsController(IInternshipBll internshipBll)
        {
            _internshipBll = internshipBll;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInternshipsAsync()
        {
            var internshipsVm = await _internshipBll.GetAsyncVm();
            return Ok(internshipsVm);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetInternshipsByIdAsync([FromRoute] int id)
        {
            var internshipsVm = await _internshipBll.GetByIdAsyncVm(id);
            return Ok(internshipsVm);
        }

        [HttpGet]
        [Route("companies/{id}")]
        public async Task<IActionResult> GetInternshipsByCompanyAsync([FromRoute] int id)
        {
            var internshipsVm = await _internshipBll.GetByCompanyAsyncVm(id);
            return Ok(internshipsVm);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateInternshipAsync([FromBody] InternshipVM internshipVm)
        {
            if (internshipVm == null) return BadRequest();
            internshipVm = await _internshipBll.UpdateInternshipAsyncVm(internshipVm);
            return Ok(internshipVm);
        }

        [HttpPut]
        [Route("multi")]
        public async Task<IActionResult> UpdateInternshipsMultiAsync([FromBody] List<InternshipVM> internshipVm)
        {
            internshipVm = await _internshipBll.UpdateInternshipsMultiAsyncVm(internshipVm);
            return Ok(internshipVm);
        }
    }
}