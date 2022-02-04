using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StagebeheerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        public GenericController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        [Route("country")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
        {
            return await _repoWrapper.Country.FindAll().ToListAsync();
        }


    }
}