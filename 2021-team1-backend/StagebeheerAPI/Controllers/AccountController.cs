using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;
using StagebeheerAPI.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StagebeheerAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<User> userManager, IRepositoryWrapper repoWrapper,  ITokenService tokenService)
        {
            _userManager = userManager;
            _repoWrapper = repoWrapper;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<ActionResult<User>> Login([FromBody]Login model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var user = await _userManager.FindByNameAsync(model.UserEmailAddress);
            if (user == null) return Unauthorized();
            if (!await _userManager.CheckPasswordAsync(user, model.Password)) return Unauthorized();
            string token = await _tokenService.GenerateJSONWebTokenAsync(user);
            return Ok(new { Token = token });
        }


        [HttpPost("Company")]
        public async Task<ActionResult<User>> Register([FromBody]Register model)
        {
            if (ModelState.IsValid) 
            {
                IdentityResult result = null;
                var user = await _userManager.FindByNameAsync(model.UserEmailAddress);
                var rolesInDatabase = _repoWrapper.Role.FindAll();

                if (user != null)
                {
                    return Conflict();
                }

                user = new User
                {
                    UserEmailAddress = model.UserEmailAddress,
                    RegistrationDate = DateTime.UtcNow,
                    RoleId = rolesInDatabase.Where(x => x.Code == "COM").First().RoleId,
                    Activated = true
                };

                result = await _userManager.CreateAsync(user, model.UserPass);
                if (result.Succeeded)
                {
                    var createdUser = await _userManager.FindByNameAsync(user.UserEmailAddress);
                    return CreatedAtAction("GetUser", _MapUserToApiModelUser(createdUser), _MapUserToApiModelUser(createdUser));
                }
                else
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        private Models.ApiModels.User _MapUserToApiModelUser(Models.User user, String token="")
        {
            return new Models.ApiModels.User
            {
                UserId = user.UserId,
                UserEmailAddress = user.UserEmailAddress,
                RoleId = user.RoleId,
                UserFirstName = user.UserFirstName,
                UserSurname = user.UserSurname,
                RoleCode = user.Role != null ? user.Role.Code : null,
                RoleDescription = user.Role != null ? user.Role.Description : null,
                IsUserActivated = user.Activated,
                CvPresent = user.CvPresent,
                CompanyId = user.Company != null ? user.Company.CompanyId : 0,
                IsCompanyActivated = user.Company != null ? user.Company.Activated : false,
                Token = token
            };
        }


  
        [HttpPost]
        public async Task<ActionResult<User>> PostCSVLector(IFormFile postedFile)
        {
            int lineIndex = 0;
            string line = null;
            string headerError = null; ;
            string dataError = null;
            var users = new List<User>();
            int newUsersCount = 0;
            int existingUsersCount = 0;
            ArrayList errorUsers = new ArrayList();
            ArrayList feedbackUsers = new ArrayList();
            var genericPassword = "AQAAAAEAACcQAAAAEPvBo3i7bpJn1faRTkFqaIQoS5B+ikTQfPR3TD8nbcy1aGItn2Z/kH8BLcqmS0SyWQ==";

            if (postedFile != null)
            {
                try
                {
                    string fileExtension = Path.GetExtension(postedFile.FileName);

                    //Validate uploaded file and return error.
                    if (fileExtension != ".csv")
                    {
                        var result = new Result();
                        result.Status = Status.Error;
                        result.Message = "Please select the csv file with .csv extension.";
                        return BadRequest(result);                                             
                    }

                    //Read file + check headers
                    using (var sreader = new StreamReader(postedFile.OpenReadStream()))
                    {
                        //First line is header.
                        List<string> headers = sreader.ReadLine().Split(';').ToList();
                        int roleholder = 0;
                        int count = headers.Count();

                        //reviewer headers Check 
                        if (count == 3)
                        {
                            headerError = _repoWrapper.User.CheckReviewerHeader(headers);
                            roleholder = 2;
                        }
                        //Headers Error 
                        else
                        {
                            headerError = "Headers van .csv-bestand bevat fouten.";
                        }
                        //Badrequest header
                        if (headerError != null)
                        {
                            var result = new Result();
                            result.Status = Status.Error;
                            result.Message = headerError;
                            return BadRequest(result);
                        }

                        //Loop through reviewer records
                        if (roleholder == 2)
                        {
                            lineIndex++;
                            while (!sreader.EndOfStream)
                            {
                                lineIndex++;
                                line = sreader.ReadLine();
                                int commas = line.Count(c => c == ';');
                                string[] rows = line.Split(';');

                                dataError = _repoWrapper.User.CheckReviewerData(rows, commas);

                                if (dataError == null)
                                {
                                    users.Add(new User
                                    {
                                        UserFirstName = rows[0].ToString(),
                                        UserSurname = rows[1].ToString(),
                                        UserEmailAddress = rows[2].ToString(),
                                        RegistrationDate = DateTime.UtcNow,
                                        Activated = true,
                                        CvPresent = false,
                                        UserPass = genericPassword,
                                        RoleId = roleholder
                                    }); ;
                                }
                                else
                                {
                                    errorUsers.Add(lineIndex + " Fout : " + dataError);
                                }
                            }
                        }
                    }
                        
                  
                    //Add Users
                    foreach (var User in users)
                    {
                        if (ModelState.IsValid)
                        {
                            IdentityResult result = null;
                            var user = await _userManager.FindByNameAsync(User.UserEmailAddress);                            

                            if (user != null)
                            {
                                existingUsersCount++;                                                         
                            }
                            else 
                            {
                                _repoWrapper.User.Create(User);
                                _repoWrapper.Save();
                                newUsersCount++;
                            }
                        }
                    }
                    feedbackUsers.Add(newUsersCount + " nieuwe gebruikers succesvol toegevoegd.");
                    feedbackUsers.Add(existingUsersCount + " gebruiker(s) bestaan reeds.");
                    if (errorUsers.Count > 0) 
                    {
                        feedbackUsers.Add("fouten :");
                        feedbackUsers.Add(errorUsers); 
                    };
                   
                    //Feedback 
                    return Ok(feedbackUsers);                    
                }
                catch (Exception ex)
                {                  
                    return BadRequest(ex.Message);                  
                }
            }
            else
            {
                var result = new Result();
                result.Status = Status.Error;
                result.Message = "Please select the file first to upload.";
                return BadRequest(result);              
            }
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostCSVStudent(IFormFile postedFile)
        {
            int lineIndex = 0;
            string line = null;
            string headerError = null; ;
            string dataError = null;
            var users = new List<User>();
            int newUsersCount = 0;
            int existingUsersCount = 0;
            ArrayList errorUsers = new ArrayList();
            ArrayList feedbackUsers = new ArrayList();
            var genericPassword = "AQAAAAEAACcQAAAAEPvBo3i7bpJn1faRTkFqaIQoS5B+ikTQfPR3TD8nbcy1aGItn2Z/kH8BLcqmS0SyWQ==";

            if (postedFile != null)
            {
                try
                {
                    string fileExtension = Path.GetExtension(postedFile.FileName);

                    //Validate uploaded file and return error.
                    if (fileExtension != ".csv")
                    {
                        var result = new Result();
                        result.Status = Status.Error;
                        result.Message = "Please select the csv file with .csv extension.";
                        return BadRequest(result);
                    }

                    //Read file + check headers
                    using (var sreader = new StreamReader(postedFile.OpenReadStream()))
                    {
                        //First line is header.
                        List<string> headers = sreader.ReadLine().Split(';').ToList();
                        int roleholder = 0;
                        int count = headers.Count();

                        //Student headers Check 
                        if (count == 10)
                        {
                            headerError = _repoWrapper.User.CheckStudentHeader(headers);
                            roleholder = 1;
                        }
                        //Headers Error 
                        else
                        {
                            headerError = "Headers van .csv-bestand bevat fouten.";
                        }
                        //Badrequest header
                        if (headerError != null)
                        {
                            var result = new Result();
                            result.Status = Status.Error;
                            result.Message = headerError;
                            return BadRequest(result);
                        }

                        //Loop through Student records
                        if (roleholder == 1)
                        {
                            lineIndex++;
                            while (!sreader.EndOfStream)
                            {
                                lineIndex++;
                                line = sreader.ReadLine();
                                int commas = line.Count(c => c == ';');
                                string[] rows = line.Split(';');

                                dataError = _repoWrapper.User.CheckStudentData(rows, commas);

                                if (dataError == null)
                                {
                                    users.Add(new User
                                    {
                                        UserFirstName = rows[0].ToString(),
                                        UserSurname = rows[1].ToString(),
                                        UserEmailAddress = rows[8].ToString(),
                                        RegistrationDate = DateTime.UtcNow,
                                        Activated = true,
                                        CvPresent = false,
                                        UserPass = genericPassword,
                                        RoleId = roleholder
                                    }) ;
                                }
                                else
                                {
                                    errorUsers.Add("regel " + lineIndex + " fout : " + dataError);
                                }
                            }
                        }
                    }

                    //Add Users
                    foreach (var User in users)
                    {
                        if (ModelState.IsValid)
                        {
                            IdentityResult result = null;
                            var user = await _userManager.FindByNameAsync(User.UserEmailAddress);

                            if (user != null)
                            {
                                existingUsersCount++;
                            }
                            else
                            {
                                _repoWrapper.User.Create(User);
                                _repoWrapper.Save();
                                newUsersCount++;
                            }
                        }
                    }
                    feedbackUsers.Add(newUsersCount + " nieuwe gebruikers succesvol toegevoegd.");
                    feedbackUsers.Add(existingUsersCount + " gebruiker(s) bestaan reeds.");
                    if (errorUsers.Count > 0)
                    {
                        feedbackUsers.Add("fouten :");
                        feedbackUsers.Add(errorUsers);
                    };

                    //Feedback 
                    return Ok(feedbackUsers);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                var result = new Result();
                result.Status = Status.Error;
                result.Message = "Please select the file first to upload.";
                return BadRequest(result);
            }
        }
    }
}