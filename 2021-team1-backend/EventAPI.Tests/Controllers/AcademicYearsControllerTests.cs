using System;
using EventAPI.BLL;
using EventAPI.Controllers;
using EventAPI.Domain.ViewModels;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace EventAPI.Tests.Controllers
{
    class AcademicYearsControllerTests
    {
        private AcademicYearsController _controller;
        private Mock<IAcademicYearBll> _academicYearBll;

        [SetUp]
        public void Setup()
        {
            _academicYearBll = new Mock<IAcademicYearBll>();
            _controller = new AcademicYearsController(_academicYearBll.Object);
        }

        [Test]
        public void GetAcademicYears_returnsAllAcademicYearsVm()
        {
            // Arrange
            var academicYearVMs = new List<AcademicYearVM>();
            _academicYearBll.Setup(b => b.GetAsyncVm()).ReturnsAsync(academicYearVMs);

            // Act
            var result = _controller.GetAcademicYears().Result as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EquivalentTo(academicYearVMs));
            _academicYearBll.Verify(r => r.GetAsyncVm(), Times.Once);
        }

        [Test]
        public void GetAcademicYearById_returnsAcademicYearDetails()
        {
            // Arrange
            var academicYearVM = new AcademicYearVM();
            _academicYearBll.Setup(b => b.GetByIdAsyncVm(academicYearVM.Id)).ReturnsAsync(academicYearVM);

            // Act
            var result = _controller.GetAcademicYearById(academicYearVM.Id).Result as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.SameAs(academicYearVM));
            _academicYearBll.Verify(r => r.GetByIdAsyncVm(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void CreateAcademicYear_CreatesNextAcademicYear()
        {
            // Arrange
            var academicYearVM = new AcademicYearVM();
            _academicYearBll.Setup(b => b.CreateAsyncVm()).ReturnsAsync(academicYearVM);

            // Act
            var result = _controller.CreateAcademicYear().Result as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.SameAs(academicYearVM));
            _academicYearBll.Verify(r => r.CreateAsyncVm(), Times.Once);
        }
    }
}