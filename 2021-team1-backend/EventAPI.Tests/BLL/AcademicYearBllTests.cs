using System;
using System.Collections.Generic;
using AutoMapper;
using EventAPI.BLL;
using EventAPI.DAL.Repositories;
using EventAPI.Domain.Models;
using EventAPI.Domain.ViewModels;
using EventAPI.Tests.Builders;
using Moq;
using NUnit.Framework;

namespace EventAPI.Tests.BLL
{
    class AcademicYearBllTests
    {
        private Mock<IAcademicYearRepository> _academicYearRepository;
        private Mock<IMapper> _mapper;
        private AcademicYearBll _academicYearBll
            ;

        [SetUp]
        public void Setup()
        {
            _mapper = new Mock<IMapper>();
            _academicYearRepository = new Mock<IAcademicYearRepository>();
            _academicYearBll = new AcademicYearBll(_mapper.Object, _academicYearRepository.Object);
        }

        [Test]
        public void GetAsync_ReturnsAllAcademicYears()
        {
            // Arrange
            var academicYears = new List<AcademicYear>();
            _academicYearRepository.Setup(b => b.GetAsync()).ReturnsAsync(academicYears);

            // Act
            var result = _academicYearBll.GetAsync().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(academicYears));
            _academicYearRepository.Verify(r => r.GetAsync(), Times.Once);
        }

        [Test]
        public void GetAsyncVm_ReturnsAllAcademicYears()
        {
            // Arrange
            var academicYears = new List<AcademicYear>();
            var academicYearVMs = new List<AcademicYearVM>();
            _mapper.Setup(m => m.Map<IList<AcademicYearVM>>(academicYears)).Returns(academicYearVMs);

            // Act
            var result = _academicYearBll.GetAsyncVm().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(academicYearVMs));
            _mapper.Verify(m => m.Map<IList<AcademicYearVM>>(It.IsAny<IList<AcademicYear>>()), Times.Once);
        }

        [Test]
        public void GetByIdAsync_ReturnsAllAcademicYears()
        {
            // Arrange
            var academicYear = new AcademicYear();
            _academicYearRepository.Setup(b => b.GetByIdAsync(academicYear.Id)).ReturnsAsync(academicYear);

            // Act
            var result = _academicYearBll.GetByIdAsync(academicYear.Id).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(academicYear));
            _academicYearRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void GetAsyncVm_MapsToVm()
        {
            // Arrange
            var academicYear = new AcademicYear();
            var academicYearVm = new AcademicYearVmBuilder().FromAcademicYear(academicYear).Build;
            _academicYearRepository.Setup(b => b.GetByIdAsync(academicYear.Id)).ReturnsAsync(academicYear);
            _mapper.Setup(m => m.Map<AcademicYearVM>(academicYear)).Returns(academicYearVm);

            // Act
            var result = _academicYearBll.GetByIdAsyncVm(academicYear.Id).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(academicYearVm));
            _academicYearRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _mapper.Verify(m => m.Map<AcademicYearVM>(It.IsAny<AcademicYear>()), Times.Once);
        }

        [Test]
        public void CreateAsync_CreatesNewAcademicYear()
        {
            // Arrange
            var academicYear = new AcademicYear();
            _academicYearRepository.Setup(b => b.CreateAsync()).ReturnsAsync(academicYear);

            // Act
            var result = _academicYearBll.CreateAsync().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(academicYear));
            _academicYearRepository.Verify(r => r.CreateAsync(), Times.Once);
        }

        [Test]
        public void CreateAsyncVm_MapsToVm()
        {
            // Arrange
            var academicYear = new AcademicYear();
            var academicYearVm = new AcademicYearVmBuilder().FromAcademicYear(academicYear).Build;
            _academicYearRepository.Setup(b => b.CreateAsync()).ReturnsAsync(academicYear);
            _mapper.Setup(m => m.Map<AcademicYearVM>(academicYear)).Returns(academicYearVm);

            // Act
            var result = _academicYearBll.CreateAsyncVm().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(academicYearVm));
            _academicYearRepository.Verify(r => r.CreateAsync(), Times.Once);
            _mapper.Verify(m => m.Map<AcademicYearVM>(It.IsAny<AcademicYear>()), Times.Once);
        }
    }
}
