using FluentAssertions;
using Moq;
using NUnit.Framework;
using StagebeheerAPI.Controllers;
using StagebeheerAPI.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using StagebeheerAPI.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MockQueryable.Moq;
using EmailService;
using Wkhtmltopdf.NetCore;
using StagebeheerAPI.Models.ApiModels;

namespace StagebeheerAPI.Test.Controllers.Unit
{
    public class InternshipsControllerTests
    {
        private InternshipsController _InternshipsController;

        private Mock<IRepositoryWrapper> _RepositoryWrapperMock;
        private Mock<IEmailSender> _EmailSenderMock;
        private Mock<IGeneratePdf> _GeneratePdfMock;
        private Mock<IInternshipPaginator> _InternshipPaginatorMock;
        private Mock<ICombinedFilter> _CombinedFilterMock;


        [SetUp]
        public void Setup()
        {
            _RepositoryWrapperMock = new Mock<IRepositoryWrapper>();
            _EmailSenderMock = new Mock<IEmailSender>();
            _GeneratePdfMock = new Mock<IGeneratePdf>();
            _InternshipPaginatorMock = new Mock<IInternshipPaginator>();
            _CombinedFilterMock = new Mock<ICombinedFilter>();
        }

        [Test]
        public async Task GetInternship_ReturnsInternships_WhenInternshipsExist()
        {
            // Arrange
            var testInternship = SetupInternship();

            var testInternships = new List<Internship>();
            testInternships.Add(testInternship);

            var internshipMock = testInternships.AsQueryable().BuildMock();

            _RepositoryWrapperMock.Setup(x => x.Internship.FindAll()).Returns(internshipMock.Object);

            _InternshipsController = new InternshipsController(_RepositoryWrapperMock.Object, 
                _EmailSenderMock.Object, 
                _GeneratePdfMock.Object, 
                _InternshipPaginatorMock.Object,
                _CombinedFilterMock.Object);

            // Act
            var apiResult = await _InternshipsController.GetInternship();
            var internshipResult = apiResult.Value as List<Internship>;

            // Assert
            internshipResult.Should().BeEquivalentTo(testInternships);
        }

        [Test]
        public async Task GetApprovedInternship_ReturnsInternships_WhenInternshipsExist()
        {
            // Arrange
            var testCriteria = new GetApprovedInternshipCriteria { 
                PageCriteria = new PageCriteria
                {
                    InternshipsPerPage = 10, 
                    PageNumber = 2
                }, 
                FilterCriteria = new Internship()
            };

            var testInternships = new List<Internship> 
            {
                new Internship()
            };
            var internshipMock = testInternships.AsQueryable().BuildMock();

            _RepositoryWrapperMock.Setup(x => x.Internship.FindAll()).Returns(internshipMock.Object);

            _InternshipsController = new InternshipsController(_RepositoryWrapperMock.Object, 
                _EmailSenderMock.Object, 
                _GeneratePdfMock.Object,
                _InternshipPaginatorMock.Object,
                _CombinedFilterMock.Object);

            var testInternshipsList = new List<Internship>();

            _CombinedFilterMock.Setup(x => x.comboFiltering(It.IsAny<List<Internship>>(), It.IsAny<Internship>())).Returns(testInternshipsList);

            // Act
            await _InternshipsController.GetApprovedInternship(testCriteria);

            // Assert
            _InternshipPaginatorMock.Verify(x => x.GetInternshipsAndPages(testInternshipsList, testCriteria.PageCriteria), Moq.Times.Once());
        }

        [Test]
        public async Task GetInternshipById_ReturnsInternship_WhenInternshipExists()
        {
            // Arrange
            var testInternshipId = 1;
            var testInternship = SetupInternship();

            var testInternships = new List<Internship>();
            testInternships.Add(testInternship);

            var internshipMock = testInternships.AsQueryable().BuildMock();

            _RepositoryWrapperMock.Setup(x => x.Internship.FindByCondition(It.IsAny<Expression<Func<Internship, bool>>>())).Returns(internshipMock.Object);

            _InternshipsController = new InternshipsController(_RepositoryWrapperMock.Object, 
                _EmailSenderMock.Object, 
                _GeneratePdfMock.Object, 
                _InternshipPaginatorMock.Object, 
                _CombinedFilterMock.Object);

            // Act
            var apiResult = await _InternshipsController.GetInternship(testInternshipId);
            var internshipResult = apiResult.Value as Internship;

            // Assert
            internshipResult.Should().BeEquivalentTo(testInternship);
        }

