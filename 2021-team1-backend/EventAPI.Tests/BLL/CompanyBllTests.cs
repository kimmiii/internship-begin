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
using CompanyVM = EventAPI.Domain.ViewModels.CompanyVM;

namespace EventAPI.Tests.BLL
{
    class CompanyBllTests
    {
        private Mock<ICompanyRepository> _companyRepository;
        private Mock<IMapper> _mapper;
        private CompanyBll _companyBll
            ;

        [SetUp]
        public void Setup()
        {
            _mapper = new Mock<IMapper>();
            _companyRepository = new Mock<ICompanyRepository>();
            _companyBll = new CompanyBll(_mapper.Object, _companyRepository.Object);
        }

        [Test]
        public void GetAsync_ReturnsAllCompanies()
        {
            // Arrange
            var companies = new List<Company>();
            _companyRepository.Setup(b => b.GetAsync()).ReturnsAsync(companies);

            // Act
            var result = _companyBll.GetAsync().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(companies));
            _companyRepository.Verify(r => r.GetAsync(), Times.Once);
        }

        [Test]
        public void GetAsyncVm_MapsToVM()
        {
            // Arrange
            var companies = new List<Company>();
            var companiesVm = new List<CompanyVM>();
            companies.ForEach(c => companiesVm.Add(new CompanyVmBuilder().FromCompany(c).Build));
            
            _companyRepository.Setup(b => b.GetAsync()).ReturnsAsync(companies);
            _mapper.Setup(m => m.Map<IList<CompanyVM>>(companies)).Returns(companiesVm);

            // Act
            var result = _companyBll.GetAsyncVM().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(companiesVm));
            _companyRepository.Verify(r => r.GetAsync(), Times.Once);
            _mapper.Verify(m => m.Map<IList<CompanyVM>>(It.IsAny<IList<Company>>()), Times.Once);
        }

        [Test]
        public void GetByIdAsync_ReturnsCompanyDetail()
        {
            // Arrange
            var company = new Company();
            _companyRepository.Setup(b => b.GetByIdAsync()).ReturnsAsync(company);

            // Act
            var result = _companyBll.GetByIdAsync().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(company));
            _companyRepository.Verify(r => r.GetByIdAsync(), Times.Once);
        }

        [Test]
        public void GetByIdAsync_WithId_ReturnsCompanyDetail()
        {
            // Arrange
            var company = new Company();
            _companyRepository.Setup(b => b.GetByIdAsync(company.CompanyId)).ReturnsAsync(company);

            // Act
            var result = _companyBll.GetByIdAsync(company.CompanyId).Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(company));
            _companyRepository.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Once);
        }


        [Test]
        public void GetByIdAsyncVM_MapsToVm()
        {
            // Arrange
            var company = new Company();
            var companyVm = new CompanyVmBuilder().FromCompany(company).Build;
            _companyRepository.Setup(b => b.GetByIdAsync()).ReturnsAsync(company);
            _mapper.Setup(m => m.Map<CompanyVM>(company)).Returns(companyVm);

            // Act
            var result = _companyBll.GetByIdAsyncVM().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.SameAs(companyVm));
            _companyRepository.Verify(r => r.GetByIdAsync(), Times.Once);
            _mapper.Verify(m => m.Map<CompanyVM>(It.IsAny<Company>()), Times.Once);
        }

    
        [Test]
        public void GetCompaniesForFilterAsyncVM()
        {
            // Arrange
            var companies = new List<Company>();
            var companiesVm = new List<CompanyFilterVM>();

            _companyRepository.Setup(b => b.GetAsync()).ReturnsAsync(companies);
            _mapper.Setup(m => m.Map<IList<CompanyFilterVM>>(companies)).Returns(companiesVm);

            // Act
            var result = _companyBll.GetCompaniesForFilterAsyncVM().Result;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EquivalentTo(companiesVm));
            _companyRepository.Verify(r => r.GetAsync(), Times.Once);
            _mapper.Verify(m => m.Map<IList<CompanyFilterVM>>(It.IsAny<IList<Company>>()), Times.Once);
        }

    }
}
