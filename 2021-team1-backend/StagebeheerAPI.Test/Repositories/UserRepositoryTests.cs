using FluentAssertions;
using NUnit.Framework;
using StagebeheerAPI.Models;
using StagebeheerAPI.Repository;
using System.Collections.Generic;

namespace StagebeheerAPI.Tests.Repositories
{
    class UserRepositoryTests
    {
        private UserRepository _UserRepository;  
        private StagebeheerDBContext _StagebeheerDbContext;

        [SetUp]
        public void Setup()
        {       
            _UserRepository = new UserRepository(_StagebeheerDbContext);       
        }

        [Test]
        public void CheckReviewerDataIsValid()
        {
            string dataError;
            int commas = 2;
            string[] rows = { "voornaamtest", "naamtest", "test@test.test" };
            dataError = _UserRepository.CheckReviewerData(rows, commas);
            dataError.Should().BeNull();
        }

        [Test]
        public void CheckReviewerDataIsNotValid()
        {
            string dataError;
            int commas = 2;
            string[] rows = { "voornaamtest", "naamtest", "error" };
            dataError = _UserRepository.CheckReviewerData(rows, commas);
            dataError.Should().Be("e-mailadres opmaak is niet correct");
        }

        [Test]
        public void CheckStudentDataIsValid()
        {
            string dataError;
            int commas = 9;
            string[] rows = { "voornaamtest", "naamtest", "straattest", "huisnrtest", "bustest", "pctest", "gemeentetest", "gsmnummertest", "test@test.test", "afstudeerrichtingtest" };
            dataError = _UserRepository.CheckStudentData(rows, commas);
            dataError.Should().BeNull();
        }

        [Test]
        public void CheckStudentDataIsNotValid()
        {
            string dataError;
            int commas = 9;
            string[] rows = { "voornaamtest", "naamtest", "straattest", "huisnrtest", "bustest", "pctest", "gemeentetest", "gsmnummertest", "error", "afstudeerrichtingtest" };
            dataError = _UserRepository.CheckStudentData(rows, commas);
            dataError.Should().Be("e-mailadres opmaak is niet correct");
        }

        [Test]
        public void CheckReviewerHeaderIsValid()
        {
            string dataError;
            List<string> headers = new List<string>
            { "voornaam", "naam", "e-mailadres"};
            dataError = _UserRepository.CheckReviewerHeader(headers);
            dataError.Should().BeNull();
        }

        [Test]
        public void CheckReviewerHeaderIsNotValid()
        {
            string dataError;
            List<string> headers = new List<string>
            { "voornaam", "naam", "error"};
            dataError = _UserRepository.CheckReviewerHeader(headers);
            dataError.Should().Be("mailadres veld niet aanwezig in invoerbestand.");
        }

        [Test]
        public void CheckStudentHeaderIsValid()
        {
            string dataError;
            List<string> headers = new List<string>
            { "voornaam", "naam", "straat", "huisnr", "bus", "pc", "gemeente", "gsmnummer", "e-mailadres", "afstudeerrichting" };
            dataError = _UserRepository.CheckStudentHeader(headers);
            dataError.Should().BeNull();
        }

        [Test]
        public void CheckStudentHeaderIsNotValid()
        {
            string dataError;
            List<string> headers = new List<string>
            { "voornaam", "naam", "straat", "huisnr", "bus", "pc", "gemeente", "gsmnummer", "error", "afstudeerrichting" };
            dataError = _UserRepository.CheckStudentHeader(headers);
            dataError.Should().Be("mailadres veld niet aanwezig in invoerbestand.");
        }
    }
}
