using FluentAssertions;
using NUnit.Framework;
using StagebeheerAPI.FilterPattern;
using StagebeheerAPI.Models;
using System;
using System.Collections.Generic;

namespace StagebeheerAPI.Tests.Domain
{
    class FilterTests
    {
        [Test]
        public void DescriptionFilterMeetFilterReturnsAllInternshipsSearchingOnDescriptionEmptyString()
        {
            // Arrange
            var testInternships = SetupInternships();
            string testValue = "";
            var testDescriptionFilter = new DescriptionFilter(testValue);

            // Act
            var testResult = testDescriptionFilter.meetFilter(testInternships);

            // Assert
            Assert.AreEqual(testInternships.Count, testResult.Count);
        }
        [Test]
        public void DescriptionFilterMeetFilterReturnsOnlyInternshipsWithCorrectdescriptionInAssignmentDescription()
        {
            // Arrange
            var testInternships = SetupInternships();
            Random random = new Random();
            string testValue = "Testdescription " + random.Next(1, 4);
            var testDescriptionFilter = new DescriptionFilter(testValue);

            // Act
            var testResult = testDescriptionFilter.meetFilter(testInternships);

            // Assert
            foreach (Internship test in testResult)
            {
                Assert.AreEqual(testValue, test.AssignmentDescription);
            }
        }
        public void FavouritesFilterMeetFilterReturnsAllInternshipsSearchingOnUserIdZero()
        {
            // Arrange
            var testInternships = SetupInternships();
            int testValue = 0;
            var testFavouritesFilter = new FavouritesFilter(testValue);

            // Act
            var testResult = testFavouritesFilter.meetFilter(testInternships);

            // Assert
            Assert.AreEqual(testInternships.Count, testResult.Count);
        }
        [Test]
        public void FavouritesFilterMeetFilterReturnsOnlyInternshipsWithCorrectUserId()
        {
            // Arrange
            var testInternships = SetupInternships();
            Random random = new Random();
            int testValue = random.Next(21, 24);
            var testFavouritesFilter = new FavouritesFilter(testValue);

            // Act
            var testResult = testFavouritesFilter.meetFilter(testInternships);

            // Assert
            foreach (Internship test in testResult)
            {
                int counter = 0;
                foreach (UserFavourites fav in test.UserFavourites)
                {
                    if (testValue.Equals(fav.UserId))
                    {
                        counter++;
                    }
                    Assert.GreaterOrEqual(1, counter);
                }
            }
        }

        [Test]
        public void DescriptionFilterMeetFilterReturnsOnlyInternshipsWithCorrectdescriptionInResearchTopicTitle()
        {
            // Arrange
            var testInternships = SetupInternships();
            Random random = new Random();
            string testValue = "Title" + random.Next(1, 4);
            var testDescriptionFilter = new DescriptionFilter(testValue);

            // Act
            var testResult = testDescriptionFilter.meetFilter(testInternships);

            // Assert
            foreach (Internship test in testResult)
            {
                Assert.AreEqual(testValue, test.ResearchTopicTitle);
            }
        }
        [Test]
        public void CompanyFilterMeetFilterReturnsOnlyInternshipsWithCorrectCompanyId()
        {
            // Arrange
            var testInternships = SetupInternships();
            Random random = new Random();
            int testValue = random.Next(6, 9);
            var testCompanyFilter = new CompanyFilter(testValue);

            // Act
            var testResult = testCompanyFilter.meetFilter(testInternships);

            // Assert
            foreach(Internship test in testResult)
            {
                Assert.AreEqual(testValue, test.CompanyId);
            }
        }

        [Test]
        public void CompanyFilterMeetFilterReturnsAllInternshipsSearchingOnCompanyIdZero()
        {
            // Arrange
            var testInternships = SetupInternships();
            int testValue = 0;
            var testCompanyFilter = new CompanyFilter(testValue);

            // Act
            var testResult = testCompanyFilter.meetFilter(testInternships);

            // Assert
                Assert.AreEqual(testInternships.Count, testResult.Count);
        }

        [Test]
        public void EnvironmentFilterMeetFilterReturnsOnlyInternshipsWithCorrectEnvironmentId()
        {
            // Arrange
            var testInternships = SetupInternships();
            Random random = new Random();
            int testValue = random.Next(1, 4);
            var testEnvironmentFilter = new EnvironmentFilter(testValue);

            // Act
            var testResult = testEnvironmentFilter.meetFilter(testInternships);

            // Assert
            foreach (Internship test in testResult)
            {
                int counter = 0;
                foreach (InternshipEnvironment env in test.InternshipEnvironment)
                {
                    if (testValue.Equals(env.EnvironmentId))
                    {
                        counter++;
                    }
                    Assert.GreaterOrEqual(1, counter);
                }
            }
        }

