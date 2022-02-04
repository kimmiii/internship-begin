//using EmailService;
//using FluentAssertions;
//using Microsoft.AspNetCore.Mvc;
//using MockQueryable.Moq;
//using Moq;
//using NUnit.Framework;
//using StagebeheerAPI.Contracts;
//using StagebeheerAPI.Controllers;
//using StagebeheerAPI.Models;
//using StagebeheerAPI.ViewModels;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Threading.Tasks;

//namespace StagebeheerAPI.Test.Controllers.Unit
//{
//    class CompanyControllerTests
//    {
//        private CompanyController _CompanyController;

//        private Mock<IRepositoryWrapper> _RepositoryWrapperMock;
//        private Mock<IEmailSender> _EmailSenderMock;
//        private Company _TestCompany;

//        [SetUp]
//        public void Setup()
//        {
//            _RepositoryWrapperMock = new Mock<IRepositoryWrapper>();
//            _EmailSenderMock = new Mock<IEmailSender>();
//            _TestCompany = new Company
//            {               
//                Activated = true,
//                BusNr = "1",
//                City = "Hasselt",
//                Country = "Belgium",
//                Email = "info@company1.be",
//                HouseNr = "1",
//                Name = "Company1",
//                PhoneNumber = "+3211020304",
//                Street = "Grovestraat",
//                TotalEmployees = 10,
//                TotalITEmployees = 5,
//                TotalITEmployeesActive = 4,
//                EvaluatedAt = DateTime.Now,
//                VATNumber = "123.4565.789",
//                ZipCode = "3500",
//                UserId = 1
//            };
//        }

//        [Test]
//        public async Task PostCompany_ReturnsCreated_WhenPostingValidCompany()
//        {
//            // Arrange
//            _RepositoryWrapperMock.Setup(x => x.Company.IsValidVatNumber(It.IsAny<string>())).Returns(true);
//            _CompanyController = new CompanyController(_RepositoryWrapperMock.Object, _EmailSenderMock.Object);

//            // Act
//            var apiResult = await _CompanyController.PostCompany(_TestCompany);
//            var createdResult = apiResult.Result as CreatedAtActionResult;
//            var companyResult = createdResult.Value as Company;

//            // Assert
//            companyResult.Should().BeEquivalentTo(_TestCompany);
//        }

//        //[Test]
//        public async Task PostCompany_ReturnsBadRequest_WhenVatNumberAlreadyExists()
//        {
//            // Arrange
//            _RepositoryWrapperMock
//                .Setup(x => x.Company
//                .IsValidVatNumber(It.IsAny<string>()))
//                .Returns(false);
//            _CompanyController = new CompanyController(_RepositoryWrapperMock.Object, _EmailSenderMock.Object);

//            // Act
//            var apiResult = await _CompanyController.PostCompany(_TestCompany);
//            var badRequestResult = apiResult.Result as BadRequestObjectResult;

//            badRequestResult.Should().BeEquivalentTo(new BadRequestResult());
//        }

//        [Test]
//        public async Task GetCompanies_ReturnsCompanies_WhenCompaniesExist()
//        {
//            // Arrange
//            var testCompany = new Company
//            {
//                CompanyId = 1,
//                Activated = true,
//                BusNr = "1",
//                City = "Hasselt",
//                Country = "Belgium",
//                Email = "info@company1.be",
//                HouseNr = "1",
//                Name = "Company1",
//                PhoneNumber = "+3211020304",
//                Street = "Grovestraat",
//                TotalEmployees = 1,
//                TotalITEmployees = 1,
//                TotalITEmployeesActive = 1,
//                EvaluatedAt = DateTime.Now,
//                VATNumber = "123.4565.789",
//                ZipCode = "3500",
//                UserId = 1
//            };

//            var testCompanies = new List<Company>();
//            testCompanies.Add(testCompany);

//            var companyMock = testCompanies.AsQueryable().BuildMock();

