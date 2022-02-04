using FluentAssertions;
using Moq;
using NUnit.Framework;
using StagebeheerAPI.Controllers;
using StagebeheerAPI.Models;
using StagebeheerAPI.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using StagebeheerAPI.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MockQueryable.Moq;
using Microsoft.EntityFrameworkCore;

namespace StagebeheerAPI.Test.Controllers.Unit
{
    class UserControllerTests
    {
        private UserController _UserController;

        private Mock<IRepositoryWrapper> _RepositoryWrapperMock;

        [SetUp]
        public void Setup()
        {
            _RepositoryWrapperMock = new Mock<IRepositoryWrapper>();
        }

        [Test]
        public async Task GetMentors_ReturnMentors_WhenMentorsExist()
        {
            // Arrange
            int testRoleId = 3;

            var testReviewer = SetupReviewer();

            var testReviewer2 = SetupReviewer();
            testReviewer2.UserId = 10;
            testReviewer2.RoleId = 2;

            var testReviewersVerification = new List<User>();
            testReviewersVerification.Add(testReviewer);

            var testReviewers = new List<User>();
            testReviewers.Add(testReviewer);
            testReviewers.Add(testReviewer2);

            var reviewerMock = testReviewers.AsQueryable().BuildMock();

            _RepositoryWrapperMock.Setup(x => x.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).Returns(reviewerMock.Object.Where(x => x.RoleId == testRoleId));

            _UserController = new UserController(_RepositoryWrapperMock.Object);

            // Act
            var apiResult = await _UserController.GetReviewers();
            var contactResult = apiResult.Value as List<User>;

            // Assert
            contactResult.Should().BeEquivalentTo(testReviewersVerification);
        }

        [Test]
        public async Task GetMentors_ReturnsEmptyList_WhenMentorsDoNotExist()
        {
            // Arrange
            int testRoleId = 2;

            var testReviewer = SetupReviewer();

            var testReviewers = new List<User>();
            testReviewers.Add(testReviewer);

            var reviewerMock = testReviewers.AsQueryable().BuildMock();

            _RepositoryWrapperMock.Setup(x => x.User.FindByCondition(It.IsAny<Expression<Func<User, bool>>>())).Returns(reviewerMock.Object.Where(x => x.RoleId == testRoleId));

            _UserController = new UserController(_RepositoryWrapperMock.Object);

            // Act
            var apiResult = await _UserController.GetReviewers();
            var contactResult = apiResult.Value as List<User>;

            // Assert
            contactResult.Should().BeEquivalentTo(new List<User>());
        
        }

        private User SetupReviewer()
        {
            var genericPassword = "AQAAAAEAACcQAAAAEPvBo3i7bpJn1faRTkFqaIQoS5B+ikTQfPR3TD8nbcy1aGItn2Z/kH8BLcqmS0SyWQ==";
            var genericRegistratioDate = new DateTime(2020, 01, 01);

            var testReviewer = new User
            {
                UserEmailAddress = "sbp.pxl.stagereviewer@gmail.com".ToUpper(),
                UserPass = genericPassword,
                RegistrationDate = genericRegistratioDate,
                Activated = true,
                RoleId = 3
            };

            return testReviewer;
        }

    }
}