        [Test]
        public async Task GetInternshipById_ReturnsNotFound_WhenInternshipDoesNotExist()
        {
            // Arrange
            var testInternshipId = 2;
            var testInternship = SetupInternship();

            var testInternships = new List<Internship>();
            testInternships.Add(testInternship);

            var internshipMock = testInternships.AsQueryable().BuildMock();

            _RepositoryWrapperMock.Setup(x => x.Internship.FindByCondition(It.IsAny<Expression<Func<Internship, bool>>>())).Returns(internshipMock.Object);

            _InternshipsController = new InternshipsController(_RepositoryWrapperMock.Object, 
                _EmailSenderMock.Object, 
                _GeneratePdfMock.Object, 
                _InternshipPaginatorMock.Object,
                _CombinedFilterMock.Object);

            // Act
            var apiResult = await _InternshipsController.GetInternship(testInternshipId);
            var internshipResult = apiResult.Result as NotFoundResult;

            // Assert
            internshipResult.Should().BeEquivalentTo(new NotFoundResult());
        }

        [Test]
        public async Task GetInternshipsByCompanyId_ReturnsInternships_WhenInternshipsExist()
        {
            // Arrange
            var testCompanyId = 1;
            var testInternship = SetupInternship();
            var testInternship2 = SetupInternship();
            testInternship2.CompanyId = 2;
            testInternship2.InternshipId = 2;

            var testInternships = new List<Internship>();
            testInternships.Add(testInternship);
            testInternships.Add(testInternship2);

            var testVerificationList = new List<Internship>();
            testVerificationList.Add(testInternship);

            var internshipMock = testInternships.AsQueryable().BuildMock();

            _RepositoryWrapperMock.Setup(x => x.Internship.FindByCondition(It.IsAny<Expression<Func<Internship, bool>>>())).Returns(internshipMock.Object.Where(x => x.CompanyId == testCompanyId));

            _InternshipsController = new InternshipsController(_RepositoryWrapperMock.Object, 
                _EmailSenderMock.Object, 
                _GeneratePdfMock.Object, 
                _InternshipPaginatorMock.Object,
                _CombinedFilterMock.Object);

            // Act
            var apiResult = await _InternshipsController.GetInternshipByCompany(testCompanyId);
            var contactResult = apiResult.Value as List<Internship>;

            // Assert
            contactResult.Should().BeEquivalentTo(testVerificationList);
        }

        [Test]
        public async Task GetInternshipsByReviewer_ReturnsInternships_WhenInternshipsExist()
        {
            // Arrange
            var testReviewerId = 4;
            var testInternship = SetupInternship();
            testInternship.InternshipReviewer = new List<InternshipReviewer> { new InternshipReviewer{UserId = 4 } };
            var testInternship2 = SetupInternship();
            testInternship2.InternshipReviewer = new List<InternshipReviewer> { new InternshipReviewer { UserId = 3 } };
            testInternship2.CompanyId = 2;
            testInternship2.InternshipId = 2;

            var testInternships = new List<Internship>();
            testInternships.Add(testInternship);
            testInternships.Add(testInternship2);

            var testVerificationList = new List<Internship>();
            testVerificationList.Add(testInternship);

            var internshipMock = testInternships.AsQueryable().BuildMock();

            _RepositoryWrapperMock.Setup(x => x.Internship.FindByCondition(It.IsAny<Expression<Func<Internship, bool>>>())).Returns(internshipMock.Object.Where(x => x.InternshipReviewer.Any(y => y.UserId == testReviewerId)));

            _InternshipsController = new InternshipsController(_RepositoryWrapperMock.Object, 
                _EmailSenderMock.Object,
                _GeneratePdfMock.Object, 
                _InternshipPaginatorMock.Object,
                _CombinedFilterMock.Object);

            // Act
            var apiResult = await _InternshipsController.GetInternshipByCompany(testReviewerId);
            var contactResult = apiResult.Value as List<Internship>;

            // Assert
            contactResult.Should().BeEquivalentTo(testVerificationList);
        }