//            _RepositoryWrapperMock.Setup(x => x.Company.FindAll()).Returns(companyMock.Object);

//            _CompanyController = new CompanyController(_RepositoryWrapperMock.Object, _EmailSenderMock.Object);

//            // Act
//            var apiResult = await _CompanyController.GetCompanies();
//            var CompanyResult = apiResult.Value as List<Company>;

//            // Assert
//            CompanyResult.Should().BeEquivalentTo(testCompanies);
//        }


//        [Test]
//         public async Task GetCompany_ReturnsUser()
//         {           
//            var testCompanys = new List<Company>();
//            testCompanys.Add(_TestCompany);
//            var contactsMock = testCompanys.AsQueryable().BuildMock();

//            _RepositoryWrapperMock.Setup(x => x.Company
//            .FindByCondition(It.IsAny<Expression<Func<Company, bool>>>()))
//                .Returns(contactsMock.Object);
         
//            _CompanyController = new CompanyController(_RepositoryWrapperMock.Object, _EmailSenderMock.Object);


//            var apiResult = await _CompanyController.GetCompany(_TestCompany.CompanyId);
//            var companyResult = apiResult.Value as Company;

//            companyResult.Should().BeEquivalentTo(_TestCompany);
       
//        }

//        [Test]
//        public async Task GetCompany_ReturnsNotFound()
//        {
//            var testCompanys = new List<Company>();
//            testCompanys.Add(_TestCompany);
//            var contactsMock = testCompanys.AsQueryable().BuildMock();
            
//            _RepositoryWrapperMock.Setup(x => x.Company
//            .FindByCondition(It.IsAny<Expression<Func<Company, bool>>>()))
//                .Returns(contactsMock.Object);
//            _CompanyController = new CompanyController(_RepositoryWrapperMock.Object, _EmailSenderMock.Object);

//            var apiResult = await _CompanyController.GetCompany(3);
//            var NotFoundResult = apiResult.Result as NotFoundResult;
//            NotFoundResult.Should().BeEquivalentTo(new NotFoundResult());
           
//        }




//        [Test]
//        public async Task ApproveCompany_WhenContactExists()
//        {
//            // Arrange
//            int testId = 123;
//            DateTime testTime = DateTime.Now;

//            Company TestCompany1 = new Company
//            {
//                CompanyId= testId,
//                Activated = true,
//                BusNr = "1",
//                City = "Hasselt",
//                Country = "Belgium",
//                Email = "info@company1.be",
//                HouseNr = "1",
//                Name = "Company1",
//                PhoneNumber = "+3211020304",
//                Street = "Grovestraat",
//                TotalEmployees = 1,
//                TotalITEmployees = 1,
//                TotalITEmployeesActive = 1,
//                EvaluatedAt = testTime,
//                VATNumber = "123.4565.789",
//                ZipCode = "3500",
//                UserId = 1
//            };

//            var testCompanys = new List<Company>();
//            testCompanys.Add(TestCompany1);
//            var contactsMock = testCompanys.AsQueryable().BuildMock();

//            List<string> mailTo = new List<string>();
//            var result = new Result();

//            _RepositoryWrapperMock.Setup(x => x.Company
//         .FindByCondition(It.IsAny<Expression<Func<Company, bool>>>()))
//             .Returns(contactsMock.Object);

//            _CompanyController = new CompanyController(_RepositoryWrapperMock.Object, _EmailSenderMock.Object);


//            // Act
//            var apiResult = await _CompanyController.ApproveCompany(testId);
//            apiResult.Should().BeEquivalentTo(new NoContentResult());


//          /*  var apiResultcheck = await _CompanyController.GetCompany(testId);
//            var createdResult = apiResultcheck.Result as CreatedAtActionResult;
//            var companyResult = createdResult.Value as Company;

//            companyResult.Activated.Should().Be(true);
//            companyResult.EvaluatedAt.Should().NotBeSameDateAs(testTime);
//          */

//        }