        [Test]
        public void ExpectationFilterMeetFilterReturnsOnlyInternshipsWithCorrectExpectationtId()
        {
            // Arrange
            var testInternships = SetupInternships();
            Random random = new Random();
            int testValue = random.Next(3, 6);
            var testExpectationFilter = new ExpectationFilter(testValue);

            // Act
            var testResult = testExpectationFilter.meetFilter(testInternships);

            // Assert
            foreach (Internship test in testResult)
            {
                int counter = 0;
                foreach (InternshipExpectation exp in test.InternshipExpectation)
                {
                    if (testValue.Equals(exp.ExpectationId))
                    {
                        counter++;
                    }
                     Assert.GreaterOrEqual(1,counter);
                }                   
            }
        }

        [Test]
        public void PeriodFilterMeetFilterReturnsOnlyInternshipsWithCorrectPeriodId()
        {
            // Arrange
            var testInternships = SetupInternships();
            Random random = new Random();
            int testValue = random.Next(1, 4);
            var testPeriodFilter = new PeriodFilter(testValue);

            // Act
            var testResult = testPeriodFilter.meetFilter(testInternships);

            // Assert
            foreach (Internship test in testResult)
            {
                int counter = 0;
                foreach (InternshipPeriod per in test.InternshipPeriod)
                {
                    if (testValue.Equals(per.PeriodId))
                    {
                        counter++;
                    }
                    Assert.GreaterOrEqual(1, counter);
                }
            }
        }

        [Test]
        public void SpecialisationFilterMeetFilterReturnsOnlyInternshipsWithCorrectSpecialisationId()
        {
            // Arrange
            var testInternships = SetupInternships();
            Random random = new Random();
            int testValue = random.Next(2, 5);
            var testSpecialisationFilter = new SpecialisationFilter(testValue);

            // Act
            var testResult = testSpecialisationFilter.meetFilter(testInternships);

            // Assert
            foreach (Internship test in testResult)
            {
                int counter = 0;
                foreach (InternshipSpecialisation spec in test.InternshipSpecialisation)
                {
                    if (testValue.Equals(spec.SpecialisationId))
                    {
                        counter++;
                    }
                    Assert.GreaterOrEqual(1, counter);
                }
            }
        }

        //[Test]
        public void comboFiltering_FiltersInternships_WithMultipleSpecialisationsInFilterCriteria()
        {
            // Arrange
            var filterCriteria = new Internship
            {
                UserFavourites = new List<UserFavourites>(),
                AssignmentDescription = "",
                InternshipEnvironmentOthers = "",
                InternshipEnvironment = new List<InternshipEnvironment>(),
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation { SpecialisationId = 2 },
                    new InternshipSpecialisation { SpecialisationId = 3 },
                }
            };

            var internshipsInDatabase = new List<Internship>
            {
                new Internship
                {
                    InternshipId = 1,   // Ok
                    InternshipSpecialisation = new List<InternshipSpecialisation>
                    {
                        new InternshipSpecialisation { SpecialisationId = 2 },
                    }
                },
                new Internship
                {
                    InternshipId = 2,   // Ok
                    InternshipSpecialisation = new List<InternshipSpecialisation>
                    {
                        new InternshipSpecialisation { SpecialisationId = 3 },
                        new InternshipSpecialisation { SpecialisationId = 4 },
                    }
                },
                new Internship
                {
                    InternshipId = 3,   // Niet juist!
                    InternshipSpecialisation = new List<InternshipSpecialisation>
                    {
                        new InternshipSpecialisation { SpecialisationId = 1 },
                        new InternshipSpecialisation { SpecialisationId = 4 },
                    }
                },
            };
            var combinedFilter = new CombinedFilter();

            // Act
            var result = combinedFilter.comboFiltering(internshipsInDatabase, filterCriteria);

            // Assert
            result.Count.Should().Be(2);
            result[0].InternshipId.Should().Be(1);
            result[1].InternshipId.Should().Be(2);
        }

        private List<Internship> SetupInternships()
        {
            //int testCounter = 1;
            List<Internship> internships = new List<Internship>();

            for (int testCounter=1; testCounter <4; testCounter++)
            {
                var testInternship = new Internship
                {
                    InternshipId = testCounter + 10,
                    CompanyId = testCounter + 5,
                    AssignmentDescription = "Testdescription " + testCounter,
                    ResearchTopicTitle = "Title" + testCounter,
                    InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = testCounter
                    }
                },
                    InternshipEnvironmentOthers = null,
                    InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = testCounter
                    }
                },
                    InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = testCounter +1
                    }
                },
                    InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = testCounter +2
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = testCounter + 1
                    }
                },
                    UserFavourites = new List<UserFavourites>
                {
                    new UserFavourites
                    {
                        UserId = testCounter +20
                    }
                }
                };
                internships.Add(testInternship);
            } 

            return internships;
        }
    }
}