        [Test]
        public async Task SetFavourite_ReturnsInternshipID()
        {
            // Arrange
            var testUserFavourite = new UserFavourites { InternshipId = 10, UserId = 15 };
            _InternshipsController = new InternshipsController(_RepositoryWrapperMock.Object,
                _EmailSenderMock.Object,
                _GeneratePdfMock.Object,
                _InternshipPaginatorMock.Object,
                _CombinedFilterMock.Object);

            // Act
            var apiResult = await _InternshipsController.PostFavourite(testUserFavourite);
            var favouriteResult = apiResult.Result;

            // Assert
            favouriteResult.Should().BeEquivalentTo(new OkObjectResult(testUserFavourite.InternshipId));
        }

        [Test]
        public async Task DeleteFavourite_ReturnsInternshipID()
        {
            // Arrange
            var testUserFavourite = new UserFavourites { InternshipId = 10, UserId = 15 };
            _InternshipsController = new InternshipsController(_RepositoryWrapperMock.Object,
                _EmailSenderMock.Object,
                _GeneratePdfMock.Object,
                _InternshipPaginatorMock.Object,
                _CombinedFilterMock.Object);

            // Act
            var apiResult = await _InternshipsController.DeleteFavourite(testUserFavourite);
            var favouriteResult = apiResult.Result;

            // Assert
            favouriteResult.Should().BeEquivalentTo(new OkObjectResult(testUserFavourite.InternshipId));
        }



        private Internship SetupInternship()
        {
            var testInternship = new Internship
            {
                InternshipId = 1,
                CompanyId = 1,
                ContactPersonId = 1,
                PromotorFirstname = "Peter",
                PromotorSurname = "Wevers",
                PromotorFunction = "IT-manager",
                PromotorEmail = "IT@testcompany.be",
                ProjectStatusId = 1,
                WpStreet = "Bakerstraat",
                WpHouseNr = "1",
                WpBusNr = "1",
                WpCity = "Hasselt",
                WpZipCode = "3500",
                WpCountry = "Belgium",
                AssignmentDescription = "Getriggerd door Azure? Top, kom jij dan bij ons stage lopen?" +
                 "Tijdens jouw stage focus jij je op Infrastructure as code (IaC) binnen Azure." +
                 "Dit is een softwarematige benadering van de IT-infrastructuur, waarbij door middel van templates de systemen " +
                 "op een consistente manier uitgerold en aangepast kunnen worden. Als er een wijziging moet plaatsvinden, " +
                 "wordt deze doorgevoerd in het template die vervolgens weer wordt uitgerold. ",
                TechnicalDetails = "Azure, IaC, python",
                Conditions = "Kennis van scripting.",
                ResearchTopicTitle = "Onderzoek Infrastructure as a code",
                ResearchTopicDescription = "Wat zijn best practices van Infrastructure as a code binnen Azure? Hoe pakt VanRoey.be dit het best aan?",
                TotalInternsRequired = 1,
                ContactStudentName = null,
                Remark = "Indien mogelijk zouden we met de aanvang van de stage flexibel willen omspringen, " +
                 "zodat een andere stagair op hetzelfde tijdstip zou kunnen beginnen. " +
                 "In duo werken biedt, uit onze ervaring, veel voordelen. Zowel voor de student als ons bedrijf.",
                CreatedAt = new DateTime(2020, 1, 1),
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = 3
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = 1
                    }
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = 2
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = 2
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = 3
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {
                    new InternshipAssignedUser
                    {
                        UserId = 4
                    }
                },
                InternshipReviewer = new List<InternshipReviewer>()
            };

            return testInternship;
        }
    }
}