//        [Test]
//        public async Task ApproveCompany_WhenContactNotExists()
//        {
//            // Arrange
//            int testId = 123;
//            DateTime testTime = DateTime.Now;

//            Company TestCompany1 = new Company
//            {
//                CompanyId = testId,
//                Activated = true,
//                BusNr = "1",
//                City = "Hasselt",
//                Country = "Belgium",
//                Email = "info@company1.be",
//                HouseNr = "1",
//                Name = "Company1",
//                PhoneNumber = "+3211020304",
//                Street = "Grovestraat",
//                TotalEmployees = 1,
//                TotalITEmployees = 1,
//                TotalITEmployeesActive = 1,
//                EvaluatedAt = testTime,
//                VATNumber = "123.4565.789",
//                ZipCode = "3500",
//                UserId = 1
//            };

//            var testCompanys = new List<Company>();
//            testCompanys.Add(TestCompany1);
//            var contactsMock = testCompanys.AsQueryable().BuildMock();

//            List<string> mailTo = new List<string>();
//            var result = new Result();

//            _RepositoryWrapperMock.Setup(x => x.Company
//         .FindByCondition(It.IsAny<Expression<Func<Company, bool>>>()))
//             .Returns(contactsMock.Object);

//            _CompanyController = new CompanyController(_RepositoryWrapperMock.Object, _EmailSenderMock.Object);


//            // Act
//            var apiResult = await _CompanyController.GetCompany(3);
//            var NotFoundResult = apiResult.Result as NotFoundResult;
//            NotFoundResult.Should().BeEquivalentTo(new NotFoundResult());

//        }

//        [Test]
//        public async Task RejectCompany_WhenContactExists()
//        {
//            // Arrange
//            int testId = 123;
//            DateTime testTime = DateTime.Now;

//            Company TestCompany1 = new Company
//            {
//                CompanyId = testId,
//                Activated = true,
//                BusNr = "1",
//                City = "Hasselt",
//                Country = "Belgium",
//                Email = "info@company1.be",
//                HouseNr = "1",
//                Name = "Company1",
//                PhoneNumber = "+3211020304",
//                Street = "Grovestraat",
//                TotalEmployees = 1,
//                TotalITEmployees = 1,
//                TotalITEmployeesActive = 1,
//                EvaluatedAt = testTime,
//                VATNumber = "123.4565.789",
//                ZipCode = "3500",
//                UserId = 1
//            };

//            var testCompanys = new List<Company>();
//            testCompanys.Add(TestCompany1);
//            var contactsMock = testCompanys.AsQueryable().BuildMock();

//            List<string> mailTo = new List<string>();
//            var result = new Result();

//            _RepositoryWrapperMock.Setup(x => x.Company
//         .FindByCondition(It.IsAny<Expression<Func<Company, bool>>>()))
//             .Returns(contactsMock.Object);

//            _CompanyController = new CompanyController(_RepositoryWrapperMock.Object, _EmailSenderMock.Object);


//            // Act
//            var apiResult = await _CompanyController.RejectCompany(testId,"test");
//            apiResult.Should().BeEquivalentTo(new NoContentResult());
//        }

//        [Test]
//        public async Task RejectCompany_WhenContactNotExists()
//        {
//            // Arrange
//            DateTime testTime = DateTime.Now;

//            var testCompanys = new List<Company>();
//            testCompanys.Add(_TestCompany);
//            var contactsMock = testCompanys.AsQueryable().BuildMock();

//            List<string> mailTo = new List<string>();
//            var result = new Result();

//            _RepositoryWrapperMock.Setup(x => x.Company
//         .FindByCondition(It.IsAny<Expression<Func<Company, bool>>>()))
//             .Returns(contactsMock.Object);

//            _CompanyController = new CompanyController(_RepositoryWrapperMock.Object, _EmailSenderMock.Object);

//            // Act

//            var apiResult = await _CompanyController.RejectCompany(3,"test");
//            apiResult.Should().BeEquivalentTo(new NotFoundResult());

//        }


//    }
//}
