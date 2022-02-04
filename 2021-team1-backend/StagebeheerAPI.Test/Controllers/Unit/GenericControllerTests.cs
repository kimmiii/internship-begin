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
    class GenericControllerTests
    {
        private GenericController _GenericController;

        private Mock<IRepositoryWrapper> _RepositoryWrapperMock;

        [SetUp]
        public void Setup()
        {
            _RepositoryWrapperMock = new Mock<IRepositoryWrapper>();
        }

        [Test]
        public async Task GetCountries_ReturnsListWithCountries_WhenTheyExist()
        {
            // Arrange
           var testCountries = new List<Country>
           { new Country
                    { CountryId = 1,
                    Code ="BE",
                    Name = "België"
                    },
                     new Country
                    { CountryId = 2,
                    Code ="NL",
                    Name = "Nederland"
                     }
           };
            var countriesMock = testCountries.AsQueryable().BuildMock();

            _RepositoryWrapperMock.Setup(x => x.Country.FindAll()).Returns(countriesMock.Object);

            _GenericController = new GenericController(_RepositoryWrapperMock.Object);

            // Act
            var apiResult = await _GenericController.GetCountries();
            var countryResult = apiResult.Value as List<Country>;

            // Assert
            countryResult.Should().BeEquivalentTo(testCountries);
        }
    }
}
