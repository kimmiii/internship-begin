using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using StagebeheerAPI.Controllers;
using StagebeheerAPI.Models;
using StagebeheerAPI.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System;
using StagebeheerAPI.Contracts;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace StagebeheerAPI.Test.Controllers
{
    public class AccountControllerTests
    {
        private AccountController _AccountController;

        private Mock<UserManager<User>> _UserManagerMock;
        private Mock<IRepositoryWrapper> _RepositoryWrapperMock;
        private Mock<ITokenService> _TokenService;

        [SetUp]
        public void Setup()
        {
            _UserManagerMock = _MockUserManager<User>();
            _RepositoryWrapperMock = new Mock<IRepositoryWrapper>();
            _TokenService = new Mock<ITokenService>();

            _AccountController = new AccountController(_UserManagerMock.Object, _RepositoryWrapperMock.Object, _TokenService.Object);
            _MockHttpContext();
        }

        //[Test]
        //public async Task Login_ReturnsSuccessfulResult_WhenLoginCredentialsAreCorrectAsync()
        //{
        //    // Arrange
        //    var loginDetails = new Login {UserEmailAddress = "testuser@hotmail.com", Password = "P4ssw0rd123" };

        //    _UserManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
        //        .ReturnsAsync(new User { 
        //            UserId = 1,
        //            UserEmailAddress = loginDetails.UserEmailAddress,
        //            RoleId = 1,
        //            Role = new Role { Code = "COM", Description = "Stagebedrijf" },
        //            Activated = true,
        //            Company = new Company { CompanyId = 5, Activated = true },
        //        });

        //    _UserManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
        //        .ReturnsAsync(true);

        //    _TokenService.Setup(x => x.GenerateJSONWebToken("issuer", "key")).Returns("token");



        //    // Act
        //    var apiResult = await _AccountController.Login(loginDetails);
        //    var okObjectResult = apiResult.Result as OkObjectResult;
        //    var userResult = okObjectResult.Value as Models.ApiModels.User;

        //    // Assert
        //    userResult.UserId.Should().Be(1);
        //    userResult.UserEmailAddress.Should().BeEquivalentTo("testuser@hotmail.com");
        //    userResult.RoleId.Should().Be(1);
        //    userResult.RoleCode.Should().Be("COM");
        //    userResult.RoleDescription.Should().Be("Stagebedrijf");
        //    userResult.IsUserActivated.Should().Be(true);
        //    userResult.CompanyId.Should().Be(5);
        //    userResult.IsCompanyActivated.Should().Be(true);
        //}

        [Test]
        public async Task Login_ReturnsUnauthorized_WhenUserDoesntExist()
        {
            // Arrange
            var loginDetails = new Login { UserEmailAddress = "NotExistingUser@hotmail.com", Password = "P4ssw0rd123" };

            _UserManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(null, new TimeSpan(1));

            // Act
            var apiResult = await _AccountController.Login(loginDetails);
            var unauthorizedResult = apiResult.Result as UnauthorizedResult;

            // Assert
            unauthorizedResult.Should().BeEquivalentTo(new UnauthorizedResult());
        }

        [Test]
        public async Task Login_ReturnsUnauthorized_WhenPasswordIsIncorrecrt()
        {
            // Arrange
            var loginDetails = new Login { UserEmailAddress = "testuser@hotmail.com", Password = "Wr0ngP4ssw0rd" };

            _UserManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new User
                {
                    UserId = 1,
                    UserEmailAddress = loginDetails.UserEmailAddress,
                    RoleId = 1,
                    Role = new Role { Code = "COM", Description = "Stagebedrijf" },
                    Activated = true,
                    Company = new Company { CompanyId = 5, Activated = true },
                });

            _UserManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var apiResult = await _AccountController.Login(loginDetails);
            var unauthorizedResult = apiResult.Result as UnauthorizedResult;

            // Assert
            unauthorizedResult.Should().BeEquivalentTo(new UnauthorizedResult());
        }

        [Test]
        public async Task CompanyRegister_RegistersUser_WhenCallIsValid()
        {
            // Arrange
            var registerDetails = new Register { UserEmailAddress = "testuser@hotmail.com", UserPass = "Password", ConfirmPassword = "Password"};

            _UserManagerMock.SetupSequence(x => x.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<User>(null))
                .Returns(Task.FromResult<User>(new User {
                    UserId = 1,
                    UserEmailAddress = registerDetails.UserEmailAddress,
                    RoleId = 1,
                    Activated = true,
                }));

            _RepositoryWrapperMock.Setup(x => x.Role.FindAll()).Returns(new List<Role> { new Role { RoleId = 1, Code = "COM" } }.AsQueryable());

            _UserManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));

            // Act
            var apiResult = await _AccountController.Register(registerDetails);
            var createdResult = apiResult.Result as CreatedAtActionResult;
            var userResult = createdResult.Value as Models.ApiModels.User;

            // Assert
            userResult.UserId.Should().Be(1);
            userResult.UserEmailAddress.Should().Be("testuser@hotmail.com");
            userResult.RoleId.Should().Be(1);
            userResult.IsUserActivated.Should().Be(true);
        }

        [Test]
        public async Task CompanyRegister_ReturnsConflict_WhenUserAlreadyExists()
        {
            // Arrange
            var registerDetails = new Register { UserEmailAddress = "testuser@hotmail.com", UserPass = "Password", ConfirmPassword = "Password" };

            _UserManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<User>(new User
                {
                    UserId = 1,
                    UserEmailAddress = registerDetails.UserEmailAddress
                }));

            _RepositoryWrapperMock.Setup(x => x.Role.FindAll()).Returns(new List<Role> { new Role { RoleId = 1, Code = "COM" } }.AsQueryable());

            // Act
            var apiResult = await _AccountController.Register(registerDetails);
            var conflictResult = apiResult.Result as ConflictResult;

            // Assert
            conflictResult.Should().BeEquivalentTo(new ConflictResult());
        }

        [Test]
        public async Task PostCSV_ValidStudent()
        {
            _RepositoryWrapperMock.Setup(x => x.User.CheckStudentData(It.IsAny<string[]>(), It.IsAny<int>())).Returns(() => null);
            string path = new DirectoryInfo(System.Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
            using (var stream = File.OpenRead(path + "\\Data\\TestStudentValid.csv"))
            {
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "text/csv"
                };

                IFormFile postedFile = file;
                var apiResult = await _AccountController.PostCSVStudent(postedFile);
                apiResult.Should().Equals(200);
            }
        }
       
        [Test]
        public async Task PostCSV_ValidReviewer()
        {
            _RepositoryWrapperMock.Setup(x => x.User.CheckReviewerData(It.IsAny<string[]>(), It.IsAny<int>())).Returns(() => null);
            string path = new DirectoryInfo(System.Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
            using (var stream = File.OpenRead(path + "\\Data\\TestReviewerValid.csv"))
            {
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "text/csv"
                };

                IFormFile postedFile = file;
                var apiResult = await _AccountController.PostCSVLector(postedFile);
                apiResult.Should().Equals(200);
            }
        }

        [Test]
        public async Task PostCSV_HeaderNotMatchPattern()
        {
            _RepositoryWrapperMock.Setup(x => x.User.CheckReviewerData(It.IsAny<string[]>(), It.IsAny<int>())).Returns(() => null);
            string path = new DirectoryInfo(System.Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
            using (var stream = File.OpenRead(path + "\\Data\\TestHeaderNotMatchPattern.csv"))
            {
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "text/csv"
                };

                IFormFile postedFile = file;
                var apiResult = await _AccountController.PostCSVStudent(postedFile);
                apiResult.Should().Equals(400);                
            }

        }

        [Test]
        public async Task PostCSV_WhenCheckReviewerHeaderIsValid()
        {
          //  _RepositoryWrapperMock.Setup(x => x.User.CheckReviewerHeader(It.IsAny<List<string>>())).Returns(() => null);
     
            string path = new DirectoryInfo(System.Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
            using (var stream = File.OpenRead(path + "\\Data\\TestStudentValid.csv"))
            {
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "text/csv"
                };

                IFormFile postedFile = file;
                var apiResult = await _AccountController.PostCSVLector(postedFile);
                apiResult.Should().Equals(200);
            }
        }

        [Test]
        public async Task PostCSV_WhenCheckReviewerHeaderIsNotValid()
        {
            string path = new DirectoryInfo(System.Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
            using (var stream = File.OpenRead(path + "\\Data\\TestReviewerNotValid.csv"))
            {
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "text/csv"
                };

                IFormFile postedFile = file;
                var apiResult = await _AccountController.PostCSVLector(postedFile);
                apiResult.Should().Equals(200);
            }
        }

        [Test]
        public async Task PostCSV_WhenCheckStudentHeaderIsValid()
        {
            _RepositoryWrapperMock.Setup(x => x.User.CheckStudentHeader(It.IsAny<List<string>>())).Returns(() => null);
        }

        [Test]
        public async Task PostCSV_WhenCheckStudentHeaderIsNotValid()
        {
            _RepositoryWrapperMock.Setup(x => x.User.CheckStudentHeader(It.IsAny<List<string>>())).Returns(() => null);
        }

        [Test]
        public async Task PostCSV_NoFileSelected()
        {
                IFormFile postedFile = null;
                var apiResult = await _AccountController.PostCSVStudent(postedFile);
                apiResult.Should().Equals(400);
        }

        private Mock<UserManager<TUser>> _MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());
            return mgr;
        }

        private void _MockHttpContext()
        {
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            authenticationServiceMock
                .Setup(a => a.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(s => s.GetService(typeof(IAuthenticationService)))
                .Returns(authenticationServiceMock.Object);

            _AccountController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
                {
                    RequestServices = serviceProviderMock.Object
                }
            };
        }
    }
}
