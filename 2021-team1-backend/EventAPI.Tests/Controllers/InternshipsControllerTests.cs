using EventAPI.BLL;
using EventAPI.Controllers;
using EventAPI.Domain.ViewModels;
using EventAPI.Tests.Builders;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace EventAPI.Tests.Controllers
{
    class InternshipsControllerTests
    {
        private Mock<IInternshipBll> _internshipBll;
        private InternshipsController _controller;

        [SetUp]
        public void Setup()
        {
            _internshipBll = new Mock<IInternshipBll>();
            _controller = new InternshipsController(_internshipBll.Object);
        }

        [Test]
        public void GetAllInternshipsAsyncReturnsAllInternships()
        {
            // Arrange
            var internshipVms = new List<InternshipVM>();
            _internshipBll.Setup(b => b.GetAsyncVm()).ReturnsAsync(internshipVms);

            // Act
            var result = _controller.GetAllInternshipsAsync().Result as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EquivalentTo(internshipVms));
            _internshipBll.Verify(r => r.GetAsyncVm(), Times.Once);
        }

        [Test]
        public void GetInternshipsByIdAsyncReturnsInternshipDetail()
        {
            // Arrange
            var internshipVm = new InternshipVmBuilder().Build;
            _internshipBll.Setup(b => b.GetByIdAsyncVm(internshipVm.InternshipId)).ReturnsAsync(internshipVm);

            // Act
            var result = _controller.GetInternshipsByIdAsync(internshipVm.InternshipId).Result as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.SameAs(internshipVm));
            _internshipBll.Verify(r => r.GetByIdAsyncVm(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void GetInternshipsByCompanyAsyncReturnsInternshipsFromCompany()
        {
            // Arrange
            int companyId = new Random().Next();
            var internshipVms = new List<InternshipVM>();
            for (var i = 0; i < 3; i++)
            {
                internshipVms.Add(new InternshipVmBuilder().WithCompanyId(companyId).Build);
            }
            _internshipBll.Setup(b => b.GetByCompanyAsyncVm(companyId)).ReturnsAsync(internshipVms);

            // Act
            var result = _controller.GetInternshipsByCompanyAsync(companyId).Result as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.SameAs(internshipVms));
            _internshipBll.Verify(r => r.GetByCompanyAsyncVm(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void UpdateInternshipAsyncUpdatesInternshipInDatabase()
        {
            // Arrange
            var internshipVm = new InternshipVmBuilder().Build;
            _internshipBll.Setup(b => b.UpdateInternshipAsyncVm(internshipVm)).ReturnsAsync(internshipVm);

            // Act
            var result = _controller.UpdateInternshipAsync(internshipVm).Result as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.SameAs(internshipVm));
            _internshipBll.Verify(r => r.UpdateInternshipAsyncVm(It.IsAny<InternshipVM>()), Times.Once);
        }

        [Test]
        public void UpdateInternshipAsyncReturnsBadRequestIfParameterIsInvalid()
        {
            // Act
            var result = _controller.UpdateInternshipAsync(null).Result as BadRequestResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            _internshipBll.Verify(r => r.UpdateInternshipAsyncVm(It.IsAny<InternshipVM>()), Times.Never);
        }

        [Test]
        public void UpdateInternshipsMultiAsyncUpdatesInternshipsInDatabase()
        {
            // Arrange
            var internshipVms = new List<InternshipVM>();
            _internshipBll.Setup(b => b.UpdateInternshipsMultiAsyncVm(internshipVms)).ReturnsAsync(internshipVms);

            // Act
            var result = _controller.UpdateInternshipsMultiAsync(internshipVms).Result as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EquivalentTo(internshipVms));
            _internshipBll.Verify(r => r.UpdateInternshipsMultiAsyncVm(It.IsAny<List<InternshipVM>>()), Times.Once);
        }

    }
}
