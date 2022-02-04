using EmailService;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using StagebeheerAPI.Contracts;
using StagebeheerAPI.Controllers;
using StagebeheerAPI.Models;
using StagebeheerAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StagebeheerAPI.Test.Controllers.Unit
{
    public class ContactControllerTests
    {
        private ContactController _ContactController;

        private Mock<IRepositoryWrapper> _RepositoryWrapperMock;
        private Mock<IEmailSender> _EmailSenderMock;

        [SetUp]
        public void Setup()
        {
            _RepositoryWrapperMock = new Mock<IRepositoryWrapper>();
            _EmailSenderMock = new Mock<IEmailSender>();
        }

        [Test]
        public async Task Post_ReturnsSuccessfulResult_WhenPostingValidContact()
        {
            // Arrange
            var testContact = SetupContact();

            _RepositoryWrapperMock.Setup(x => x.Contact.ContactValidEmail(It.IsAny<string>())).Returns(true);
            _RepositoryWrapperMock.Setup(x => x.Contact.ContactValidPhoneNumber(It.IsAny<string>())).Returns(true);

            _ContactController = new ContactController(_RepositoryWrapperMock.Object, _EmailSenderMock.Object);

            // Act
            var apiResult = await _ContactController.PostInternship(testContact);
            var createdResult = apiResult.Result as CreatedAtActionResult;
            var contactResult = createdResult.Value as Contact;

            // Assert
            contactResult.Should().BeEquivalentTo(testContact);
        }

        //[Test]
        public async Task Post_ReturnsBadRequestMessage_WhenUsingExistingPhoneNumber()
        {
            // Arrange
            var testContact = SetupContact();
            
            _RepositoryWrapperMock.Setup(x => x.Contact.ContactValidEmail(It.IsAny<string>())).Returns(true);
            _RepositoryWrapperMock.Setup(x => x.Contact.ContactValidPhoneNumber(It.IsAny<string>())).Returns(false);

            _ContactController = new ContactController(_RepositoryWrapperMock.Object, _EmailSenderMock.Object);

            // Act
            var apiResult = await _ContactController.PostInternship(testContact);
            var badRequest = apiResult.Result as BadRequestObjectResult;
            var errorResult = badRequest.Value as Result;
            // Assert
            
            errorResult.Status.Should().BeEquivalentTo(Status.Error);
            errorResult.Message.Should().BeEquivalentTo("Telefoon Nummer is reeds in gebruik.");
            badRequest.Should().BeEquivalentTo(new BadRequestObjectResult(errorResult));
        }

        //[Test]
        public async Task Post_ReturnsBadRequestMessage_WhenUsingExistingEmail()
        {
            // Arrange
            var testContact = SetupContact();

            _RepositoryWrapperMock.Setup(x => x.Contact.ContactValidEmail(It.IsAny<string>())).Returns(false);
            _RepositoryWrapperMock.Setup(x => x.Contact.ContactValidPhoneNumber(It.IsAny<string>())).Returns(true);

            _ContactController = new ContactController(_RepositoryWrapperMock.Object, _EmailSenderMock.Object);

            // Act
            var apiResult = await _ContactController.PostInternship(testContact);
            var badRequest = apiResult.Result as BadRequestObjectResult;
            var errorResult = badRequest.Value as Result;
            // Assert
            
            errorResult.Status.Should().BeEquivalentTo(Status.Error);
            errorResult.Message.Should().BeEquivalentTo("E-Mail is reeds in gebruik.");
            badRequest.Should().BeEquivalentTo(new BadRequestObjectResult(errorResult));
        }

        [Test]
        public async Task GetContactById_ReturnsContact_WhenContactExists()
        {
            // Arrange
            int testId = 123;

            var testContact = SetupContact();

            var testContacts = new List<Contact>();
            testContacts.Add(testContact);
           
            var contactsMock = testContacts.AsQueryable().BuildMock();
            
            _RepositoryWrapperMock.Setup(x => x.Contact.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>())).Returns(contactsMock.Object);

            _ContactController = new ContactController(_RepositoryWrapperMock.Object, _EmailSenderMock.Object);

            // Act
            var apiResult = await _ContactController.GetContact(testId);
            var contactResult = apiResult.Value as Contact;
            
            // Assert
            contactResult.Should().BeEquivalentTo(testContact);
        }

        [Test]
        public async Task GetContactByCompanyId_ReturnsContacts_WhenContactsExist()
        {
            // Arrange
            int testCompanyId = 1;

            var testContact = SetupContact();

            var testContacts = new List<Contact>();
            testContacts.Add(testContact);

            var contactsMock = testContacts.AsQueryable().BuildMock();

            _RepositoryWrapperMock.Setup(x => x.Contact.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>())).Returns(contactsMock.Object.Where(x => x.CompanyId == testCompanyId));

            _ContactController = new ContactController(_RepositoryWrapperMock.Object, _EmailSenderMock.Object);

            // Act
            var apiResult = await _ContactController.GetContactByCompany(testCompanyId);
            var contactResult = apiResult.Value as List<Contact>;

            // Assert
            contactResult.Should().BeEquivalentTo(testContacts);
        }

        [Test]
        public async Task GetContactByCompanyId_ReturnsEmptyList_WhenContactDoNotExist()
        {
            // Arrange
            int testCompanyId = 2;

            var testContact = SetupContact();

            var testContacts = new List<Contact>();
            testContacts.Add(testContact);

            var contactsMock = testContacts.AsQueryable().BuildMock();

            _RepositoryWrapperMock.Setup(x => x.Contact.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>())).Returns(contactsMock.Object.Where(x => x.CompanyId == testCompanyId));

            _ContactController = new ContactController(_RepositoryWrapperMock.Object, _EmailSenderMock.Object);

            // Act
            var apiResult = await _ContactController.GetContactByCompany(testCompanyId);
            var contactResult = apiResult.Value as List<Contact>;

            // Assert
            contactResult.Should().BeEquivalentTo(new List<Contact>());
        }
        [Test]
        public async Task GetContactById_ReturnsNotFound_WhenContactDoesNotExist()
        {
            // Arrange
            int testId = 124;

            var testContact = SetupContact();

            var testContacts = new List<Contact>();
            testContacts.Add(testContact);
            //testContacts = null;

            var contactsMock = testContacts.AsQueryable().BuildMock();
            //var contacttester = contactsMock.Object;
            //contacttester = null;

            _RepositoryWrapperMock.Setup(x => x.Contact.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>())).Returns(contactsMock.Object);

            _ContactController = new ContactController(_RepositoryWrapperMock.Object, _EmailSenderMock.Object);

            // Act
            var apiResult = await _ContactController.GetContact(testId);
            var notFoundResult = apiResult.Result as NotFoundResult;

            // Assert
            notFoundResult.Should().BeEquivalentTo(new NotFoundResult());
        }

        [Test]
        public async Task Put_ReturnsBadRequestMessage_WhenUsingWrongId()
        {
            // Arrange
            var testId = 124;
            var testContact = SetupContact();

            _ContactController = new ContactController(_RepositoryWrapperMock.Object, _EmailSenderMock.Object);

            // Act
            var apiResult = await _ContactController.PutContact(testId, testContact);
            var badRequest = apiResult as BadRequestResult;
            //var errorResult = badRequest.Value as Result;
            // Assert

            badRequest.Should().BeEquivalentTo(new BadRequestResult());
        }

        [Test]
        public async Task Put_GivesNotFoundResult_WhenContactDoesNotExist()
        {
            // Arrange
            var testContact = SetupContact();

            var testContact2 = SetupContact();
            testContact2.ContactId = 124;

            var testContacts = new List<Contact>();
            testContacts.Add(testContact);

            var contactsMock = testContacts.AsQueryable().BuildMock();
            var testMockObject = contactsMock.Object;

            _RepositoryWrapperMock.Setup(x => x.Contact.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>())).Returns(testMockObject);

            _ContactController = new ContactController(_RepositoryWrapperMock.Object, _EmailSenderMock.Object);

            // Act
            var apiResult = await _ContactController.PutContact(testContact2.ContactId ,testContact2);
            var contactFoundResult = apiResult as NotFoundResult;

            //Assert            
            contactFoundResult.Should().BeEquivalentTo(new NotFoundResult());
        }

        [Test]
        public async Task Put_ReturnsNoContent_WhenContactExists()
        {
            // Arrange
            var testContact = SetupContact();

            var testContact2 = SetupContact();
            testContact2.ContactId = 124;

            var testContacts = new List<Contact>();
            testContacts.Add(testContact);
            testContacts.Add(testContact2);

            var contactsMock = testContacts.AsQueryable().BuildMock();
            var testMockObject = contactsMock.Object;

            _RepositoryWrapperMock.Setup(x => x.Contact.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>())).Returns(testMockObject);

            _ContactController = new ContactController(_RepositoryWrapperMock.Object, _EmailSenderMock.Object);

            // Act
            var apiResult = await _ContactController.PutContact(testContact2.ContactId, testContact2);
            //var contactFoundResult = apiResult as NotFoundResult;

            //Assert            
            apiResult.Should().BeEquivalentTo(new NoContentResult());
        }

        [Test]
        public async Task SoftdeleteContact_GivesNotFoundResult_WhenContactDoesNotExist()
        {
            // Arrange
            int testId = 124;

            var testContact = SetupContact();
            var testContacts = new List<Contact>();
            testContacts.Add(testContact);

            var contactsMock = testContacts.AsQueryable().BuildMock();
            var testMockObject = contactsMock.Object;

            _RepositoryWrapperMock.Setup(x => x.Contact.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>())).Returns(testMockObject);

            _ContactController = new ContactController(_RepositoryWrapperMock.Object, _EmailSenderMock.Object);

            // Act
            var apiResult = await _ContactController.SoftDeleteContact(testId);
            var contactFoundResult = apiResult as NotFoundResult;
            //Assert
            contactFoundResult.Should().BeEquivalentTo(new NotFoundResult());
        }

        [Test]
        public async Task SoftdeleteContact_ReturnsNoContent_WhenContactExists()
        {
            // Arrange
            int testId = 123;
            var testContact = SetupContact();

            var testContacts = new List<Contact>();
            testContacts.Add(testContact);

            var contactsMock = testContacts.AsQueryable().BuildMock();

            _RepositoryWrapperMock.Setup(x => x.Contact.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>())).Returns(contactsMock.Object);

            _ContactController = new ContactController(_RepositoryWrapperMock.Object, _EmailSenderMock.Object);

            // Act
            var apiResult = await _ContactController.SoftDeleteContact(testId);

            //Assert
            apiResult.Should().BeEquivalentTo(new NoContentResult());
        }

        //[Test]
        //public async Task ContactExists_ReturnsTrue_WhenContactExists()
        //{
        //    // Arrange
        //    int testId = 123;
        //    var testContact = SetupContact();

        //    var testContacts = new List<Contact>();
        //    testContacts.Add(testContact);

        //    var contactsMock = testContacts.AsQueryable().BuildMock();

        //    _RepositoryWrapperMock.Setup(x => x.Contact.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>())).Returns(contactsMock.Object);

        //    _ContactController = new ContactController(_RepositoryWrapperMock.Object);

        //    // Act
        //    var apiResult = _ContactController.ContactExists(testId);
        //    //var ContactFoundResult = apiResult as Contact;

        //    //Assert
        //    apiResult.Should().BeTrue();
        //}

        private Contact SetupContact()
        {
            var testContact = new Contact
            {
                ContactId = 123,
                Firstname = "Albert",
                Surname = "Adelstein",
                PhoneNumber = "+32476112233",
                Email = $"switch.to.it.test@gmail.com",
                Function = "Contact person",
                CompanyId = 1,
                Activated = true
            };


            return testContact;
        }
    }
    }

