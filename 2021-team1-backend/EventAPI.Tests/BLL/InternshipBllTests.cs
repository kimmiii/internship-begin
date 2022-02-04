using AutoMapper;
using EventAPI.BLL;
using EventAPI.DAL.Repositories;
using EventAPI.Domain.Models;
using EventAPI.Domain.ViewModels;
using EventAPI.Tests.Builders;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace EventAPI.Tests.BLL
{
    class InternshipBllTests
    {
        private Mock<IMapper> _mapper;
        private Mock<IInternshipRepository> _internshipRepository;
        private Mock<ISpecializationRepository> _specialisationRepository;
        private Mock<ILocationRepository> _locationRepository;
        private Mock<IEnvironmentRepository> _environmentRepository;
        private Mock<IContactRepository> _contactRepository;
        private InternshipBll _internshipBll;

        [SetUp]
        public void Setup()
        {
            _mapper = new Mock<IMapper>();
            _internshipRepository = new Mock<IInternshipRepository>();
            _specialisationRepository = new Mock<ISpecializationRepository>();
            _locationRepository = new Mock<ILocationRepository>();
            _environmentRepository = new Mock<IEnvironmentRepository>();
            _contactRepository = new Mock<IContactRepository>();
            _internshipBll = new InternshipBll(_mapper.Object, _internshipRepository.Object, _specialisationRepository.Object, _locationRepository.Object, _environmentRepository.Object, _contactRepository.Object);
        }

        [Test]
        public void GetAsyncReturnsAllInternships()
        {
            // Arrange
            var internships = new InternshipListBuilder().Build;
            _internshipRepository.Setup(b => b.GetAsync()).ReturnsAsync(internships);

            // Act
            var result = _internshipBll.GetAsync().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(internships));
            _internshipRepository.Verify(r => r.GetAsync(), Times.Once);
        }

        [Test]
        public void GetAsyncVmReturnsAllInternshipsVm()
        {
            // Arrange
            var internships = new InternshipListBuilder().Build;
            var internshipVms = new List<InternshipVM>();
            _internshipRepository.Setup(b => b.GetAsync()).ReturnsAsync(internships);
            _mapper.Setup(m => m.Map<IList<InternshipVM>>(internships)).Returns(internshipVms);

            // Act
            var result = _internshipBll.GetAsyncVm().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(internshipVms));
            _internshipRepository.Verify(r => r.GetAsync(), Times.Once);
            _mapper.Verify(r => r.Map<IList<InternshipVM>>(It.IsAny<IList<Internship>>()), Times.Once);

        }

        [Test]
        public void GetByIdAsyncReturnsInternshipDetail()
        {
            // Arrange
            var internship = new InternshipBuilder().Build;
            _internshipRepository.Setup(b => b.GetByIdAsync(internship.InternshipId)).ReturnsAsync(internship);

            // Act
            var result = _internshipBll.GetByIdAsync(internship.InternshipId).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(internship));
            _internshipRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void GetByIdAsyncVmReturnsInternshipDetailVm()
        {
            // Arrange
            var internship = new InternshipBuilder().Build;
            var internshipVm = new InternshipVmBuilder().FromInternship(internship).Build;
            _internshipRepository.Setup(b => b.GetByIdAsync(internship.InternshipId)).ReturnsAsync(internship);
            _mapper.Setup(m => m.Map<InternshipVM>(internship)).Returns(internshipVm);

            // Act
            var result = _internshipBll.GetByIdAsyncVm(internship.InternshipId).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(internshipVm));
            _internshipRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _mapper.Verify(r => r.Map<InternshipVM>(It.IsAny<Internship>()), Times.Once);
        }

        [Test]
        public void GetByCompanyAsyncReturnsInternshipsFromCompany()
        {
            // Arrange
            var companyId = new Random().Next();
            var internships = new InternshipListBuilder().WithCompanyId(companyId).Build;
            _internshipRepository.Setup(b => b.GetByCompanyAsync(companyId)).ReturnsAsync(internships);

            // Act
            var result = _internshipBll.GetByCompanyAsync(companyId).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(internships));
            _internshipRepository.Verify(r => r.GetByCompanyAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void GetByCompanyAsyncVmReturnsInternshipsFromCompanyVm()
        {
            // Arrange
            var companyId = new Random().Next();
            var internships = new InternshipListBuilder().WithCompanyId(companyId).Build;
            var internshipVms = new List<InternshipVM>();
            internships.ForEach(i => internshipVms.Add(new InternshipVmBuilder().FromInternship(i).Build));
            
            _internshipRepository.Setup(b => b.GetByCompanyAsync(companyId)).ReturnsAsync(internships);
            _mapper.Setup(m => m.Map<List<InternshipVM>>(internships)).Returns(internshipVms);

            // Act
            var result = _internshipBll.GetByCompanyAsyncVm(companyId).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(internshipVms));
            _internshipRepository.Verify(r => r.GetByCompanyAsync(It.IsAny<int>()), Times.Once);
            _mapper.Verify(r => r.Map<List<InternshipVM>>(It.IsAny<List<Internship>>()), Times.Once);
        }

        [Test]
        public void GetByAcademicYearAsyncReturnsInternshipsFromAcademicYear()
        {
            // Arrange
            var academicYear = new AcademicYear();
            var internships = new InternshipListBuilder().WithAcademicYear(academicYear.Description).Build;
            _internshipRepository.Setup(b => b.GetByAcademicYearAsync(academicYear.Description)).ReturnsAsync(internships);

            // Act
            var result = _internshipBll.GetByAcademicYearAsync(academicYear.Description).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(internships));
            _internshipRepository.Verify(r => r.GetByAcademicYearAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void GetByAcademicYearAsyncVmReturnsInternshipsFromAcademicYearVm()
        {
            // Arrange
            var academicYear = new AcademicYear();
            var internships = new InternshipListBuilder().WithAcademicYear(academicYear.Description).Build;
            var internshipVms = new List<InternshipVM>();
            internships.ForEach(i => internshipVms.Add(new InternshipVmBuilder().FromInternship(i).Build));
            _internshipRepository.Setup(b => b.GetByAcademicYearAsync(academicYear.Description)).ReturnsAsync(internships);
            _mapper.Setup(m => m.Map<List<InternshipVM>>(internships)).Returns(internshipVms);

            // Act
            var result = _internshipBll.GetByAcademicYearAsyncVm(academicYear.Description).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(internshipVms));
            _internshipRepository.Verify(r => r.GetByAcademicYearAsync(It.IsAny<string>()), Times.Once);
            _mapper.Verify(r => r.Map<List<InternshipVM>>(It.IsAny<List<Internship>>()), Times.Once);

        }

        [Test]
        public void GetInternshipsFromEventByCompanyAsyncReturnsInternshipsFromEventAndCompany()
        {
            // Arrange
            var academicYear = new AcademicYear();
            var companyId = new Random().Next();

            var internships = new InternshipListBuilder().WithAcademicYear(academicYear.Description).WithCompanyId(companyId).Build;
            _internshipRepository.Setup(b => b.GetInternshipsFromEventByCompanyAsync(academicYear.Description, companyId)).ReturnsAsync(internships);

            // Act
            var result = _internshipBll.GetInternshipsFromEventByCompanyAsync(academicYear.Description, companyId).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(internships));
            _internshipRepository.Verify(r => r.GetInternshipsFromEventByCompanyAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void GetInternshipsFromEventByCompanyVmAsyncReturnsInternshipsFromEventAndCompanyVm()
        {
            // Arrange
            var academicYear = new AcademicYear();
            var companyId = new Random().Next();

            var internships = new InternshipListBuilder().WithAcademicYear(academicYear.Description).WithCompanyId(companyId).Build;
            var internshipVms = new List<InternshipVM>();
            internships.ForEach(i => internshipVms.Add(new InternshipVmBuilder().FromInternship(i).Build)); _internshipRepository.Setup(b => b.GetInternshipsFromEventByCompanyAsync(academicYear.Description, companyId)).ReturnsAsync(internships);
            _internshipRepository.Setup(b => b.GetInternshipsFromEventByCompanyAsync(academicYear.Description, companyId)).ReturnsAsync(internships);
            _mapper.Setup(m => m.Map<List<InternshipVM>>(internships)).Returns(internshipVms);

            // Act
            var result = _internshipBll.GetInternshipsFromEventByCompanyAsyncVm(academicYear.Description, companyId).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(internshipVms));
            _internshipRepository.Verify(r => r.GetInternshipsFromEventByCompanyAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
            _mapper.Verify(r => r.Map<List<InternshipVM>>(It.IsAny<List<Internship>>()), Times.Once);
        }

        [Test]
        public void UpdateInternshipAsyncUpdatesInternship()
        {
            // Arrange
            var internship = new InternshipBuilder().Build;
            var internshipToUpdate = internship;
            _internshipRepository.Setup(b => b.GetByIdAsync(internship.InternshipId)).ReturnsAsync(internshipToUpdate);
            _mapper.Setup(m => m.Map(internship, internshipToUpdate)).Returns(internshipToUpdate);
            _internshipRepository.Setup(b => b.UpdateAsync(internshipToUpdate));

            // Act
            var result = _internshipBll.UpdateInternshipAsync(internship).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(internshipToUpdate));
            _internshipRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Exactly(2));
            _mapper.Verify(r => r.Map(It.IsAny<Internship>(), It.IsAny<Internship>()), Times.Once);
            _internshipRepository.Verify(r => r.UpdateAsync(It.IsAny<Internship>()), Times.Once);
        }

        [Test]
        public void UpdateInternshipVmAsyncUpdatesInternshipVm()
        {
            // Arrange
            var internship = new InternshipBuilder().Build;
            var internshipVm = new InternshipVmBuilder().FromInternship(internship).Build; 
            _internshipRepository.Setup(b => b.GetByIdAsync(internship.InternshipId)).ReturnsAsync(internship);
            _mapper.Setup(m => m.Map<Internship>(internshipVm)).Returns(internship);
            _mapper.Setup(m => m.Map<InternshipVM>(internship)).Returns(internshipVm);

            // Act
            var result = _internshipBll.UpdateInternshipAsyncVm(internshipVm).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(internshipVm));
            _internshipRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Exactly(2));
            _mapper.Verify(r => r.Map<InternshipVM>(It.IsAny<Internship>()), Times.Once);
            _mapper.Verify(r => r.Map<Internship>(It.IsAny<InternshipVM>()), Times.Once);
        }

        [Test]
        public void UpdateInternshipsMultiAsyncUpdatesInternships()
        {
            // Arrange
            var internships = new InternshipListBuilder().Build;
            var internshipsUpdated = new InternshipListBuilder().Build;
            internships.ForEach(i =>
            {
                var internshipToUpdate = i;
                _internshipRepository.Setup(b => b.GetByIdAsync(i.InternshipId)).ReturnsAsync(internshipToUpdate);
                _mapper.Setup(m => m.Map(i, internshipToUpdate)).Returns(internshipToUpdate);
                _internshipRepository.Setup(b => b.UpdateAsync(internshipToUpdate));
                internshipsUpdated.Add(internshipToUpdate);
            });
            var count = internshipsUpdated.Count;
          
            // Act
            var result = _internshipBll.UpdateInternshipsMultiAsync(internships).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(internshipsUpdated));
            _internshipRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Exactly(count*2));
            _mapper.Verify(r => r.Map(It.IsAny<Internship>(), It.IsAny<Internship>()), Times.Exactly(count));
            _internshipRepository.Verify(r => r.UpdateAsync(It.IsAny<Internship>()), Times.Exactly(count));
        }

        [Test]
        public void UpdateInternshipsMultiAsyncVmUpdatesInternshipsVm()
        {
            // Arrange
            var internships = new InternshipListBuilder().Build;
            var internshipVms = new List<InternshipVM>();
            internships.ForEach(i => internshipVms.Add(new InternshipVmBuilder().FromInternship(i).Build));
            internships.ForEach(i => _internshipRepository.Setup(b => b.GetByIdAsync(i.InternshipId)).ReturnsAsync(i));
            _mapper.Setup(m => m.Map<List<InternshipVM>>(internships)).Returns(internshipVms);
            _mapper.Setup(m => m.Map< List<Internship>>(internshipVms)).Returns(internships);
            var count = internshipVms.Count;

            // Act
            var result = _internshipBll.UpdateInternshipsMultiAsyncVm(internshipVms).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(internshipVms));
            _internshipRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Exactly(count * 2));
            _mapper.Verify(r => r.Map<InternshipVM>(It.IsAny<Internship>()), Times.Exactly(count));
            _mapper.Verify(r => r.Map<Internship>(It.IsAny<InternshipVM>()), Times.Exactly(count));
        }

        [Test]
        public void GetSpecializationsForFilterAsyncReturnsSpecializations()
        {
            // Arrange
            var specializations = new List<Specialisation>();
            _specialisationRepository.Setup(b => b.GetAsync()).ReturnsAsync(specializations);

            // Act
            var result = _internshipBll.GetSpecializationsForFilterAsync().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(specializations));
            _specialisationRepository.Verify(r => r.GetAsync(), Times.Once);
        }

        [Test]
        public void GetSpecializationsForFilterAsyncVm()
        {
            // Arrange
            var specializations = new List<Specialisation>();
            var specializationVms = new List<SpecialisationFilterVM>();
            _specialisationRepository.Setup(b => b.GetAsync()).ReturnsAsync(specializations);
            _mapper.Setup(m => m.Map<List<SpecialisationFilterVM>>(specializations)).Returns(specializationVms);

            // Act
            var result = _internshipBll.GetSpecializationsForFilterAsyncVm().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(specializationVms));
            _specialisationRepository.Verify(r => r.GetAsync(), Times.Once);
            _mapper.Verify(r => r.Map<List<SpecialisationFilterVM>>(It.IsAny<List<Specialisation>>()), Times.Once);
        }

        [Test]
        public void GetLocationsForFilterAsync()
        {
            // Arrange
            var locations = new List<Location>();
            _locationRepository.Setup(b => b.GetAsync()).ReturnsAsync(locations);

            // Act
            var result = _internshipBll.GetLocationsForFilterAsync().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(locations));
            _locationRepository.Verify(r => r.GetAsync(), Times.Once);
        }

        [Test]
        public void GetLocationsForFilterAsyncVm()
        {
            // Arrange
            var locations = new List<Location>();
            var locationVms = new List<LocationFilterVM>();
            _locationRepository.Setup(b => b.GetAsync()).ReturnsAsync(locations);
            _mapper.Setup(m => m.Map<List<LocationFilterVM>>(locations)).Returns(locationVms);

            // Act
            var result = _internshipBll.GetLocationsForFilterAsyncVm().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(locationVms));
            _locationRepository.Verify(r => r.GetAsync(), Times.Once);
            _mapper.Verify(r => r.Map<List<LocationFilterVM>>(It.IsAny<List<Location>>()), Times.Once);
        }

    }
}
