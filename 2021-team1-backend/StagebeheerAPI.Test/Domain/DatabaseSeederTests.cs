using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Moq;
using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;
using StagebeheerAPI.Domain.DatabaseSeeder;

namespace StagebeheerAPI.Test.Domain
{
    public class DatabaseSeederTests
    {
        private Mock<IRepositoryWrapper> _RepositoryWrapperMock;

        [Test]
        public void SeedCountries_CallsDatabase()
        {
            _RepositoryWrapperMock = new Mock<IRepositoryWrapper>();
            _RepositoryWrapperMock.Setup(x => x.Country.CreateRange(It.IsAny<List<Country>>()));

            var DatabaseSeeder = new DatabaseSeeder(_RepositoryWrapperMock.Object);
            DatabaseSeeder.SeedCountries();

            _RepositoryWrapperMock.Verify(mock => mock.Country.CreateRange(It.IsAny<List<Country>>()), Times.Once());
        }
    }
}
