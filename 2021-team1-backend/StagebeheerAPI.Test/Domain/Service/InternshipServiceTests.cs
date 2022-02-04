using StagebeheerAPI.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using StagebeheerAPI.Contracts;
using EmailService;
using StagebeheerAPI.Models;
using System.Linq.Expressions;
using System.Linq;
using MockQueryable.Moq;
using StagebeheerAPI.Domain.Services;

namespace StagebeheerAPI.Tests.Domain.Service
{
    public class InternshipServiceTests
    {

        private Mock<IRepositoryWrapper> repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        private Mock<IEmailSender> emailServiceMock = new Mock<IEmailSender>();
        private InternshipService internshipService;

        private const string ROLE_COORDINATOR = "COO";
        private const string ROLE_COMPANY = "COM";
        private const string ROLE_REVIEWER = "REV";

        private const string STATUS_NEW = "NEW";
        private const string STATUS_APPROVED = "APP";
        private const string STATUS_REJECTED = "REJ";
        private const string STATUS_REVIEW = "REV";

        public InternshipServiceTests()
        {
            internshipService = new InternshipService(repositoryWrapperMock.Object, emailServiceMock.Object);
        }


        [Test]
        public async Task CheckProcessFlow_Coordinator_Can_Approve_New_Request()
        {
            //Arrange            
            var projectStatus = new ProjectStatus { Code = STATUS_APPROVED };
            var internship = new Internship { InternshipId = 1, ProjectStatus = projectStatus, ContactPersonId = 2 };

            var statusNew = new ProjectStatus { Code = STATUS_NEW };
            var internshipIM = new Internship
            {
                InternshipId = 1,
                ProjectStatus = statusNew
            };


            var internships = new List<Internship>();
            internships.Add(internshipIM);

            var internshipMock = internships.AsQueryable().BuildMock();
            repositoryWrapperMock.Setup(x => x.Internship.FindByCondition(It.IsAny<Expression<Func<Internship, bool>>>())).Returns(internshipMock.Object);

            //Act
            var processFlowResult = internshipService.CheckProcessFlow(internship, ROLE_COORDINATOR, ROLE_COMPANY);

            //Assert
            Assert.IsTrue(processFlowResult.FlowAllowed);

        }


        [Test]
        public async Task CheckProcessFlow_Coordinator_Can_Reject_New_Request()
        {
            //Arrange            
            var projectStatus = new ProjectStatus { Code = STATUS_REJECTED };
            var internship = new Internship { InternshipId = 1, ProjectStatus = projectStatus, ContactPersonId = 2 };

            var statusNew = new ProjectStatus { Code = STATUS_NEW };
            var internshipIM = new Internship
            {
                InternshipId = 1,
                ProjectStatus = statusNew
            };


            var internships = new List<Internship>();
            internships.Add(internshipIM);

            var internshipMock = internships.AsQueryable().BuildMock();
            repositoryWrapperMock.Setup(x => x.Internship.FindByCondition(It.IsAny<Expression<Func<Internship, bool>>>())).Returns(internshipMock.Object);

            //Act
            var processFlowResult = internshipService.CheckProcessFlow(internship, ROLE_COORDINATOR, ROLE_COMPANY);

            //Assert
            Assert.IsTrue(processFlowResult.FlowAllowed);

        }


        [Test]
        public async Task CheckProcessFlow_Reviewer_Cannot_Approve_New_Request()
        {
            //Arrange            
            var projectStatus = new ProjectStatus { Code = STATUS_APPROVED };
            var internship = new Internship { InternshipId = 1, ProjectStatus = projectStatus, ContactPersonId = 3 };

            var statusNew = new ProjectStatus { Code = STATUS_NEW };
            var internshipIM = new Internship
            {
                InternshipId = 1,
                ProjectStatus = statusNew
            };


            var internships = new List<Internship>();
            internships.Add(internshipIM);

            var internshipMock = internships.AsQueryable().BuildMock();
            repositoryWrapperMock.Setup(x => x.Internship.FindByCondition(It.IsAny<Expression<Func<Internship, bool>>>())).Returns(internshipMock.Object);

            //Act
            var processFlowResult = internshipService.CheckProcessFlow(internship, ROLE_REVIEWER, ROLE_COMPANY);

            //Assert
            Assert.IsNull(processFlowResult);

        }

        [Test]
        public async Task CheckProcessFlow_Coordinator_Can_Assign_To_Reviewer()
        {
            //Arrange            
            var projectStatus = new ProjectStatus { Code = STATUS_REVIEW };
            var internship = new Internship { InternshipId = 1, ProjectStatus = projectStatus, ContactPersonId = 3 };

            var statusNew = new ProjectStatus { Code = STATUS_NEW };
            var internshipIM = new Internship
            {
                InternshipId = 1,
                ProjectStatus = statusNew
            };


            var internships = new List<Internship>();
            internships.Add(internshipIM);

            var internshipMock = internships.AsQueryable().BuildMock();
            repositoryWrapperMock.Setup(x => x.Internship.FindByCondition(It.IsAny<Expression<Func<Internship, bool>>>())).Returns(internshipMock.Object);

            //Act
            var processFlowResult = internshipService.CheckProcessFlow(internship, ROLE_COORDINATOR, ROLE_REVIEWER);

            //Assert
            Assert.IsTrue(processFlowResult.FlowAllowed);

        }


        [Test]
        public async Task CheckProcessFlow_Reviewer_Can_Assign_To_Coordinator()
        {
            //Arrange            
            var projectStatus = new ProjectStatus { Code = STATUS_REVIEW };
            var internship = new Internship { InternshipId = 1, ProjectStatus = projectStatus, ContactPersonId = 3 };

            var statusNew = new ProjectStatus { Code = STATUS_REVIEW };
            var internshipIM = new Internship
            {
                InternshipId = 1,
                ProjectStatus = statusNew
            };


            var internships = new List<Internship>();
            internships.Add(internshipIM);

            var internshipMock = internships.AsQueryable().BuildMock();
            repositoryWrapperMock.Setup(x => x.Internship.FindByCondition(It.IsAny<Expression<Func<Internship, bool>>>())).Returns(internshipMock.Object);

            //Act
            var processFlowResult = internshipService.CheckProcessFlow(internship, ROLE_REVIEWER, ROLE_COORDINATOR);

            //Assert
            Assert.IsTrue(processFlowResult.FlowAllowed);

        }




    }
}
