using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using StagebeheerAPI.Contracts;
using StagebeheerAPI.Domain;
using StagebeheerAPI.FilterPattern;
using StagebeheerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace StagebeheerAPI.Tests.Domain
{
    class InternshipPaginatorTests
    {
        private Mock<IRepositoryWrapper> _RepositoryWrapperMock;
        private InternshipPaginator _InternshipPaginator;
        private List<Internship> _InternshipTestData;

        [SetUp]
        public void Setup()
        {
            _InternshipTestData = _CreateTestData();
            var internshipMock = _InternshipTestData.AsQueryable().BuildMock();

            _RepositoryWrapperMock = new Mock<IRepositoryWrapper>();
            _RepositoryWrapperMock.Setup(x => x.Internship.FindByCondition(It.IsAny<Expression<Func<Internship, bool>>>()))
                                               .Returns(internshipMock.Object);
            var _FilterMock = new Mock<CombinedFilter>();

            _InternshipPaginator = new InternshipPaginator(_RepositoryWrapperMock.Object);
        }

        [Test]
        public void GetInternshipsAndPages_ReturnsValidResult_WithRegularTestData()
        {
            // Arrange
            var testCriteria = new PageCriteria
            {
                InternshipsPerPage = 3,
                PageNumber = 2
            };
            
            // Act
            var result = _InternshipPaginator.GetInternshipsAndPages(_InternshipTestData, testCriteria);

            // Assert
            result.Internships.Count.Should().Be(testCriteria.InternshipsPerPage);
            result.Internships[0].InternshipId.Should().Be(4);
            result.Internships[1].InternshipId.Should().Be(5);
            result.Internships[2].InternshipId.Should().Be(6);
            result.Pages.Should().Be(4);
        }

        [Test]
        public void GetInternshipsAndPages_ReturnsZeroInternships_WhenPageNumberDoesntExist()
        {
            // Arrange
            var testCriteria = new PageCriteria
            {
                InternshipsPerPage = 10,
                PageNumber = 99
            };

            // Act
            var result = _InternshipPaginator.GetInternshipsAndPages(_InternshipTestData, testCriteria);

            // Assert
            result.Internships.Count.Should().Be(0);
            result.Pages.Should().Be(2);
        }

        [Test]
        public void GetInternshipsAndPages_ReturnsValidResult_WhenInternshipsPerPageAreHigherThanExistingInternships()
        {
            // Arrange
            var testCriteria = new PageCriteria
            {
                InternshipsPerPage = 99,
                PageNumber = 1
            };

            // Act
            var result = _InternshipPaginator.GetInternshipsAndPages(_InternshipTestData, testCriteria);

            // Assert
            result.Internships.Count.Should().Be(_InternshipTestData.Count);
            result.Pages.Should().Be(1);
        }

        [Test]
        public void GetInternshipsAndPages_ReturnsZeroInternshipsAndZeroPages_WhenNoInternshipsExist()
        {
            // Arrange
            var testCriteria = new PageCriteria
            {
                InternshipsPerPage = 5,
                PageNumber = 2
            };

            var testData = new List<Internship>();
            var internshipMock = testData.AsQueryable().BuildMock();

            var repoMock = new Mock<IRepositoryWrapper>();
            var filterMock = new Mock<CombinedFilter>();
            repoMock.Setup(x => x.Internship.FindByCondition(It.IsAny<Expression<Func<Internship, bool>>>()))
                                               .Returns(internshipMock.Object);

            _InternshipPaginator = new InternshipPaginator(repoMock.Object);

            // Act
            var result = _InternshipPaginator.GetInternshipsAndPages(testData, testCriteria);

            // Assert
            result.Internships.Count.Should().Be(0);
            result.Pages.Should().Be(0);
        }

        private List<Internship> _CreateTestData()
        {
            return new List<Internship>
            {
                new Internship {InternshipId = 1},
                new Internship {InternshipId = 2},
                new Internship {InternshipId = 3},
                new Internship {InternshipId = 4},
                new Internship {InternshipId = 5},
                new Internship {InternshipId = 6},
                new Internship {InternshipId = 7},
                new Internship {InternshipId = 8},
                new Internship {InternshipId = 9},
                new Internship {InternshipId = 10},
                new Internship {InternshipId = 11},
            };
        }
    }
}
