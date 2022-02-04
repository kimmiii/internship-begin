using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StagebeheerAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private IRepositoryWrapper _repoWrapper;

        public UserController(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        //// GET: api/Users
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<User>>> GetUser()
        //{

        //    var totalOverviewUsers = _repoWrapper.User.FindAll();
        //    return await totalOverviewUsers.ToListAsync();
        //}

        //// GET api/Users/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<User>> GetUser(int id)
        //{

        //    var user = await _repoWrapper.User.FindByCondition(x => x.UserId == id)
        //        .Include(r => r.Role)
        //        //.Include(i => i.Internships)
        //        .FirstOrDefaultAsync(i => i.UserId == id);
        //    user.UserPass = null;
        //    user.Salt = null;
                        
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return user;
        //}

        // GET: api/Users/5/Role
        [HttpGet("getReviewers")]
        public async Task<ActionResult<IEnumerable<User>>> GetReviewers()
        {
            var totalOverviewUsers = _repoWrapper.User.FindByCondition(x => x.Role.Code == "REV");
            var usersToSend = await totalOverviewUsers.ToListAsync();
            foreach(User us in usersToSend)
            {
                us.UserPass = null;
                us.Salt = null;
            }
            return usersToSend;
        }

        // GET: api/User/GetStudents
        [HttpGet("getStudents")]
        public async Task<ActionResult<IEnumerable<User>>> GetStudents()
        {
            var totalOverviewUsers = _repoWrapper.User.FindByCondition(x => x.Role.Code == "STU");
            var usersToSend = await totalOverviewUsers.ToListAsync();
            foreach (User us in usersToSend)
            {
                us.UserPass = null;
                us.Salt = null;
            }
            return usersToSend;
        }

        // GET: api/User/GetUserById/{userId}
        [HttpGet("GetUserById/{userId}")]
        public async Task<ActionResult<User>> GetUserById(int userId)
        {
            var user = await _repoWrapper.User.FindByCondition(x => x.UserId == userId)
                .Include(x => x.UserInternships) 
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost("UploadPDF/{id}"), DisableRequestSizeLimit]
        public IActionResult UploadPDF(int id)
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Cv");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = $"cv_student{id}.pdf";
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    var existingStudent = _repoWrapper.User.FindByCondition(x => x.UserId == id)
                        .FirstOrDefault();
                    existingStudent.CvPresent = true;

                    if (existingStudent != null)
                    {
                        try
                        {
                            _repoWrapper.User.Update(existingStudent);
                            _repoWrapper.Save();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            throw;
                        }
                    }
                    else
                    {
                        return NotFound();
                    }

                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("DownloadCV/{studentId}")]
        public async Task<FileStream> DownloadCV(int studentId)
        {
            var folderName = Path.Combine("Resources", "Cv");
            var fileName = $"cv_student{studentId}.pdf";
            var fullPath = Path.Combine(folderName, fileName);
            return new FileStream(fullPath, FileMode.Open, FileAccess.Read);
        }

        [HttpGet("RemoveCv/{id}")]
        public IActionResult RemoveCv(int id)
        {
            var existingStudent = _repoWrapper.User.FindByCondition(x => x.UserId == id)
                       .FirstOrDefault();
            existingStudent.CvPresent = false;

            if (existingStudent != null)
            {
                try
                {
                    _repoWrapper.User.Update(existingStudent);
                    _repoWrapper.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            else
            {
                return NotFound();
            }
            
            return Ok();
        }

        [HttpGet("GetCsvTemplate/{userTypeCode}")]
        public async Task<FileStreamResult> GetCsvTemplate(string userTypeCode)
        {
            //var folderName = Path.Combine("Resources", "Templates");
            //var fileName = "";

            //if (userTypeCode == "REV")
            //{
            //    fileName = "Lectors_template.csv";
            //}
            //else if (userTypeCode == "STU")
            //{
            //    fileName = "Students_template.csv";
            //}

            //var fullPath = Path.Combine(folderName, fileName);

            //return new FileStream(fullPath, FileMode.Open, FileAccess.Read);

            var columns = "";

            if (userTypeCode == "REV")
            {
                // Example in Resources/Templates/Lectors_template.csv
                columns = "voornaam;naam;e-mailadres";
            }
            else
            {
                // Example in Resources/Temlates/Students_template.csv
                columns = "voornaam;naam;straat;huisnr;bus;pc;gemeente;gsmnummer;e-mailadres;afstudeerrichting";
            }

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(columns));
            var result = new FileStreamResult(stream, "text/csv");

            return result;
        }
    }
}
