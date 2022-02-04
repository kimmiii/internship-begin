using EventAPI.BLL;
using EventAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using EventAPI.Domain.ViewModels;
using CompanyFilterVM = EventAPI.Domain.ViewModels.CompanyFilterVM;

namespace EventAPI.Tests.Controllers
{
    class FiltersControllerTests
    {
        private FiltersController _controller;
        private Mock<ICompanyBll> _companyBll;
        private Mock<IEventBll> _eventBll;
        private Mock<IInternshipBll> _internshipBll;

        [SetUp]
        public void Setup()
        {
            _companyBll = new Mock<ICompanyBll>();
            _internshipBll = new Mock<IInternshipBll>();
            _eventBll = new Mock<IEventBll>();
            _controller = new FiltersController(_companyBll.Object, _eventBll.Object, _internshipBll.Object);
        }

        [Test]
        public void GetCompaniesAsync_ReturnsAllCompaniesForFiltering()
        {
            // Arrange
            var companyVms = new List<CompanyFilterVM>();
            _eventBll.Setup(b => b.GetCompaniesFromActiveEventForFilterAsyncVm()).ReturnsAsync(companyVms);

            // Act
            var result = _controller.GetCompaniesAsync().Result as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EquivalentTo(companyVms));
            _eventBll.Verify(r => r.GetCompaniesFromActiveEventForFilterAsyncVm(), Times.Once);
        }

        [Test]
        public void GetSpecialisationsAsync_ReturnsAllSpezializationForFiltering()
        {
            // Arrange
            var specialisationFilterVms = new List<SpecialisationFilterVM>();
            _internshipBll.Setup(b => b.GetSpecializationsForFilterAsyncVm()).ReturnsAsync(specialisationFilterVms);

            // Act
            var result = _controller.GetSpecialisationsAsync().Result as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EquivalentTo(specialisationFilterVms));
            _internshipBll.Verify(r => r.GetSpecializationsForFilterAsyncVm(), Times.Once);
        }

        [Test]
        public void GetLocationsAsync_ReturnsAllLocationsForFiltering()
        {
            // Arrange
            var locationFilterVms = new List<LocationFilterVM>();
            _internshipBll.Setup(b => b.GetLocationsForFilterAsyncVm()).ReturnsAsync(locationFilterVms);

            // Act
            var result = _controller.GetLocationsAsync().Result as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EquivalentTo(locationFilterVms));
            _internshipBll.Verify(r => r.GetLocationsForFilterAsyncVm(), Times.Once);
        }
    }
}
