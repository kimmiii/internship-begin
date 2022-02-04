using System;
using EventAPI.BLL;
using EventAPI.Controllers;
using EventAPI.Domain.ViewModels;
using EventAPI.Tests.Builders;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using EventAPI.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace EventAPI.Tests.Controllers
{
    class EventsControllerTests
    {
        private EventsController _controller;
        private Mock<IEventBll> _eventBll;

        [SetUp]
        public void Setup()
        {
            _eventBll = new Mock<IEventBll>();
            _controller = new EventsController(_eventBll.Object);
        }

        [Test]
        public void GetEventsAsync_ReturnsAllEvents()
        {
            // Arrange
            var eventVms = new List<EventVM>();
            _eventBll.Setup(b => b.GetAsyncVm()).ReturnsAsync(eventVms);

            // Act
            var result = _controller.GetEventsAsync().Result as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EquivalentTo(eventVms));
            _eventBll.Verify(r => r.GetAsyncVm(), Times.Once);
        }

        [Test]
        public void GetActiveEventAsync_ReturnsActiveEvent()
        {
            // Arrange
            var eventVm = new EventVmBuilder().WithId.Build;
            _eventBll.Setup(b => b.GetActiveAsyncVm()).ReturnsAsync(eventVm);

            // Act
            var result = _controller.GetActiveEventAsync().Result as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.SameAs(eventVm));
            _eventBll.Verify(r => r.GetActiveAsyncVm(), Times.Once);
        }

        [Test]
        public void GetActiveEventAsync_ReturnsNotFoundWhenNoActiveEvent()
        {
            // Arrange
            _eventBll.Setup(b => b.GetActiveAsyncVm()).ReturnsAsync(() => null);

            // Act
            var result = _controller.GetActiveEventAsync().Result as NoContentResult; ;

            // Assert
            Assert.That(result, Is.Not.Null);
            _eventBll.Verify(r => r.GetActiveAsyncVm(), Times.Once);
        }

        [Test]
        public void CreateEventAsync_CreatesAnValidEvent()
        {
            // Arrange
            var eventVm = new EventVmBuilder().Build;
            _eventBll.Setup(b => b.CreateAsyncVm(eventVm)).ReturnsAsync(eventVm);

            // Act
            var result = _controller.CreateEventAsync(eventVm).Result as OkObjectResult; ;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.SameAs(eventVm));
            _eventBll.Verify(r => r.CreateAsyncVm(It.IsAny<EventVM>()), Times.Once);
        }

        [Test]
        public void CreateEventAsync_ReturnsBadRequestWithInvalidEvent()
        {
            // Act
            var result = _controller.CreateEventAsync(null).Result as BadRequestResult; 

            // Assert
            Assert.That(result, Is.Not.Null);
            _eventBll.Verify(r => r.CreateAsyncVm(It.IsAny<EventVM>()), Times.Never);
        }

        [Test]
        public void CreateEventAsync_ReturnsBadRequestWithInvalidModelState()
        {
            //Arrange
            var e = new EventVmBuilder().WithEmptyName.Build;
            _controller.ModelState.AddModelError("Name", "Name is required");

            //Act
            var result = _controller.CreateEventAsync(e).Result as BadRequestResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            _eventBll.Verify(repo => repo.CreateAsyncVm(It.IsAny<EventVM>()), Times.Never);
        }

        [Test]
        public void UpdateEventAsync_UpdatesValidEvent()
        {
            // Arrange
            var eventVm = new EventVmBuilder().WithId.Build; 
            _eventBll.Setup(b => b.UpdateAsyncVm(eventVm)).ReturnsAsync(eventVm);

            // Act
            var result = _controller.UpdateEventAsync(eventVm).Result as OkObjectResult; ;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.SameAs(eventVm));
            _eventBll.Verify(r => r.UpdateAsyncVm(It.IsAny<EventVM>()), Times.Once);
        }

        [Test]
        public void UpdateEventAsync_ReturnsBadRequestWithInvalidParameter()
        {
            // Act
            var result = _controller.UpdateEventAsync(null).Result as BadRequestResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            _eventBll.Verify(r => r.UpdateAsyncVm(It.IsAny<EventVM>()), Times.Never);
        }

        [Test]
        public void GetCompaniesFromActiveEventAsync_ReturnsCompaniesFromActiveEvent()
        {
            // Arrange
            var eventCompanyVMs = new List<EventCompanyVM>();
            _eventBll.Setup(b => b.GetCompaniesFromActiveEventAsyncVm()).ReturnsAsync(eventCompanyVMs);

            // Act
            var result = _controller.GetCompaniesFromActiveEventAsync().Result as OkObjectResult; ;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EquivalentTo(eventCompanyVMs));
            _eventBll.Verify(r => r.GetCompaniesFromActiveEventAsyncVm(), Times.Once);
        }


        [Test]
        public void GetCompanyFromActiveEventAsync_ReturnsDetailFromEventCompany()
        {
            // Arrange
            var eventCompanyVm = new EventCompanyVMBuilder().Build;
            _eventBll.Setup(b => b.GetCompanyFromActiveEventAsyncVm()).ReturnsAsync(eventCompanyVm);

            // Act
            var result = _controller.GetCompanyFromActiveEventAsync().Result as OkObjectResult; ;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.SameAs(eventCompanyVm));
            _eventBll.Verify(r => r.GetCompanyFromActiveEventAsyncVm(), Times.Once);
        }

        [Test]
        public void GetCompanyFromActiveEventByIdAsync()
        {
            // Arrange
            var eventCompanyVm = new EventCompanyVMBuilder().Build;
            _eventBll.Setup(b => b.GetCompanyFromActiveEventByIdAsyncVm(eventCompanyVm.CompanyId)).ReturnsAsync(eventCompanyVm);

            // Act
            var result = _controller.GetCompanyFromActiveEventByIdAsync(eventCompanyVm.CompanyId).Result as OkObjectResult; ;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.SameAs(eventCompanyVm));
            _eventBll.Verify(r => r.GetCompanyFromActiveEventByIdAsyncVm(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void CreateEventCompany_CreatesValidEventCompany()
        {
            // Arrange
            var eventCompanyVm = new EventCompanyVMBuilder().Build;
            _eventBll.Setup(b => b.CreateEventCompanyAsyncVm(eventCompanyVm)).ReturnsAsync(eventCompanyVm);

            // Act
            var result = _controller.CreateEventCompany(eventCompanyVm).Result as OkObjectResult; ;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.SameAs(eventCompanyVm));
            _eventBll.Verify(r => r.CreateEventCompanyAsyncVm(It.IsAny<EventCompanyVM>()), Times.Once);
        }

        [Test]
        public void CreateEventCompany_ReturnsBadRequestWithInvalidParameter()
        {
            // Act
            var result = _controller.CreateEventCompany(null).Result as BadRequestResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            _eventBll.Verify(r => r.CreateEventCompanyAsyncVm(It.IsAny<EventCompanyVM>()), Times.Never);
        }

        [Test]
        public void GetInternshipsByEventIdAsync_ReturnsinternshipsFromEvent()
        {
            // Arrange
            var internshipVm = new List<InternshipVM>();
            _eventBll.Setup(b => b.GetInternshipsFromActiveEventAsyncVm()).ReturnsAsync(internshipVm);

            // Act
            var result = _controller.GetInternshipsByEventIdAsync().Result as OkObjectResult; ;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EquivalentTo(internshipVm));
            _eventBll.Verify(r => r.GetInternshipsFromActiveEventAsyncVm(), Times.Once);
        }

        [Test]
        public void GetInternshipsByCompanyFromActiveEventAsync_ReturnsInternshipsByCompanyFromActiveEvent()
        {
            // Arrange
            var internshipVm = new List<InternshipVM>();
            _eventBll.Setup(b => b.GetInternshipsByCompanyFromActiveEventAsyncVm()).ReturnsAsync(internshipVm);

            // Act
            var result = _controller.GetInternshipsByCompanyFromActiveEventAsync().Result as OkObjectResult; ;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EquivalentTo(internshipVm));
            _eventBll.Verify(r => r.GetInternshipsByCompanyFromActiveEventAsyncVm(), Times.Once);
        }

        [Test]
        public void GetAppointmentsByIdAsync_ReturnsAppointmentsDetailVM()
        {
            // Arrange
            var appointmentVM = new AppointmentVM();
            _eventBll.Setup(b => b.GetAppointmentsByIdAsyncVm(appointmentVM.Id)).ReturnsAsync(appointmentVM);

            // Act
            var result = _controller.GetAppointmentsByIdAsync(appointmentVM.Id).Result as OkObjectResult; ;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.SameAs(appointmentVM));
            _eventBll.Verify(r => r.GetAppointmentsByIdAsyncVm(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void GetAppointmentsFromCompanyAsync_ReturnsAppointmentsVM()
        {
            // Arrange
            var status = new List<string>();
            status.Add(AppointmentStatus.CONFIRMED.ToString());
            var statusString = AppointmentStatus.CONFIRMED.ToString();
            var appointmentVMs = new List<AppointmentVM>();
            _eventBll.Setup(b => b.GetAllAppointmentsFromCompanyByStatusAsyncVm(status)).ReturnsAsync(appointmentVMs);

            // Act
            var result = _controller.GetAppointmentsFromCompanyAsync(statusString).Result as OkObjectResult; ;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EquivalentTo(appointmentVMs));
            _eventBll.Verify(r => r.GetAllAppointmentsFromCompanyByStatusAsyncVm(It.IsAny<List<string>>()), Times.Once);
        }

        [Test]
        public void GetAppointmentsByCompanyAsync_ReturnsAppointmentsVMByCompany()
        {
            // Arrange
            var status = new List<string>();
            status.Add(AppointmentStatus.CONFIRMED.ToString());
            var statusString = AppointmentStatus.CONFIRMED.ToString();
            var id = new Random().Next();
            var appointmentVMs = new List<AppointmentWithoutStudentDataVM>();
            _eventBll.Setup(b => b.GetAppointmentsByCompanyByStatusAsyncVm(id, status)).ReturnsAsync(appointmentVMs);

            // Act
            var result = _controller.GetAppointmentsByCompanyAsync(id, statusString).Result as OkObjectResult; ;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EquivalentTo(appointmentVMs));
            _eventBll.Verify(r => r.GetAppointmentsByCompanyByStatusAsyncVm(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Once);
        }

        [Test]
        public void GetAppointmentsFromStudentAsync_ReturnsAppointmentsVMFromStudent()
        {
            // Arrange
            var status = new List<string>();
            status.Add(AppointmentStatus.CONFIRMED.ToString());
            var statusString = AppointmentStatus.CONFIRMED.ToString();
            var appointmentVMs = new List<AppointmentVM>();
            _eventBll.Setup(b => b.GetAllAppointmentsForStudentByStatusAsyncVm(status)).ReturnsAsync(appointmentVMs);

            // Act
            var result = _controller.GetAppointmentsFromStudentAsync(statusString).Result as OkObjectResult; ;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EquivalentTo(appointmentVMs));
            _eventBll.Verify(r => r.GetAllAppointmentsForStudentByStatusAsyncVm(It.IsAny<List<string>>()), Times.Once);
        }

        [Test]
        public void CreateAppointmentAsync_CreatesAppointment()
        {
            // Arrange
            var appointmentVM = new AppointmentVmBuilder().Build;
            _eventBll.Setup(b => b.CreateAppointmentAsyncVm(appointmentVM)).ReturnsAsync(appointmentVM);

            // Act
            var result = _controller.CreateAppointmentAsync(appointmentVM).Result as OkObjectResult; ;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.SameAs(appointmentVM));
            _eventBll.Verify(r => r.CreateAppointmentAsyncVm(It.IsAny<AppointmentVM>()), Times.Once);
        }

        [Test]
        public void CreateAppointmentAsync_ReturnsBadRequestWithInvalidParameter()
        {
            // Act
            var result = _controller.CreateAppointmentAsync(null).Result as BadRequestResult; ;

            // Assert
            Assert.That(result, Is.Not.Null);
            _eventBll.Verify(r => r.CreateAppointmentAsyncVm(It.IsAny<AppointmentVM>()), Times.Never);
        }

        [Test]
        public void CreateAppointmentAsync_ReturnsBadRequestWithModelStateInvalid()
        {
            // Arrange
            var appointmentVM = new AppointmentVmBuilder().WithInvalidBeginHour.Build;
            _controller.ModelState.AddModelError("BeginHour", "BeginHour is invalid");

            // Act
            var result = _controller.CreateAppointmentAsync(null).Result as BadRequestResult; ;

            // Assert
            Assert.That(result, Is.Not.Null);
            _eventBll.Verify(r => r.CreateAppointmentAsyncVm(It.IsAny<AppointmentVM>()), Times.Never);
        }

        [Test]
        public void UpdateAppointmentAsync_updatesValidAppointment()
        {
            // Arrange
            var appointmentVM = new AppointmentVmBuilder().Build;
            _eventBll.Setup(b => b.UpdateAppointmentAsyncVm(appointmentVM)).ReturnsAsync(appointmentVM);

            // Act
            var result = _controller.UpdateAppointmentAsync(appointmentVM).Result as OkObjectResult; ;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.SameAs(appointmentVM));
            _eventBll.Verify(r => r.UpdateAppointmentAsyncVm(It.IsAny<AppointmentVM>()), Times.Once);
        }

        [Test]
        public void UpdateAppointmentAsync_ReturnsBadRequestWithInvalidParameter()
        {
            // Act
            var result = _controller.UpdateAppointmentAsync(null).Result as BadRequestResult; ;

            // Assert
            Assert.That(result, Is.Not.Null);
            _eventBll.Verify(r => r.UpdateAppointmentAsyncVm(It.IsAny<AppointmentVM>()), Times.Never);
        }

        [Test]
        public void UpdateAppointmentAsync_ReturnsBadRequestWithModelStateInvalid()
        {
            // Arrange
            var appointmentVM = new AppointmentVmBuilder().WithInvalidBeginHour.Build;
            _controller.ModelState.AddModelError("BeginHour", "BeginHour is invalid");

            // Act
            var result = _controller.UpdateAppointmentAsync(null).Result as BadRequestResult; ;

            // Assert
            Assert.That(result, Is.Not.Null);
            _eventBll.Verify(r => r.UpdateAppointmentAsyncVm(It.IsAny<AppointmentVM>()), Times.Never);
        }

        [Test]
        public void UploadFilesAsync()
        {
            // Arrange
            var id = Guid.NewGuid();
            var form = new Mock<IFormFile>();
            var fileStorage = new FileStorage();
            _eventBll.Setup(r => r.UploadFileAsync(id, form.Object)).ReturnsAsync(fileStorage);

            // Act
            var result = _controller.UploadFilesAsync(id, form.Object).Result as OkObjectResult; ;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.SameAs(fileStorage));
            _eventBll.Verify(r => r.UploadFileAsync(It.IsAny<Guid>(), It.IsAny<IFormFile>()), Times.Once);
        }
    }
}
