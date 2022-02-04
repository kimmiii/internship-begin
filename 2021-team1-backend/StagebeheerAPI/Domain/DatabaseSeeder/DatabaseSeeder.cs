using Newtonsoft.Json;
using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace StagebeheerAPI.Domain.DatabaseSeeder
{
    [ExcludeFromCodeCoverage]
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private IRepositoryWrapper _RepositoryWrapper;

        public DatabaseSeeder(IRepositoryWrapper repositoryWrapper)
        {
            _RepositoryWrapper = repositoryWrapper;
        }

        public void SeedAllTables(bool shouldOverwrite = false)
        {
            SeedStaticTables(shouldOverwrite);
            SeedTestData(shouldOverwrite);
        }

        public void SeedStaticTables(bool shouldOverwrite = false)
        {
            SeedCountries(shouldOverwrite);
            SeedPeriods(shouldOverwrite);
            SeedExpectations(shouldOverwrite);
            SeedSpecializations(shouldOverwrite);
            SeedRoles(shouldOverwrite);
            SeedStatus(shouldOverwrite);
            SeedEnvironments(shouldOverwrite);
        }

        public void SeedTestData(bool shouldOverwrite = false)
        {
            SeedUser(shouldOverwrite);
            SeedCompanies(shouldOverwrite);
            SeedContacts(shouldOverwrite);
            SeedInternships(shouldOverwrite);
        }

        public void SeedCountries(bool shouldOverwrite = false)
        {
            Console.WriteLine("Seeding countries");

            var countriesInDatabase = _RepositoryWrapper.Country.FindAll().ToList();

            var countriesFromJson = JsonConvert.DeserializeObject<List<Country>>(File.ReadAllText("Configuration/CountriesList.json"));

            List<Country> countriesToAdd = new List<Country>();
            if (shouldOverwrite)
            {
                _RepositoryWrapper.Country.DeleteRange(countriesInDatabase);
                _RepositoryWrapper.Save();
                countriesToAdd = countriesFromJson;
            }
            else
            {
                // Look for countries that are not yet in the database.
                countriesToAdd = countriesFromJson.Where(jsonCountry => !countriesInDatabase.Any(dbCountry => jsonCountry.Code == dbCountry.Code)).ToList();
            }

            _RepositoryWrapper.Country.CreateRange(countriesToAdd);
            _RepositoryWrapper.Save();
        }

        public void SeedPeriods(bool shouldOverwrite = false)
        {
            Console.WriteLine("Seeding periods");

            var periodsInDatabase = _RepositoryWrapper.Period.FindAll().ToList();

            var periodsFromJson = JsonConvert.DeserializeObject<List<Period>>(File.ReadAllText("Configuration/PeriodsList.json"));

            List<Period> periodsToAdd = new List<Period>();
            if (shouldOverwrite)
            {
                _RepositoryWrapper.Period.DeleteRange(periodsInDatabase);
                _RepositoryWrapper.Save();
                periodsToAdd = periodsFromJson;
            }
            else
            {
                // Look for periods that are not yet in the database.
                periodsToAdd = periodsFromJson.Where(jsonPeriod => !periodsInDatabase.Any(dbPeriod => jsonPeriod.Code == dbPeriod.Code)).ToList();
            }

            _RepositoryWrapper.Period.CreateRange(periodsToAdd);
            _RepositoryWrapper.Save();
        }

        public void SeedExpectations(bool shouldOverwrite = false)
        {
            Console.WriteLine("Seeding expectations");

            var expectationsInDatabase = _RepositoryWrapper.Expectation.FindAll().ToList();

            var expectationsFromJson = JsonConvert.DeserializeObject<List<Expectation>>(File.ReadAllText("Configuration/ExpectationsList.json"));

            List<Expectation> expecationsToAdd = new List<Expectation>();
            if (shouldOverwrite)
            {
                _RepositoryWrapper.Expectation.DeleteRange(expectationsInDatabase);
                _RepositoryWrapper.Save();
                expecationsToAdd = expectationsFromJson;
            }
            else
            {
                // Look for Expectations that are not yet in the database.
                expecationsToAdd = expectationsFromJson.Where(jsonExpectation => !expectationsInDatabase.Any(dbObject => jsonExpectation.Code == dbObject.Code)).ToList();
            }

            _RepositoryWrapper.Expectation.CreateRange(expecationsToAdd);
            _RepositoryWrapper.Save();
        }

        public void SeedSpecializations(bool shouldOverwrite = false)
        {
            Console.WriteLine("Seeding specializations");

            var specializationsInDatabase = _RepositoryWrapper.Specialisation.FindAll().ToList();

            var specializationsFromJson = JsonConvert.DeserializeObject<List<Specialisation>>(File.ReadAllText("Configuration/SpecializationsList.json"));

            List<Specialisation> specializationsToAdd = new List<Specialisation>();
            if (shouldOverwrite)
            {
                _RepositoryWrapper.Specialisation.DeleteRange(specializationsInDatabase);
                _RepositoryWrapper.Save();
                specializationsToAdd = specializationsFromJson;
            }
            else
            {
                // Look for Specializations that are not yet in the database.
                specializationsToAdd = specializationsFromJson.Where(jsonSpecialization => !specializationsInDatabase.Any(dbObject => jsonSpecialization.Code == dbObject.Code)).ToList();
            }

            _RepositoryWrapper.Specialisation.CreateRange(specializationsToAdd);
            _RepositoryWrapper.Save();
        }

        public void SeedRoles(bool shouldOverwrite = false)
        {
            Console.WriteLine("Seeding roles");

            var rolesInDatabase = _RepositoryWrapper.Role.FindAll().ToList();

            var rolesFromJson = JsonConvert.DeserializeObject<List<Role>>(File.ReadAllText("Configuration/RolesList.json"));

            List<Role> rolesToAdd = new List<Role>();
            if (shouldOverwrite)
            {
                _RepositoryWrapper.Role.DeleteRange(rolesInDatabase);
                _RepositoryWrapper.Save();
                rolesToAdd = rolesFromJson;
            }
            else
            {
                // Look for Roles that are not yet in the database.
                rolesToAdd = rolesFromJson.Where(jsonRole => !rolesInDatabase.Any(dbObject => jsonRole.Code == dbObject.Code)).ToList();
            }

            _RepositoryWrapper.Role.CreateRange(rolesToAdd);
            _RepositoryWrapper.Save();
        }

        public void SeedStatus(bool shouldOverwrite = false)
        {
            Console.WriteLine("Seeding project statuses");

            var statusInDatabase = _RepositoryWrapper.ProjectStatus.FindAll().ToList();

            var statusFromJson = JsonConvert.DeserializeObject<List<ProjectStatus>>(File.ReadAllText("Configuration/StatusList.json"));

            List<ProjectStatus> statusToAdd = new List<ProjectStatus>();
            if (shouldOverwrite)
            {
                _RepositoryWrapper.ProjectStatus.DeleteRange(statusInDatabase);
                _RepositoryWrapper.Save();
                statusToAdd = statusFromJson;
            }
            else
            {
                // Look for statusses that are not yet in the database.
                statusToAdd = statusFromJson.Where(jsonStatus => !statusInDatabase.Any(dbObject => jsonStatus.Description == dbObject.Description)).ToList();
            }

            _RepositoryWrapper.ProjectStatus.CreateRange(statusToAdd);
            _RepositoryWrapper.Save();
        }

        public void SeedEnvironments(bool shouldOverwrite = false)
        {
            Console.WriteLine("Seeding environments");

            var environmentsInDatabase = _RepositoryWrapper.Environment.FindAll().ToList();

            var environmentsFromJson = JsonConvert.DeserializeObject<List<Models.Environment>>(File.ReadAllText("Configuration/EnvironmentsList.json"));

            List<Models.Environment> environmentsToAdd = new List<Models.Environment>();
            if (shouldOverwrite)
            {
                _RepositoryWrapper.Environment.DeleteRange(environmentsInDatabase);
                _RepositoryWrapper.Save();
                environmentsToAdd = environmentsFromJson;
            }
            else
            {
                // Look for statusses that are not yet in the database.
                environmentsToAdd = environmentsFromJson.Where(jsonEnvironent => !environmentsInDatabase.Any(dbObject => jsonEnvironent.Description == dbObject.Description)).ToList();
            }

            _RepositoryWrapper.Environment.CreateRange(environmentsToAdd);
            _RepositoryWrapper.Save();
        }

        // ---------
        // TEST DATA
        // ---------
        public void SeedUser(bool shouldOverwrite = false)
        {
            Console.WriteLine("Seeding users");

            var genericPassword = "AQAAAAEAACcQAAAAEPvBo3i7bpJn1faRTkFqaIQoS5B+ikTQfPR3TD8nbcy1aGItn2Z/kH8BLcqmS0SyWQ==";
            var genericRegistratioDate = new DateTime(2020, 01, 01);

            var rolesInDatabase = _RepositoryWrapper.Role.FindAll().ToList();
            if (rolesInDatabase == null) throw new Exception("Please add roles to database first");
            var usersInDatabase = _RepositoryWrapper.User.FindAll().ToList();

            List<User> usersToAdd = new List<User>();

            // Companies
            usersToAdd.Add(new User
            {
                UserEmailAddress = "sbp.pxl.company1@gmail.com",
                UserFirstName = null,
                UserSurname = null,
                UserPass = genericPassword,
                RegistrationDate = genericRegistratioDate,
                Activated = true,
                CvPresent = false,
                RoleId = rolesInDatabase.Where(role => role.Code == "COM").First().RoleId
            });

            usersToAdd.Add(new User
            {
                UserEmailAddress = "sbp.pxl.company2@gmail.com",
                UserFirstName = null,
                UserSurname = null,
                UserPass = genericPassword,
                RegistrationDate = genericRegistratioDate,
                Activated = true,
                CvPresent = false,
                RoleId = rolesInDatabase.Where(role => role.Code == "COM").First().RoleId
            });

            usersToAdd.Add(new User
            {
                UserEmailAddress = "sbp.pxl.company3@gmail.com",
                UserFirstName = null,
                UserSurname = null,
                UserPass = genericPassword,
                RegistrationDate = genericRegistratioDate,
                Activated = true,
                CvPresent = false,
                RoleId = rolesInDatabase.Where(role => role.Code == "COM").First().RoleId
            });

            usersToAdd.Add(new User
            {
                UserEmailAddress = "sbp.pxl.company4@gmail.com",
                UserFirstName = null,
                UserSurname = null,
                UserPass = genericPassword,
                RegistrationDate = genericRegistratioDate,
                Activated = true,
                CvPresent = false,
                RoleId = rolesInDatabase.Where(role => role.Code == "COM").First().RoleId
            });

            usersToAdd.Add(new User
            {
                UserEmailAddress = "jan.janssen@cegeka.com",
                UserFirstName = null,
                UserSurname = null,
                UserPass = genericPassword,
                RegistrationDate = genericRegistratioDate,
                Activated = true,
                CvPresent = false,
                RoleId = rolesInDatabase.Where(role => role.Code == "COM").First().RoleId
            });

            usersToAdd.Add(new User
            {
                UserEmailAddress = "email.ramaeckers@jarchitects.com",
                UserFirstName = null,
                UserSurname = null,
                UserPass = genericPassword,
                RegistrationDate = genericRegistratioDate,
                Activated = true,
                CvPresent = false,
                RoleId = rolesInDatabase.Where(role => role.Code == "COM").First().RoleId
            });

            usersToAdd.Add(new User
            {
                UserEmailAddress = "lucas.grootveld@zappware.com",
                UserFirstName = null,
                UserSurname = null,
                UserPass = genericPassword,
                RegistrationDate = genericRegistratioDate,
                Activated = true,
                CvPresent = false,
                RoleId = rolesInDatabase.Where(role => role.Code == "COM").First().RoleId
            });

            usersToAdd.Add(new User
            {
                UserEmailAddress = "harry.bornvoets@happyvoc.com",
                UserFirstName = null,
                UserSurname = null,
                UserPass = genericPassword,
                RegistrationDate = genericRegistratioDate,
                Activated = true,
                CvPresent = false,
                RoleId = rolesInDatabase.Where(role => role.Code == "COM").First().RoleId
            });

            usersToAdd.Add(new User
            {
                UserEmailAddress = "sergio.slissers@abinbev.com",
                UserFirstName = null,
                UserSurname = null,
                UserPass = genericPassword,
                RegistrationDate = genericRegistratioDate,
                Activated = true,
                CvPresent = false,
                RoleId = rolesInDatabase.Where(role => role.Code == "COM").First().RoleId
            });

            usersToAdd.Add(new User
            {
                UserEmailAddress = "hanne.put@cronosgroup.be",
                UserFirstName = null,
                UserSurname = null,
                UserPass = genericPassword,
                RegistrationDate = genericRegistratioDate,
                Activated = true,
                CvPresent = false,
                RoleId = rolesInDatabase.Where(role => role.Code == "COM").First().RoleId
            });

            usersToAdd.Add(new User
            {
                UserEmailAddress = "sophie.steegmans@3it.be",
                UserFirstName = null,
                UserSurname = null,
                UserPass = genericPassword,
                RegistrationDate = genericRegistratioDate,
                Activated = true,
                CvPresent = false,
                RoleId = rolesInDatabase.Where(role => role.Code == "COM").First().RoleId
            });

            // Stagecoordinators
            usersToAdd.Add(new User
            {
                UserEmailAddress = "sbp.pxl.stagecoordinator1@gmail.com",
                UserFirstName = "Marijke",
                UserSurname = "Willems",
                UserPass = genericPassword,
                RegistrationDate = genericRegistratioDate,
                Activated = true,
                CvPresent = false,
                RoleId = rolesInDatabase.Where(role => role.Code == "COO").First().RoleId
            });

            usersToAdd.Add(new User
            {
                UserEmailAddress = "sbp.pxl.stagecoordinator2@gmail.com",
                UserFirstName = "Bart",
                UserSurname = "Stukken",
                UserPass = genericPassword,
                RegistrationDate = genericRegistratioDate,
                Activated = true,
                CvPresent = false,
                RoleId = rolesInDatabase.Where(role => role.Code == "COO").First().RoleId
            });

            // Lectors
            usersToAdd.Add(new User
            {
                UserEmailAddress = "sbp.pxl.stagereviewer1@gmail.com",
                UserFirstName = "Peter",
                UserSurname = "Peeters",
                UserPass = genericPassword,
                RegistrationDate = genericRegistratioDate,
                Activated = true,
                CvPresent = false,
                RoleId = rolesInDatabase.Where(role => role.Code == "REV").First().RoleId
            });

            usersToAdd.Add(new User
            {
                UserEmailAddress = "sbp.pxl.stagereviewer2@gmail.com",
                UserFirstName = "Jan",
                UserSurname = "Janssen",
                UserPass = genericPassword,
                RegistrationDate = genericRegistratioDate,
                Activated = true,
                CvPresent = false,
                RoleId = rolesInDatabase.Where(role => role.Code == "REV").First().RoleId
            });

            // Students
            usersToAdd.Add(new User
            {
                UserEmailAddress = "sbp.pxl.student1@gmail.com",
                UserFirstName = "Paul",
                UserSurname = "Paulussen",
                UserPass = genericPassword,
                RegistrationDate = genericRegistratioDate,
                Activated = true,
                CvPresent = false,
                RoleId = rolesInDatabase.Where(role => role.Code == "STU").First().RoleId
            });

            usersToAdd.Add(new User
            {
                UserEmailAddress = "sbp.pxl.student2@gmail.com",
                UserFirstName = "Pieter",
                UserSurname = "Pietersen",
                UserPass = genericPassword,
                RegistrationDate = genericRegistratioDate,
                Activated = true,
                CvPresent = true,
                RoleId = rolesInDatabase.Where(role => role.Code == "STU").First().RoleId
            });

            usersToAdd.Add(new User
            {
                UserEmailAddress = "sbp.pxl.student3@gmail.com",
                UserFirstName = "Jack",
                UserSurname = "Jackson",
                UserPass = genericPassword,
                RegistrationDate = genericRegistratioDate,
                Activated = true,
                CvPresent = false,
                RoleId = rolesInDatabase.Where(role => role.Code == "STU").First().RoleId
            });

            usersToAdd.Add(new User
            {
                UserEmailAddress = "sbp.pxl.student4@gmail.com",
                UserFirstName = "Will",
                UserSurname = "Willson",
                UserPass = genericPassword,
                RegistrationDate = genericRegistratioDate,
                Activated = true,
                CvPresent = false,
                RoleId = rolesInDatabase.Where(role => role.Code == "STU").First().RoleId
            });

            if (shouldOverwrite)
            {
                _RepositoryWrapper.User.DeleteRange(usersInDatabase);
                _RepositoryWrapper.Save();
            }
            else
            {
                // Look for users that are not yet in the database.
                usersToAdd = usersToAdd.Where(newUser => !usersInDatabase.Any(dbObject => newUser.UserEmailAddress == dbObject.UserEmailAddress)).ToList();
            }

            foreach (var user in usersToAdd)
            {
                Console.WriteLine($" - User email address: {user.UserEmailAddress}, Role: {rolesInDatabase.Where(role => role.RoleId == user.RoleId).First().Code}");
            }

            _RepositoryWrapper.User.CreateRange(usersToAdd);
            _RepositoryWrapper.Save();
        }

        public void SeedCompanies(bool shouldOverwrite = false)
        {
            Console.WriteLine("Seeding companies");
            var companiesInDatabase = _RepositoryWrapper.Company.FindAll().ToList();
            var usersInDatabase = _RepositoryWrapper.User.FindAll().ToList();
            var rolesInDatabase = _RepositoryWrapper.Role.FindAll().ToList();

            var roleIdForCompany = rolesInDatabase.Where(role => role.Code == "COM").First().RoleId;

            List<Company> companiesToAdd = new List<Company>();

            // Actived company
            var company1 = new Company
            {
                Activated = true,
                BusNr = "A",
                City = "Hasselt",
                Country = "Belgium",
                Email = "info@company1.be",
                HouseNr = "1",
                Name = "Company1",
                PhoneNumber = "+3211020304",
                Street = "Grovestraat",
                TotalEmployees = 10,
                TotalITEmployees = 5,
                TotalITEmployeesActive = 4,
                VATNumber = "123.4565.789",
                ZipCode = "3500",
                UserId = usersInDatabase.Where(user => user.RoleId == roleIdForCompany).ToList()[10].UserId // sbp.pxl.company1@gmail.com
            };
            Console.WriteLine($" - {company1.Name}");
            companiesToAdd.Add(company1);

            // Activated company
            var company2 = new Company
            {
                Activated = true,
                BusNr = "1",
                City = "Lanaken",
                Country = "Belgium",
                Email = "info@company2.be",
                HouseNr = "24",
                Name = "Company2",
                PhoneNumber = "+3211123456",
                Street = "Steenweg",
                TotalEmployees = 100,
                TotalITEmployees = 50,
                TotalITEmployeesActive = 10,
                VATNumber = "987.6543.210",
                ZipCode = "3621",
                UserId = usersInDatabase.Where(user => user.RoleId == roleIdForCompany).ToList()[7].UserId // sbp.pxl.company2@gmail.com
            };
            Console.WriteLine($" - {company2.Name}");
            companiesToAdd.Add(company2);

            // Refused company
            var company3 = new Company
            {
                Activated = false,
                BusNr = "1",
                City = "Lanaken",
                Country = "Belgium",
                Email = "info@company3.be",
                HouseNr = "24",
                Name = "Company3",
                PhoneNumber = "+3211123456",
                Street = "Steenweg",
                TotalEmployees = 100,
                TotalITEmployees = 50,
                TotalITEmployeesActive = 10,
                VATNumber = "123.9876.123",
                ZipCode = "3621",
                EvaluatedAt = DateTime.Now,
                UserId = usersInDatabase.Where(user => user.RoleId == roleIdForCompany).ToList()[0].UserId // sbp.pxl.company3@gmail.com
            };
            Console.WriteLine($" - {company3.Name}");
            companiesToAdd.Add(company3);

            // New registered company
            var company4 = new Company
            {
                Activated = false,
                BusNr = "1",
                City = "Lanaken",
                Country = "Belgium",
                Email = "info@company4.be",
                HouseNr = "24",
                Name = "Company4",
                PhoneNumber = "+3211123456",
                Street = "Steenweg",
                TotalEmployees = 100,
                TotalITEmployees = 50,
                TotalITEmployeesActive = 10,
                VATNumber = "987.1234.987",
                ZipCode = "3621",
                UserId = usersInDatabase.Where(user => user.RoleId == roleIdForCompany).ToList()[1].UserId // sbp.pxl.company4@gmail.com
            };
            Console.WriteLine($" - {company4.Name}");
            companiesToAdd.Add(company4);

            // Actived company
            var cegeka = new Company
            {
                Activated = true,
                BusNr = "1",
                City = "Hasselt",
                Country = "Belgium",
                Email = "info@cegeka.be",
                HouseNr = "9",
                Name = "Cegeka",
                PhoneNumber = "+3211020304",
                Street = "Universiteitslaan",
                TotalEmployees = 750,
                TotalITEmployees = 600,
                TotalITEmployeesActive = 550,
                VATNumber = "123.4565.789",
                ZipCode = "3500",
                UserId = usersInDatabase.Where(user => user.RoleId == roleIdForCompany).ToList()[2].UserId // jan.janssen@cegeka.com
            };
            Console.WriteLine($" - {cegeka.Name}");
            companiesToAdd.Add(cegeka);

            // Activated company
            var jarchitects = new Company
            {
                Activated = true,
                BusNr = "2",
                City = "Beringen",
                Country = "Belgium",
                Email = "info@jarchitects.be",
                HouseNr = "20",
                Name = "Jarchitects",
                PhoneNumber = "+3211123456",
                Street = "Beverlosesteenweg",
                TotalEmployees = 100,
                TotalITEmployees = 50,
                TotalITEmployeesActive = 10,
                VATNumber = "987.6543.210",
                ZipCode = "3583",
                UserId = usersInDatabase.Where(user => user.RoleId == roleIdForCompany).ToList()[3].UserId // email.ramaeckers@jarchitects.com
            };
            Console.WriteLine($" - {jarchitects.Name}");
            companiesToAdd.Add(jarchitects);

            // Activated company
            var zappware = new Company
            {
                Activated = true,
                BusNr = "1",
                City = "Hasselt",
                Country = "Belgium",
                Email = "info@zappware.be",
                HouseNr = "21",
                Name = "Zappware",
                PhoneNumber = "+3211123456",
                Street = "Ilgatlaan",
                TotalEmployees = 80,
                TotalITEmployees = 70,
                TotalITEmployeesActive = 60,
                VATNumber = "987.6543.210",
                ZipCode = "3500",
                UserId = usersInDatabase.Where(user => user.RoleId == roleIdForCompany).ToList()[4].UserId // lucas.grootveld@zappware.com
            };
            Console.WriteLine($" - {zappware.Name}");
            companiesToAdd.Add(zappware);

            // Activated company
            var happyVolcano = new Company
            {
                Activated = true,
                BusNr = "4",
                City = "Leuven",
                Country = "Belgium",
                Email = "info@happyvolcano.be",
                HouseNr = "35",
                Name = "Happy Volcano",
                PhoneNumber = "+3211123456",
                Street = "Engels Plein",
                TotalEmployees = 6,
                TotalITEmployees = 4,
                TotalITEmployeesActive = 4,
                VATNumber = "987.6543.210",
                ZipCode = "3000",
                UserId = usersInDatabase.Where(user => user.RoleId == roleIdForCompany).ToList()[5].UserId // harry.bornvoets@happyvoc.com
            };
            Console.WriteLine($" - {happyVolcano.Name}");
            companiesToAdd.Add(happyVolcano);

            // Activated company
            var abInbev = new Company
            {
                Activated = true,
                BusNr = "1",
                City = "Leuven",
                Country = "Belgium",
                Email = "info@abinbev.be",
                HouseNr = "1",
                Name = "AB Inbev",
                PhoneNumber = "+3211123456",
                Street = "Brouwerijplein",
                TotalEmployees = 50000,
                TotalITEmployees = 500,
                TotalITEmployeesActive = 430,
                VATNumber = "987.6543.210",
                ZipCode = "3000",
                UserId = usersInDatabase.Where(user => user.RoleId == roleIdForCompany).ToList()[6].UserId // sergio.slissers@abinbev.com
            };
            Console.WriteLine($" - {abInbev.Name}");
            companiesToAdd.Add(abInbev);

            // Activated company
            var cronos = new Company
            {
                Activated = true,
                BusNr = "B4",
                City = "Leuven",
                Country = "Belgium",
                Email = "info@cronosgroup.be",
                HouseNr = "11",
                Name = "Cronos Group",
                PhoneNumber = "+3211123456",
                Street = "Gaston Geenslaan",
                TotalEmployees = 100,
                TotalITEmployees = 50,
                TotalITEmployeesActive = 10,
                VATNumber = "987.6543.210",
                ZipCode = "3000",
                UserId = usersInDatabase.Where(user => user.RoleId == roleIdForCompany).ToList()[8].UserId // hanne.put@cronosgroup.be
            };
            Console.WriteLine($" - {cronos.Name}");
            companiesToAdd.Add(cronos);

            // Activated company
            var threeIt = new Company
            {
                Activated = true,
                BusNr = "B",
                City = "Westerlo",
                Country = "Belgium",
                Email = "info@3it.be",
                HouseNr = "48",
                Name = "3It",
                PhoneNumber = "+3211123456",
                Street = "Nijverheidsstraat",
                TotalEmployees = 12,
                TotalITEmployees = 10,
                TotalITEmployeesActive = 10,
                VATNumber = "987.6543.210",
                ZipCode = "2260",
                UserId = usersInDatabase.Where(user => user.RoleId == roleIdForCompany).ToList()[9].UserId // sophie.steegmans@3it.be
            };
            Console.WriteLine($" - {threeIt.Name}");
            companiesToAdd.Add(threeIt);

            if (shouldOverwrite)
            {
                _RepositoryWrapper.Company.DeleteRange(companiesInDatabase);
                _RepositoryWrapper.Save();
            }
            else
            {
                // Look for Companies that are not yet in the database.
                companiesToAdd = companiesToAdd.Where(newCompany => !companiesInDatabase.Any(dbObject => newCompany.VATNumber == dbObject.VATNumber)).ToList();
            }

            _RepositoryWrapper.Company.CreateRange(companiesToAdd);
            _RepositoryWrapper.Save();
        }

        public void SeedContacts(bool shouldOverwrite = false)
        {
            Console.WriteLine("Seeding contacts");

            var companiesInDatabase = _RepositoryWrapper.Company.FindAll().ToList();
            if (companiesInDatabase == null) throw new Exception("Please add companies to database first");
            var contactsInDatabase = _RepositoryWrapper.Contact.FindAll().ToList();

            List<Contact> contactsToAdd = new List<Contact>();

            foreach (var company in companiesInDatabase)
            {
                var firstNewContact = new Contact
                {
                   Firstname = "Albert",
                   Surname = "Adelstein",
                   PhoneNumber = "+32476112233",
                   Email = $"sbp.pxl.contact@gmail.com",
                   Function = "Contact person",
                   CompanyId = company.CompanyId,
                   Activated = true
                };
                
                Console.WriteLine($" - {firstNewContact.Firstname} {firstNewContact.Surname} of company {company.Name}");
                contactsToAdd.Add(firstNewContact);

                var secondNewContact = new Contact
                {
                    Firstname = "Bert",
                    Surname = "Bertels",
                    PhoneNumber = "+32476112233",
                    Email = $"sbp.pxl.contact@gmail.com",
                    Function = "Company internee promotor",
                    CompanyId = company.CompanyId,
                    Activated = true
                };

                Console.WriteLine($" - {secondNewContact.Firstname} {secondNewContact.Surname} of company {company.Name}");
                contactsToAdd.Add(secondNewContact);
            }

            if (shouldOverwrite)
            {
                _RepositoryWrapper.Contact.DeleteRange(contactsInDatabase);
                _RepositoryWrapper.Save();
            }
            else
            {
                // Look for users that are not yet in the database.
                contactsToAdd = contactsToAdd.Where(newContact => !contactsInDatabase.Any(dbObject => newContact.Email == dbObject.Email)).ToList();
            }

            _RepositoryWrapper.Contact.CreateRange(contactsToAdd);
            _RepositoryWrapper.Save();
        }

        public void SeedInternships(bool shouldOverwrite = false)
        {
            Console.WriteLine("Seeding internships");

            var companiesInDatabase = _RepositoryWrapper.Company.FindAll().ToList();
            var contactsInDatabase = _RepositoryWrapper.Contact.FindAll().ToList();
            var statusInDatabase = _RepositoryWrapper.ProjectStatus.FindAll().ToList();
            var usersInDatabase = _RepositoryWrapper.User.FindAll().ToList();
            var rolesInDatabase = _RepositoryWrapper.Role.FindAll().ToList();
            var environmentsInDatabase = _RepositoryWrapper.Environment.FindAll().ToList();
            var periodsInDatabase = _RepositoryWrapper.Period.FindAll().ToList();
            var specialisationsInDatabase = _RepositoryWrapper.Specialisation.FindAll().ToList();
            var expectationsInDatabase = _RepositoryWrapper.Expectation.FindAll().ToList();

            if (companiesInDatabase == null) throw new Exception("Please add companies to database first");
            if (contactsInDatabase == null) throw new Exception("Please add contacts to database first");
            if (statusInDatabase == null) throw new Exception("Please add statusses to database first");
            if (usersInDatabase == null) throw new Exception("Please add users to database first");
            if (environmentsInDatabase == null) throw new Exception("Please add environments to database first");
            if (periodsInDatabase == null) throw new Exception("Please add periods to database first");
            if (specialisationsInDatabase == null) throw new Exception("Please add specialisations to database first");
            if (expectationsInDatabase == null) throw new Exception("Please add expectations to database first");

            var internshipsInDatabase = _RepositoryWrapper.Internship.FindAll().ToList();

            List<Internship> internshipsToAdd = new List<Internship>();

            // Company1: New internship assigned to coordinators
            internshipsToAdd.Add(new Internship
            {
                CompanyId = companiesInDatabase.ToList()[3].CompanyId,
                ContactPersonId = contactsInDatabase.First().ContactId,
                PromotorFirstname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Firstname : contactsInDatabase[0].Firstname,
                PromotorSurname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Surname : contactsInDatabase[0].Surname,
                PromotorFunction = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Function : contactsInDatabase[0].Function,
                PromotorEmail = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Email : contactsInDatabase[0].Email,
                ProjectStatusId = statusInDatabase.Where(x => x.Code == "NEW").First().ProjectStatusId,
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
                ResearchTopicTitle = "Onderzoek infrastructure as a code",
                ResearchTopicDescription = "Wat zijn best practices van Infrastructure as a code binnen Azure? Hoe pakt VanRoey.be dit het best aan?",
                TotalInternsRequired = 1,
                ContactStudentName = null,
                AcademicYear = "2020-2021",
                Remark = "Indien mogelijk zouden we met de aanvang van de stage flexibel willen omspringen, " +
                 "zodat een andere stagair op hetzelfde tijdstip zou kunnen beginnen. " +
                 "In duo werken biedt, uit onze ervaring, veel voordelen. Zowel voor de student als ons bedrijf.",
                CreatedAt = new DateTime(2020, 1, 1),
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "SYS").First().EnvironmentId
                    },
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "EICT").First().EnvironmentId
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S2").First().PeriodId
                    }
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "SN").First().SpecialisationId
                    },
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "EICT").First().SpecialisationId
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "JI").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "RE").First().ExpectationId
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {
                    new InternshipAssignedUser
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "COO").First().RoleId).ToList()[0].UserId
                    },
                    new InternshipAssignedUser
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "COO").First().RoleId).ToList()[1].UserId
                    }
                },
                InternshipReviewer = new List<InternshipReviewer>()
            });

            // Company1: New internship (2021-2022) assigned to coordinators
            internshipsToAdd.Add(new Internship
            {
                CompanyId = companiesInDatabase.ToList()[3].CompanyId,
                ContactPersonId = contactsInDatabase.First().ContactId,
                PromotorFirstname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Firstname : contactsInDatabase[0].Firstname,
                PromotorSurname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Surname : contactsInDatabase[0].Surname,
                PromotorFunction = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Function : contactsInDatabase[0].Function,
                PromotorEmail = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Email : contactsInDatabase[0].Email,
                ProjectStatusId = statusInDatabase.Where(x => x.Code == "NEW").First().ProjectStatusId,
                WpStreet = "Bakerstraat",
                WpHouseNr = "1",
                WpBusNr = "1",
                WpCity = "Hasselt",
                WpZipCode = "3500",
                WpCountry = "Belgium",
                AssignmentDescription = "Stageopdracht om studenten van de toekomst volgend schooljaar een optimale begeleiding te geven.",
                TechnicalDetails = "Angular",
                Conditions = "Kennis van scripting.",
                ResearchTopicTitle = "Onderzoek future in code",
                ResearchTopicDescription = "Wat zijn best practices van Infrastructure as a code binnen Azure? Hoe pakt VanRoey.be dit het best aan?",
                TotalInternsRequired = 1,
                ContactStudentName = null,
                AcademicYear = "2021-2022",
                Remark = "Indien mogelijk zouden we met de aanvang van de stage flexibel willen omspringen, " +
                 "zodat een andere stagair op hetzelfde tijdstip zou kunnen beginnen. " +
                 "In duo werken biedt, uit onze ervaring, veel voordelen. Zowel voor de student als ons bedrijf.",
                CreatedAt = new DateTime(2021, 7, 1),
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "SYS").First().EnvironmentId
                    },
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "EICT").First().EnvironmentId
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S2").First().PeriodId
                    }
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "SN").First().SpecialisationId
                    },
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "EICT").First().SpecialisationId
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "JI").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "RE").First().ExpectationId
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {
                    new InternshipAssignedUser
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "COO").First().RoleId).ToList()[0].UserId
                    },
                    new InternshipAssignedUser
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "COO").First().RoleId).ToList()[1].UserId
                    }
                },
                InternshipReviewer = new List<InternshipReviewer>()
            });

            // Company2: New internship assigned to coordinators
            internshipsToAdd.Add(new Internship
            {
                CompanyId = companiesInDatabase.ToList()[2].CompanyId,
                ContactPersonId = contactsInDatabase.ToList()[2].ContactId,
                PromotorFirstname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Firstname : contactsInDatabase[0].Firstname,
                PromotorSurname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Surname : contactsInDatabase[0].Surname,
                PromotorFunction = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Function : contactsInDatabase[0].Function,
                PromotorEmail = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Email : contactsInDatabase[0].Email,
                ProjectStatusId = statusInDatabase.Where(x => x.Code == "NEW").First().ProjectStatusId,
                WpStreet = "Kempische Steenweg",
                WpHouseNr = "293",
                WpBusNr = "A",
                WpCity = "Hasselt",
                WpZipCode = "3500",
                WpCountry = "Belgium",
                AssignmentDescription = "iBeauty maakt saas software voor schoonheidssalons. Ons hoofdproduct is zo goed als klaar, " +
                "is live, en wordt reeds beheerd intern. We hebben echter enkele nieuwe apps die we willen koppelen. " +
                "Eentje daarvan is een native app (verschillende modules, vooral de agenda, in een app gieten).",
                TechnicalDetails = "PHP Framework: CodeIgniter of Laravel",
                Conditions = "Kennis van scripting.",
                ResearchTopicTitle = "Verbeteren bedrijfsproces (PHP)",
                ResearchTopicDescription = "Via onze software is het de bedoeling van gedrag van cosmetica-verdelers en " +
                "consumenten in kaart te brengen, en zo inzichten te kunnen vergaren om bedrijfsprocessen te verbeteren.~" +
                "Tweede onderzoeksopdracht nog te bepalen.",
                TotalInternsRequired = 2,
                ContactStudentName = null,
                AcademicYear = "2020-2021",
                Remark = "Indien mogelijk zouden we met de aanvang van de stage flexibel willen omspringen, " +
                 "zodat een andere stagair op hetzelfde tijdstip zou kunnen beginnen. " +
                 "In duo werken biedt, uit onze ervaring, veel voordelen. Zowel voor de student als ons bedrijf.",
                CreatedAt = new DateTime(2020, 1, 1),
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "WEB").First().EnvironmentId
                    },
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "MOB").First().EnvironmentId
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S1").First().PeriodId
                    },
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S2").First().PeriodId
                    }
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "AO").First().SpecialisationId
                    },
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "SN").First().SpecialisationId
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "JI").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "RE").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "TC").First().ExpectationId
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {
                    new InternshipAssignedUser
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "COO").First().RoleId).ToList()[0].UserId
                    },
                    new InternshipAssignedUser
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "COO").First().RoleId).ToList()[1].UserId
                    }
                },
                InternshipReviewer = new List<InternshipReviewer>()
            });

            // Company1: Internship from coordinator to 2 reviewers
            internshipsToAdd.Add(new Internship
            {
                CompanyId = companiesInDatabase.ToList()[3].CompanyId,
                ContactPersonId = contactsInDatabase.First().ContactId,
                PromotorFirstname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Firstname : contactsInDatabase[0].Firstname,
                PromotorSurname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Surname : contactsInDatabase[0].Surname,
                PromotorFunction = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Function : contactsInDatabase[0].Function,
                PromotorEmail = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Email : contactsInDatabase[0].Email,
                ProjectStatusId = statusInDatabase.Where(x => x.Code == "REV").First().ProjectStatusId,
                WpStreet = "Berkstraat",
                WpHouseNr = "1",
                WpBusNr = "1",
                WpCity = "Hasselt",
                WpZipCode = "3500",
                WpCountry = "Belgium",
                AssignmentDescription = "Cagaké zoekt studenten die hun talent willen bijdragen aan één van onze SaaS projecten. ",
                TechnicalDetails = "Java, C#",
                Conditions = null,
                ResearchTopicTitle = "Onderzoek .Net Core 3.1",
                ResearchTopicDescription = ".Net Core 3.1 heeft nieuwe functionaliteiten waarvan wij gebruik willen maken.",
                TotalInternsRequired = 1,
                ContactStudentName = null,
                AcademicYear = "2020-2021",
                Remark = null,
                CreatedAt = new DateTime(2020, 1, 2),
                InternalFeedback = "[{\"MessageDT\":\"20200502110943\",\"MessageBody\":\"Kunnen jullie dit even nakijken?\",\"UserFrom\":5,\"UserFromName\":\"Marijke Willems\",\"UserTo\":2,\"UserToName\":\"Jan Janssen\"}]",
                CountTotalAssignedReviewers = 2,
                SentToReviewersAt = DateTime.Now,
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "SYS").First().EnvironmentId
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S2").First().PeriodId
                    },
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S1").First().PeriodId
                    },
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "SN").First().SpecialisationId
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "JI").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "RE").First().ExpectationId
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {
                    new InternshipAssignedUser
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "REV").First().RoleId).ToList()[0].UserId
                    },
                    new InternshipAssignedUser
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "REV").First().RoleId).ToList()[1].UserId
                    }
                },
                InternshipReviewer = new List<InternshipReviewer>
                {
                    new InternshipReviewer
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "REV").First().RoleId).ToList()[0].UserId
                    },
                    new InternshipReviewer
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "REV").First().RoleId).ToList()[1].UserId
                    }
                }
            });

            // Company2: Internship from coordinator to 1 reviewer
            internshipsToAdd.Add(new Internship
            {
                CompanyId = companiesInDatabase.ToList()[2].CompanyId,
                ContactPersonId = contactsInDatabase.ToList()[3].ContactId,
                PromotorFirstname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Firstname : contactsInDatabase[0].Firstname,
                PromotorSurname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Surname : contactsInDatabase[0].Surname,
                PromotorFunction = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Function : contactsInDatabase[0].Function,
                PromotorEmail = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Email : contactsInDatabase[0].Email,
                ProjectStatusId = statusInDatabase.Where(x => x.Code == "REV").First().ProjectStatusId,
                WpStreet = "Berkstraat",
                WpHouseNr = "1",
                WpBusNr = "1",
                WpCity = "Hasselt",
                WpZipCode = "3500",
                WpCountry = "Belgium",
                AssignmentDescription = "Cagaké zoekt studenten die hun talent willen bijdragen aan één van onze SaaS projecten. ",
                TechnicalDetails = "Java, C#",
                Conditions = null,
                ResearchTopicTitle = ".Net Core: functionaliteiten",
                ResearchTopicDescription = ".Net Core 3.1 heeft nieuwe functionaliteiten waarvan wij gebruik willen maken.",
                TotalInternsRequired = 1,
                ContactStudentName = null,
                AcademicYear = "2020-2021",
                Remark = null,
                CreatedAt = new DateTime(2020, 1, 2),
                InternalFeedback = "[{\"MessageDT\":\"20200502110943\",\"MessageBody\":\"Is deze stageaanvraag oké voor jou?\",\"UserFrom\":5,\"UserFromName\":\"Marijke Willems\",\"UserTo\":2,\"UserToName\":\"Jan Janssen\"}]",
                CountTotalAssignedReviewers = 2,
                SentToReviewersAt = DateTime.Now,
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "SYS").First().EnvironmentId
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S2").First().PeriodId
                    },
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S1").First().PeriodId
                    },
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "SN").First().SpecialisationId
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "JI").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "RE").First().ExpectationId
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {
                    new InternshipAssignedUser
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "REV").First().RoleId).ToList()[0].UserId
                    }
                },
                InternshipReviewer = new List<InternshipReviewer>
                {
                    new InternshipReviewer
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "REV").First().RoleId).ToList()[0].UserId
                    }
                }
            });

            // Company1: modification asked for internship
            internshipsToAdd.Add(new Internship
            {
                CompanyId = companiesInDatabase.ToList()[3].CompanyId,
                ContactPersonId = contactsInDatabase.First().ContactId,
                PromotorFirstname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Firstname : contactsInDatabase[0].Firstname,
                PromotorSurname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Surname : contactsInDatabase[0].Surname,
                PromotorFunction = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Function : contactsInDatabase[0].Function,
                PromotorEmail = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Email : contactsInDatabase[0].Email,
                ProjectStatusId = statusInDatabase.Where(x => x.Code == "FEE").First().ProjectStatusId,
                WpStreet = "Europalaan",
                WpHouseNr = "10",
                WpBusNr = "B",
                WpCity = "Dilsen-Stokkem",
                WpZipCode = "3650",
                WpCountry = "Belgium",
                AssignmentDescription = "Tijdens jouw stage focus jij je op Infrastructure as code (IaC) binnen Azure." +
                 "Dit is een softwarematige benadering van de IT-infrastructuur, waarbij door middel van templates de systemen " +
                 "op een consistente manier uitgerold en aangepast kunnen worden. Als er een wijziging moet plaatsvinden, " +
                 "wordt deze doorgevoerd in het template die vervolgens weer wordt uitgerold. ",
                TechnicalDetails = "Python",
                Conditions = "Kennis van scripting.",
                ResearchTopicTitle = "Onderzoek Azure",
                ResearchTopicDescription = "Wat zijn best practices van Infrastructure as a code binnen Azure? Hoe pakt VanRoey.be dit het best aan?",
                TotalInternsRequired = 1,
                ContactStudentName = null,
                AcademicYear = "2020-2021",
                Remark = "Indien mogelijk zouden we met de aanvang van de stage flexibel willen omspringen, " +
                 "zodat een andere stagair op hetzelfde tijdstip zou kunnen beginnen. " +
                 "In duo werken biedt, uit onze ervaring, veel voordelen. Zowel voor de student als ons bedrijf.",
                CreatedAt = new DateTime(2020, 1, 1),
                ExternalFeedback = "[{\"MessageDT\":\"20201104091543\",\"MessageBody\":\"Gelieve de beschrijving van de stage meer in detail te formuleren.\",\"UserFrom\":4,\"UserFromName\":\"Bart Stukken\",\"UserTo\":0,\"UserToName\":\"\"}]",
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "EICT").First().EnvironmentId
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S2").First().PeriodId
                    }
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "EICT").First().SpecialisationId
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "JI").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "RE").First().ExpectationId
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {

                },
                InternshipReviewer = new List<InternshipReviewer>()
            });

            // Company2: modification asked for internship
            internshipsToAdd.Add(new Internship
            {
                CompanyId = companiesInDatabase.ToList()[2].CompanyId,
                ContactPersonId = contactsInDatabase.ToList()[3].ContactId,
                PromotorFirstname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Firstname : contactsInDatabase[0].Firstname,
                PromotorSurname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Surname : contactsInDatabase[0].Surname,
                PromotorFunction = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Function : contactsInDatabase[0].Function,
                PromotorEmail = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Email : contactsInDatabase[0].Email,
                ProjectStatusId = statusInDatabase.Where(x => x.Code == "FEE").First().ProjectStatusId,
                WpStreet = "Europalaan",
                WpHouseNr = "10",
                WpBusNr = "B",
                WpCity = "Dilsen-Stokkem",
                WpZipCode = "3650",
                WpCountry = "Belgium",
                AssignmentDescription = "Tijdens jouw stage focus jij je op Infrastructure as code (IaC) binnen Azure." +
                 "Dit is een softwarematige benadering van de IT-infrastructuur, waarbij door middel van templates de systemen " +
                 "op een consistente manier uitgerold en aangepast kunnen worden. Als er een wijziging moet plaatsvinden, " +
                 "wordt deze doorgevoerd in het template die vervolgens weer wordt uitgerold. ",
                TechnicalDetails = "Python",
                Conditions = "Kennis van scripting.",
                ResearchTopicTitle = "VanRoey.be - infrastructure",
                ResearchTopicDescription = "Wat zijn best practices van Infrastructure as a code binnen Azure? Hoe pakt VanRoey.be dit het best aan?",
                TotalInternsRequired = 1,
                ContactStudentName = null,
                AcademicYear = "2020-2021",
                Remark = "Indien mogelijk zouden we met de aanvang van de stage flexibel willen omspringen, " +
                 "zodat een andere stagair op hetzelfde tijdstip zou kunnen beginnen. " +
                 "In duo werken biedt, uit onze ervaring, veel voordelen. Zowel voor de student als ons bedrijf.",
                CreatedAt = new DateTime(2020, 1, 1),
                ExternalFeedback = "[{\"MessageDT\":\"20201104091543\",\"MessageBody\":\"Gelieve de beschrijving van de stage meer in detail te formuleren.\",\"UserFrom\":4,\"UserFromName\":\"Bart Stukken\",\"UserTo\":0,\"UserToName\":\"\"}]",
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "SYS").First().EnvironmentId
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S2").First().PeriodId
                    }
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "SN").First().SpecialisationId
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "JI").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "RE").First().ExpectationId
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {

                },
                InternshipReviewer = new List<InternshipReviewer>()
            });

            // Company1: internship evaluated by reviewer
            internshipsToAdd.Add(new Internship
            {
                CompanyId = companiesInDatabase.ToList()[3].CompanyId,
                ContactPersonId = contactsInDatabase.First().ContactId,
                PromotorFirstname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Firstname : contactsInDatabase[0].Firstname,
                PromotorSurname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Surname : contactsInDatabase[0].Surname,
                PromotorFunction = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Function : contactsInDatabase[0].Function,
                PromotorEmail = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Email : contactsInDatabase[0].Email,
                ProjectStatusId = statusInDatabase.Where(x => x.Code == "REV").First().ProjectStatusId,
                WpStreet = "Berkstraat",
                WpHouseNr = "1",
                WpBusNr = "1",
                WpCity = "Hasselt",
                WpZipCode = "3500",
                WpCountry = "Belgium",
                AssignmentDescription = "Cagaké zoekt studenten die hun talent willen bijdragen aan één van onze SaaS projecten. ",
                TechnicalDetails = "Java, C#",
                Conditions = null,
                ResearchTopicTitle = "Online deployment",
                ResearchTopicDescription = ".Net Core 3.1 heeft nieuwe functionaliteiten waarvan wij gebruik willen maken.",
                TotalInternsRequired = 1,
                ContactStudentName = null,
                AcademicYear = "2020-2021",
                Remark = null,
                CreatedAt = new DateTime(2020, 1, 2),
                InternalFeedback = "[{\"MessageDT\":\"20200502110943\",\"MessageBody\":\"Is deze stageaanvraag oké voor jou ?\",\"UserFrom\":4,\"UserFromName\":\"Bart Stukken\",\"UserTo\":2,\"UserToName\":\"Jan Janssen\"}," +
                "{\"MessageDT\":\"20201104110001\",\"MessageBody\":\"Prima!\",\"UserFrom\":3,\"UserFromName\":\"Jan Janssen\",\"UserTo\":5,\"UserToName\":\"Bart Stukken\"}]",
                CountTotalAssignedReviewers = 2,
                SentToReviewersAt = DateTime.Now,
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "EICT").First().EnvironmentId
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S2").First().PeriodId
                    },
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S1").First().PeriodId
                    },
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "EICT").First().SpecialisationId
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "JI").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "RE").First().ExpectationId
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {
                    new InternshipAssignedUser
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "COO").First().RoleId).ToList()[0].UserId
                    }
                },
                InternshipReviewer = new List<InternshipReviewer>
                {
                    new InternshipReviewer
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "REV").First().RoleId).ToList()[0].UserId
                    }
                }
            });

            // Company1: Approved internship with 1 student attached to favourites and 2 student attached to userinternships (applied - hire requested)
            internshipsToAdd.Add(new Internship
            {
                CompanyId = companiesInDatabase.ToList()[3].CompanyId,
                ContactPersonId = contactsInDatabase.First().ContactId,
                PromotorFirstname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Firstname : contactsInDatabase[0].Firstname,
                PromotorSurname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Surname : contactsInDatabase[0].Surname,
                PromotorFunction = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Function : contactsInDatabase[0].Function,
                PromotorEmail = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Email : contactsInDatabase[0].Email,
                ProjectStatusId = statusInDatabase.Where(x => x.Code == "APP").First().ProjectStatusId,
                WpStreet = "Europalaan",
                WpHouseNr = "25",
                WpBusNr = "1",
                WpCity = "Dilsen-Stokkem",
                WpZipCode = "3650",
                WpCountry = "Belgium",
                AssignmentDescription = "Wij zoeken studenten die hun talent willen bijdragen aan één van onze projecten. ",
                TechnicalDetails = "Java, C#",
                Conditions = null,
                ResearchTopicTitle = "Vergelijking Java/C#",
                ResearchTopicDescription = ".Net Core 3.1 heeft nieuwe functionaliteiten waarvan wij gebruik willen maken.",
                TotalInternsRequired = 1,
                ContactStudentName = null,
                AcademicYear = "2020-2021",
                Remark = null,
                CreatedAt = new DateTime(2020, 1, 2),
                ExternalFeedback = "[{\"MessageDT\":\"20201104091543\",\"MessageBody\":\"Gelieve de beschrijving van de stage meer in detail te formuleren.\",\"UserFrom\":4,\"UserFromName\":\"Bart Stukken\",\"UserTo\":0,\"UserToName\":\"\"}]",
                InternalFeedback = "[{\"MessageDT\":\"20200502110943\",\"MessageBody\":\"Kunnen jullie dit even nakijken?\",\"UserFrom\":5,\"UserFromName\":\"Marijke Willems\",\"UserTo\":2,\"UserToName\":\"Jan Janssen\"}]",
                CountTotalAssignedReviewers = 2,
                SentToReviewersAt = DateTime.Now,
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "SYS").First().EnvironmentId
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S1").First().PeriodId
                    },
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "SN").First().SpecialisationId
                    },
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "EICT").First().SpecialisationId
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "JI").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "RE").First().ExpectationId
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {

                },
                InternshipReviewer = new List<InternshipReviewer>
                {
                    new InternshipReviewer
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "REV").First().RoleId).ToList()[0].UserId
                    },
                    new InternshipReviewer
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "REV").First().RoleId).ToList()[1].UserId
                    }
                },
                UserFavourites = new List<UserFavourites>
                {
                    new UserFavourites
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").ToList()[0].RoleId).ToList()[1].UserId
                    }
                },
                UserInternships = new List<UserInternships>
                {
                    new UserInternships
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId
                    },
                    new UserInternships
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[1].UserId,
                        HireRequested = true
                    }
                }
            });

            // Company1: Approved internship (2021-2022) with 1 student attached to favourites and 2 student attached to userinternships (applied - hire requested)
            internshipsToAdd.Add(new Internship
            {
                CompanyId = companiesInDatabase.ToList()[3].CompanyId,
                ContactPersonId = contactsInDatabase.First().ContactId,
                PromotorFirstname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Firstname : contactsInDatabase[0].Firstname,
                PromotorSurname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Surname : contactsInDatabase[0].Surname,
                PromotorFunction = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Function : contactsInDatabase[0].Function,
                PromotorEmail = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Email : contactsInDatabase[0].Email,
                ProjectStatusId = statusInDatabase.Where(x => x.Code == "APP").First().ProjectStatusId,
                WpStreet = "Europalaan",
                WpHouseNr = "25",
                WpBusNr = "1",
                WpCity = "Dilsen-Stokkem",
                WpZipCode = "3650",
                WpCountry = "Belgium",
                AssignmentDescription = "Wij zoeken studenten die hun talent willen bijdragen aan één van onze projecten. ",
                TechnicalDetails = "Java, C#",
                Conditions = null,
                ResearchTopicTitle = "Back to the past",
                ResearchTopicDescription = ".Net Core 3.1 heeft nieuwe functionaliteiten waarvan wij gebruik willen maken.",
                TotalInternsRequired = 1,
                ContactStudentName = null,
                AcademicYear = "2021-2022",
                Remark = null,
                CreatedAt = new DateTime(2020, 7, 2),
                ExternalFeedback = "[{\"MessageDT\":\"20201104091543\",\"MessageBody\":\"Gelieve de beschrijving van de stage meer in detail te formuleren.\",\"UserFrom\":4,\"UserFromName\":\"Bart Stukken\",\"UserTo\":0,\"UserToName\":\"\"}]",
                InternalFeedback = "[{\"MessageDT\":\"20200502110943\",\"MessageBody\":\"Kunnen jullie dit even nakijken?\",\"UserFrom\":5,\"UserFromName\":\"Marijke Willems\",\"UserTo\":2,\"UserToName\":\"Jan Janssen\"}]",
                CountTotalAssignedReviewers = 2,
                SentToReviewersAt = DateTime.Now,
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "SYS").First().EnvironmentId
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S2").First().PeriodId
                    },
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S1").First().PeriodId
                    },
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "SN").First().SpecialisationId
                    },
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "EICT").First().SpecialisationId
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "JI").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "RE").First().ExpectationId
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {

                },
                InternshipReviewer = new List<InternshipReviewer>
                {
                    new InternshipReviewer
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "REV").First().RoleId).ToList()[0].UserId
                    },
                    new InternshipReviewer
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "REV").First().RoleId).ToList()[1].UserId
                    }
                },
                UserFavourites = new List<UserFavourites>
                {
                },
                UserInternships = new List<UserInternships>
                {
                }
            });

            // Company2: Approved internship with no student attached to favourites and 1 student attached to userinternships (applied - hire requested nadconfirmed)
            internshipsToAdd.Add(new Internship
            {
                CompanyId = companiesInDatabase.ToList()[2].CompanyId,
                ContactPersonId = contactsInDatabase.ToList()[2].ContactId,
                PromotorFirstname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Firstname : contactsInDatabase[0].Firstname,
                PromotorSurname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Surname : contactsInDatabase[0].Surname,
                PromotorFunction = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Function : contactsInDatabase[0].Function,
                PromotorEmail = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Email : contactsInDatabase[0].Email,
                ProjectStatusId = statusInDatabase.Where(x => x.Code == "APP").First().ProjectStatusId,
                WpStreet = "Dorpsstraat",
                WpHouseNr = "1",
                WpBusNr = "1",
                WpCity = "Genk",
                WpZipCode = "3600",
                WpCountry = "Belgium",
                AssignmentDescription = "Wij zoeken studenten die hun talent willen bijdragen aan één van onze projecten. ",
                TechnicalDetails = "Java, C#",
                Conditions = null,
                ResearchTopicTitle = ".Net Core 2.9",
                ResearchTopicDescription = ".Net Core 3.1 heeft nieuwe functionaliteiten waarvan wij gebruik willen maken.",
                TotalInternsRequired = 1,
                ContactStudentName = null,
                AcademicYear = "2020-2021",
                Remark = null,
                CreatedAt = new DateTime(2020, 1, 2),
                InternalFeedback = "[{\"MessageDT\":\"20200502110943\",\"MessageBody\":\"Kunnen jullie dit even nakijken?\",\"UserFrom\":4,\"UserFromName\":\"Marijke Willems\",\"UserTo\":2,\"UserToName\":\"Jan Janssen\"}]",
                CountTotalAssignedReviewers = 2,
                SentToReviewersAt = DateTime.Now,
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "SYS").First().EnvironmentId
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S2").First().PeriodId
                    },
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S1").First().PeriodId
                    },
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "SN").First().SpecialisationId
                    },
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "EICT").First().SpecialisationId
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "JI").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "RE").First().ExpectationId
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {

                },
                InternshipReviewer = new List<InternshipReviewer>
                {
                    new InternshipReviewer
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "REV").First().RoleId).ToList()[0].UserId
                    }
                },
                UserFavourites = new List<UserFavourites>
                {
                    new UserFavourites
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId
                    }
                },
                UserInternships = new List<UserInternships>
                {
                    new UserInternships
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[1].UserId,
                        HireRequested = true,
                        HireConfirmed = true
                    }
                }
            });

            // Company1: Approved internship with no student attached to favourites and 1 student attached to userinternships (applied - hire requested nadconfirmed)
            internshipsToAdd.Add(new Internship
            {
                CompanyId = companiesInDatabase.ToList()[3].CompanyId,
                ContactPersonId = contactsInDatabase.First().ContactId,
                PromotorFirstname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Firstname : contactsInDatabase[0].Firstname,
                PromotorSurname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Surname : contactsInDatabase[0].Surname,
                PromotorFunction = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Function : contactsInDatabase[0].Function,
                PromotorEmail = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Email : contactsInDatabase[0].Email,
                ProjectStatusId = statusInDatabase.Where(x => x.Code == "APP").First().ProjectStatusId,
                WpStreet = "Dorpsstraat",
                WpHouseNr = "1",
                WpBusNr = "1",
                WpCity = "Genk",
                WpZipCode = "3600",
                WpCountry = "Belgium",
                AssignmentDescription = "Wij zoeken studenten die hun talent willen bijdragen aan één van onze projecten. ",
                TechnicalDetails = "Java, C#",
                Conditions = null,
                ResearchTopicTitle = "BettyBlocks",
                ResearchTopicDescription = "Applicatie naar keuze maken in het no-code platform BettyBlocks.",
                TotalInternsRequired = 1,
                ContactStudentName = null,
                AcademicYear = "2020-2021",
                Remark = null,
                CreatedAt = new DateTime(2020, 1, 2),
                InternalFeedback = "[{\"MessageDT\":\"20200502110943\",\"MessageBody\":\"Kunnen jullie dit even nakijken?\",\"UserFrom\":4,\"UserFromName\":\"Marijke Willems\",\"UserTo\":2,\"UserToName\":\"Jan Janssen\"}]",
                CountTotalAssignedReviewers = 1,
                SentToReviewersAt = DateTime.Now,
                Completed = true,
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "SYS").First().EnvironmentId
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S2").First().PeriodId
                    },
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S1").First().PeriodId
                    },
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "SN").First().SpecialisationId
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "JI").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "RE").First().ExpectationId
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {
                    new InternshipAssignedUser
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId
                    }
                },
                InternshipReviewer = new List<InternshipReviewer>
                {
                    new InternshipReviewer
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "REV").First().RoleId).ToList()[0].UserId
                    }
                },
                UserFavourites = new List<UserFavourites>
                {
                    new UserFavourites
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId
                    }
                },
                UserInternships = new List<UserInternships>
                {
                    new UserInternships
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId,
                        HireRequested = true,
                        HireConfirmed = true,
                        HireApproved = true,
                        EvaluatedAt = DateTime.Now
                    }
                },
                ShowInEvent = true
            });

            // Company1: Approved internship with no favourties and no applications
            internshipsToAdd.Add(new Internship
            {
                CompanyId = companiesInDatabase.ToList()[3].CompanyId,
                ContactPersonId = contactsInDatabase.First().ContactId,
                PromotorFirstname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Firstname : contactsInDatabase[0].Firstname,
                PromotorSurname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Surname : contactsInDatabase[0].Surname,
                PromotorFunction = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Function : contactsInDatabase[0].Function,
                PromotorEmail = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Email : contactsInDatabase[0].Email,
                ProjectStatusId = statusInDatabase.Where(x => x.Code == "APP").First().ProjectStatusId,
                WpStreet = "Europalaan",
                WpHouseNr = "25",
                WpBusNr = "1",
                WpCity = "Dilsen-Stokkem",
                WpZipCode = "3650",
                WpCountry = "Belgium",
                AssignmentDescription = "Wij zoeken studenten die hun talent willen bijdragen aan één van onze projecten. ",
                TechnicalDetails = "Angular, .NET Core",
                Conditions = null,
                ResearchTopicTitle = "Stagebeheerapplicatie PXL",
                ResearchTopicDescription = "Je maakt voor de PXL Hogeschool een stagebeheerapplicatie met behulp van Angular en .NET Core.~" +
                "Je zal deze applicatie generisch bouwen zodat deze voor elk PXL-departement werkt.",
                TotalInternsRequired = 2,
                ContactStudentName = "Maarten Warson",
                AcademicYear = "2020-2021",
                Remark = null,
                CreatedAt = new DateTime(2020, 1, 2),
                CountTotalAssignedReviewers = 1,
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "WEB").First().EnvironmentId
                    },
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "TES").First().EnvironmentId
                    },
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "SYS").First().EnvironmentId
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S2").First().PeriodId
                    },
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S1").First().PeriodId
                    },
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "AO").First().SpecialisationId
                    },
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "SN").First().SpecialisationId
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "JI").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "RE").First().ExpectationId
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {
                    new InternshipAssignedUser
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[2].UserId
                    }
                },
                InternshipReviewer = new List<InternshipReviewer>
                {
                },
                UserFavourites = new List<UserFavourites>
                {
                },
                UserInternships = new List<UserInternships>
                {
                    new UserInternships
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[2].UserId,
                        HireRequested = true,
                        HireConfirmed = true,
                        HireApproved = true,
                        EvaluatedAt = DateTime.Now,
                        Interesting = true
                    }
                }
            });

            // Company2: Approved internship with 1 favourite and no applications
            internshipsToAdd.Add(new Internship
            {
                CompanyId = companiesInDatabase.ToList()[2].CompanyId,
                ContactPersonId = contactsInDatabase.ToList()[2].ContactId,
                PromotorFirstname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Firstname : contactsInDatabase[0].Firstname,
                PromotorSurname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Surname : contactsInDatabase[0].Surname,
                PromotorFunction = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Function : contactsInDatabase[0].Function,
                PromotorEmail = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Email : contactsInDatabase[0].Email,
                ProjectStatusId = statusInDatabase.Where(x => x.Code == "APP").First().ProjectStatusId,
                WpStreet = "Europalaan",
                WpHouseNr = "25",
                WpBusNr = "1",
                WpCity = "Dilsen-Stokkem",
                WpZipCode = "3650",
                WpCountry = "Belgium",
                AssignmentDescription = "Wij zoeken studenten die hun talent willen bijdragen aan één van onze projecten. ",
                TechnicalDetails = "Angular, .NET Core",
                Conditions = null,
                ResearchTopicTitle = "Deeko",
                ResearchTopicDescription = "Je maakt een Smartschool-appliactie, maar dan voor het Deeltijds Kunstonderwijs.",
                TotalInternsRequired = 1,
                ContactStudentName = null,
                AcademicYear = "2020-2021",
                Remark = null,
                CreatedAt = new DateTime(2020, 1, 2),
                CountTotalAssignedReviewers = 1,
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "WEB").First().EnvironmentId
                    },
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "TES").First().EnvironmentId
                    },
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "SYS").First().EnvironmentId
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S2").First().PeriodId
                    }
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "AO").First().SpecialisationId
                    },
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "SN").First().SpecialisationId
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "JI").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "RE").First().ExpectationId
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {
                },
                InternshipReviewer = new List<InternshipReviewer>
                {
                },
                UserFavourites = new List<UserFavourites>
                {
                    new UserFavourites
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[1].UserId
                    }
                },
                UserInternships = new List<UserInternships>
                {
                    new UserInternships
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId,
                        HireRequested = true
                    }
                },
                ShowInEvent = true
            });

            // Company1: Rejected internship
            internshipsToAdd.Add(new Internship
            {
                CompanyId = companiesInDatabase.ToList()[3].CompanyId,
                ContactPersonId = contactsInDatabase.First().ContactId,
                PromotorFirstname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Firstname : contactsInDatabase[0].Firstname,
                PromotorSurname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Surname : contactsInDatabase[0].Surname,
                PromotorFunction = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Function : contactsInDatabase[0].Function,
                PromotorEmail = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Email : contactsInDatabase[0].Email,
                ProjectStatusId = statusInDatabase.Where(x => x.Code == "REJ").First().ProjectStatusId,
                WpStreet = "Europalaan",
                WpHouseNr = "25",
                WpBusNr = "1",
                WpCity = "Dilsen-Stokkem",
                WpZipCode = "3650",
                WpCountry = "Belgium",
                AssignmentDescription = "Wij zoeken studenten die hun talent willen bijdragen aan één van onze projecten. ",
                TechnicalDetails = "Angular, .NET Core",
                Conditions = null,
                ResearchTopicTitle = "Stagebeheerapplicatie UCLL",
                ResearchTopicDescription = "Je maakt voor de UCLL een stagebeheerapplicatie met behulp van Angular en .NET Core.",
                TotalInternsRequired = 1,
                ContactStudentName = null,
                AcademicYear = "2020-2021",
                Remark = null,
                CreatedAt = new DateTime(2020, 1, 2),
                CountTotalAssignedReviewers = 1,
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "WEB").First().EnvironmentId
                    },
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "TES").First().EnvironmentId
                    },
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "SYS").First().EnvironmentId
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S2").First().PeriodId
                    },
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S1").First().PeriodId
                    },
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "AO").First().SpecialisationId
                    },
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "SN").First().SpecialisationId
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "JI").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "RE").First().ExpectationId
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {
                },
                InternshipReviewer = new List<InternshipReviewer>
                {
                },
                UserFavourites = new List<UserFavourites>
                {
                },
                UserInternships = new List<UserInternships>
                {
                }
            });

            // Company2: Rejected internship
            internshipsToAdd.Add(new Internship
            {
                CompanyId = companiesInDatabase.ToList()[2].CompanyId,
                ContactPersonId = contactsInDatabase.ToList()[2].ContactId,
                PromotorFirstname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Firstname : contactsInDatabase[0].Firstname,
                PromotorSurname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Surname : contactsInDatabase[0].Surname,
                PromotorFunction = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Function : contactsInDatabase[0].Function,
                PromotorEmail = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Email : contactsInDatabase[0].Email,
                ProjectStatusId = statusInDatabase.Where(x => x.Code == "REJ").First().ProjectStatusId,
                WpStreet = "Europalaan",
                WpHouseNr = "25",
                WpBusNr = "1",
                WpCity = "Dilsen-Stokkem",
                WpZipCode = "3650",
                WpCountry = "Belgium",
                AssignmentDescription = "Wij zoeken studenten die hun talent willen bijdragen aan één van onze projecten. ",
                TechnicalDetails = "Angular, .NET Core",
                Conditions = null,
                ResearchTopicTitle = "Examen generator",
                ResearchTopicDescription = "Je maakt een examen generator voor KU Leuven.",
                TotalInternsRequired = 1,
                ContactStudentName = null,
                AcademicYear = "2020-2021",
                Remark = null,
                CreatedAt = new DateTime(2020, 1, 2),
                CountTotalAssignedReviewers = 1,
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "WEB").First().EnvironmentId
                    },
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "TES").First().EnvironmentId
                    },
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "SYS").First().EnvironmentId
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S2").First().PeriodId
                    }
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "AO").First().SpecialisationId
                    },
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "SN").First().SpecialisationId
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "JI").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "RE").First().ExpectationId
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {
                },
                InternshipReviewer = new List<InternshipReviewer>
                {
                },
                UserFavourites = new List<UserFavourites>
                {
                },
                UserInternships = new List<UserInternships>
                {
                }
            });

            // Cegeka: Approved internship with no student attached to favourites and 1 student attached to userinternships (applied - hire requested nadconfirmed)
            internshipsToAdd.Add(new Internship
            {
                CompanyId = companiesInDatabase.ToList()[6].CompanyId,
                ContactPersonId = contactsInDatabase.First().ContactId,
                PromotorFirstname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Firstname : contactsInDatabase[0].Firstname,
                PromotorSurname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Surname : contactsInDatabase[0].Surname,
                PromotorFunction = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Function : contactsInDatabase[0].Function,
                PromotorEmail = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Email : contactsInDatabase[0].Email,
                ProjectStatusId = statusInDatabase.Where(x => x.Code == "APP").First().ProjectStatusId,
                WpStreet = "Universiteitslaan",
                WpHouseNr = "9",
                WpBusNr = "1",
                WpCity = "Hasselt",
                WpZipCode = "3500",
                WpCountry = "Belgium",
                AssignmentDescription = "Voor een klant mag je als stagiair meehelpen met de bouw van een banking web applicatie.",
                TechnicalDetails = "React, NodeJS",
                Conditions = null,
                ResearchTopicTitle = "Banking Web App Authentication",
                ResearchTopicDescription = "Wat zou de beste benadering zijn voor veilige authenticatie in een banking web applicatie?",
                TotalInternsRequired = 1,
                ContactStudentName = null,
                AcademicYear = "2020-2021",
                Remark = null,
                CreatedAt = new DateTime(2020, 2, 3),
                InternalFeedback = null,
                CountTotalAssignedReviewers = 1,
                SentToReviewersAt = DateTime.Now,
                Completed = true,
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "WEB").First().EnvironmentId
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S2").First().PeriodId
                    },
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S1").First().PeriodId
                    },
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "AO").First().SpecialisationId
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "JI").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "RE").First().ExpectationId
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {
                    new InternshipAssignedUser
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId
                    }
                },
                InternshipReviewer = new List<InternshipReviewer>
                {
                    new InternshipReviewer
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "REV").First().RoleId).ToList()[0].UserId
                    }
                },
                UserFavourites = new List<UserFavourites>
                {
                    new UserFavourites
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId
                    }
                },
                UserInternships = new List<UserInternships>
                {
                    new UserInternships
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId,
                        HireRequested = true,
                        HireConfirmed = true,
                        HireApproved = true,
                        EvaluatedAt = DateTime.Now
                    }
                },
                ShowInEvent = true
            });

            // Jarchitects: Approved internship with no student attached to favourites and 1 student attached to userinternships (applied - hire requested nadconfirmed)
            internshipsToAdd.Add(new Internship
            {
                CompanyId = companiesInDatabase.ToList()[5].CompanyId,
                ContactPersonId = contactsInDatabase.First().ContactId,
                PromotorFirstname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Firstname : contactsInDatabase[0].Firstname,
                PromotorSurname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Surname : contactsInDatabase[0].Surname,
                PromotorFunction = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Function : contactsInDatabase[0].Function,
                PromotorEmail = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Email : contactsInDatabase[0].Email,
                ProjectStatusId = statusInDatabase.Where(x => x.Code == "APP").First().ProjectStatusId,
                WpStreet = "Beverlosesteenweg",
                WpHouseNr = "20",
                WpBusNr = "2",
                WpCity = "Beringen",
                WpZipCode = "3583",
                WpCountry = "Belgium",
                AssignmentDescription = "Heb je een passie voor Java? Dan is deze stage misschien wel DE plek voor jou!",
                TechnicalDetails = "Java, Junit",
                Conditions = null,
                ResearchTopicTitle = "Automated testing in Java",
                ResearchTopicDescription = "Automated testing is een wereld die snel evolueert. We willen graag een blik op de huidige beschikbare technieken die compatibel zijn met Java.",
                TotalInternsRequired = 2,
                ContactStudentName = null,
                AcademicYear = "2020-2021",
                Remark = null,
                CreatedAt = new DateTime(2020, 3, 3),
                InternalFeedback = null,
                CountTotalAssignedReviewers = 1,
                SentToReviewersAt = DateTime.Now,
                Completed = true,
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "TES").First().EnvironmentId
                    },
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "JAV").First().EnvironmentId
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S2").First().PeriodId
                    },
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S1").First().PeriodId
                    },
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "SN").First().SpecialisationId
                    },
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "AO").First().SpecialisationId
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "JI").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "RE").First().ExpectationId
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {
                    new InternshipAssignedUser
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId
                    }
                },
                InternshipReviewer = new List<InternshipReviewer>
                {
                    new InternshipReviewer
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "REV").First().RoleId).ToList()[0].UserId
                    }
                },
                UserFavourites = new List<UserFavourites>
                {
                    new UserFavourites
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId
                    }
                },
                UserInternships = new List<UserInternships>
                {
                    new UserInternships
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId,
                        HireRequested = true,
                        HireConfirmed = true,
                        HireApproved = true,
                        EvaluatedAt = DateTime.Now
                    }
                },
                ShowInEvent = true
            });

            // Zappware: Approved internship with no student attached to favourites and 1 student attached to userinternships (applied - hire requested nadconfirmed)
            internshipsToAdd.Add(new Internship
            {
                CompanyId = companiesInDatabase.ToList()[4].CompanyId,
                ContactPersonId = contactsInDatabase.First().ContactId,
                PromotorFirstname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Firstname : contactsInDatabase[0].Firstname,
                PromotorSurname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Surname : contactsInDatabase[0].Surname,
                PromotorFunction = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Function : contactsInDatabase[0].Function,
                PromotorEmail = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Email : contactsInDatabase[0].Email,
                ProjectStatusId = statusInDatabase.Where(x => x.Code == "APP").First().ProjectStatusId,
                WpStreet = "Ilgatlaan",
                WpHouseNr = "21",
                WpBusNr = "1",
                WpCity = "Hasselt",
                WpZipCode = "3500",
                WpCountry = "Belgium",
                AssignmentDescription = "We zijn op zoek naar een stagiair met een passie voor digitale TV en kennis van Linux. Sta je je mannetje in Python scripting, kom dan zeker langs.",
                TechnicalDetails = "Linux, Python",
                Conditions = null,
                ResearchTopicTitle = "Deployment automation",
                ResearchTopicDescription = "We willen graag onze deployment activiteiten automatiseren en zoeken hiervoor geschikte frameworks.",
                TotalInternsRequired = 1,
                ContactStudentName = null,
                AcademicYear = "2020-2021",
                Remark = null,
                CreatedAt = new DateTime(2020, 3, 6),
                InternalFeedback = null,
                CountTotalAssignedReviewers = 1,
                SentToReviewersAt = DateTime.Now,
                Completed = true,
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "SYS").First().EnvironmentId
                    },
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "JAV").First().EnvironmentId
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S2").First().PeriodId
                    },
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S1").First().PeriodId
                    },
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "SN").First().SpecialisationId
                    },
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "AO").First().SpecialisationId
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "JI").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "RE").First().ExpectationId
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {
                    new InternshipAssignedUser
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId
                    }
                },
                InternshipReviewer = new List<InternshipReviewer>
                {
                    new InternshipReviewer
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "REV").First().RoleId).ToList()[0].UserId
                    }
                },
                UserFavourites = new List<UserFavourites>
                {
                    new UserFavourites
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId
                    }
                },
                UserInternships = new List<UserInternships>
                {
                    new UserInternships
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId,
                        HireRequested = true,
                        HireConfirmed = true,
                        HireApproved = true,
                        EvaluatedAt = DateTime.Now
                    }
                },
                ShowInEvent = true
            });

            // Happy Volcano: Approved internship with no student attached to favourites and 1 student attached to userinternships (applied - hire requested nadconfirmed)
            internshipsToAdd.Add(new Internship
            {
                CompanyId = companiesInDatabase.ToList()[3].CompanyId,
                ContactPersonId = contactsInDatabase.First().ContactId,
                PromotorFirstname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Firstname : contactsInDatabase[0].Firstname,
                PromotorSurname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Surname : contactsInDatabase[0].Surname,
                PromotorFunction = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Function : contactsInDatabase[0].Function,
                PromotorEmail = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Email : contactsInDatabase[0].Email,
                ProjectStatusId = statusInDatabase.Where(x => x.Code == "APP").First().ProjectStatusId,
                WpStreet = "Engels Plein",
                WpHouseNr = "35",
                WpBusNr = "0101",
                WpCity = "Leuven",
                WpZipCode = "3000",
                WpCountry = "Belgium",
                AssignmentDescription = "We zijn op zoek naar een gemotiveerde stagiair met een passie voor games die ons als gameplay developer wil helpen om kick-ass games te ontwikkelen.",
                TechnicalDetails = "C#, Unity",
                Conditions = null,
                ResearchTopicTitle = "Monetization voor multiplayer games",
                ResearchTopicDescription = "We spelen met het idee om aan een bestaande game een monetization platform toe te voegen. Aan jou de vraag hoe we dit best aanpakken.",
                TotalInternsRequired = 1,
                ContactStudentName = null,
                AcademicYear = "2020-2021",
                Remark = null,
                CreatedAt = new DateTime(2020, 5, 16),
                InternalFeedback = null,
                CountTotalAssignedReviewers = 1,
                SentToReviewersAt = DateTime.Now,
                Completed = true,
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "DOT").First().EnvironmentId
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S2").First().PeriodId
                    },
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S1").First().PeriodId
                    },
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "AO").First().SpecialisationId
                    },
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "AI").First().SpecialisationId
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "JI").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "RE").First().ExpectationId
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {
                    new InternshipAssignedUser
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId
                    }
                },
                InternshipReviewer = new List<InternshipReviewer>
                {
                    new InternshipReviewer
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "REV").First().RoleId).ToList()[0].UserId
                    }
                },
                UserFavourites = new List<UserFavourites>
                {
                    new UserFavourites
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId
                    }
                },
                UserInternships = new List<UserInternships>
                {
                    new UserInternships
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId,
                        HireRequested = true,
                        HireConfirmed = true,
                        HireApproved = true,
                        EvaluatedAt = DateTime.Now
                    }
                },
                ShowInEvent = true
            });

            // AB Inbev: Approved internship with no student attached to favourites and 1 student attached to userinternships (applied - hire requested nadconfirmed)
            internshipsToAdd.Add(new Internship
            {
                CompanyId = companiesInDatabase.ToList()[2].CompanyId,
                ContactPersonId = contactsInDatabase.First().ContactId,
                PromotorFirstname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Firstname : contactsInDatabase[0].Firstname,
                PromotorSurname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Surname : contactsInDatabase[0].Surname,
                PromotorFunction = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Function : contactsInDatabase[0].Function,
                PromotorEmail = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Email : contactsInDatabase[0].Email,
                ProjectStatusId = statusInDatabase.Where(x => x.Code == "APP").First().ProjectStatusId,
                WpStreet = "Brouwerijplein",
                WpHouseNr = "1",
                WpBusNr = "1",
                WpCity = "Leuven",
                WpZipCode = "3000",
                WpCountry = "Belgium",
                AssignmentDescription = "Voor het uitrollen van een nieuwe webwinkel moet er backend voorbereidend werk gebeuren in Java.",
                TechnicalDetails = "Java",
                Conditions = null,
                ResearchTopicTitle = "Betalingsplatformen voor webwinkel",
                ResearchTopicDescription = "Uitzoeken welke betalingsplatformen voor onze webwinkel ondersteund moeten worden.",
                TotalInternsRequired = 2,
                ContactStudentName = null,
                AcademicYear = "2020-2021",
                Remark = null,
                CreatedAt = new DateTime(2020, 6, 13),
                InternalFeedback = null,
                CountTotalAssignedReviewers = 1,
                SentToReviewersAt = DateTime.Now,
                Completed = true,
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "JAV").First().EnvironmentId
                    },
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "WEB").First().EnvironmentId
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S2").First().PeriodId
                    },
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S1").First().PeriodId
                    },
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "AO").First().SpecialisationId
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "JI").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "RE").First().ExpectationId
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {
                    new InternshipAssignedUser
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId
                    }
                },
                InternshipReviewer = new List<InternshipReviewer>
                {
                    new InternshipReviewer
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "REV").First().RoleId).ToList()[0].UserId
                    }
                },
                UserFavourites = new List<UserFavourites>
                {
                    new UserFavourites
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId
                    }
                },
                UserInternships = new List<UserInternships>
                {
                    new UserInternships
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId,
                        HireRequested = true,
                        HireConfirmed = true,
                        HireApproved = true,
                        EvaluatedAt = DateTime.Now
                    }
                },
                ShowInEvent = true
            });

            // Cronos Group: Approved internship with no student attached to favourites and 1 student attached to userinternships (applied - hire requested nadconfirmed)
            internshipsToAdd.Add(new Internship
            {
                CompanyId = companiesInDatabase.ToList()[1].CompanyId,
                ContactPersonId = contactsInDatabase.First().ContactId,
                PromotorFirstname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Firstname : contactsInDatabase[0].Firstname,
                PromotorSurname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Surname : contactsInDatabase[0].Surname,
                PromotorFunction = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Function : contactsInDatabase[0].Function,
                PromotorEmail = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Email : contactsInDatabase[0].Email,
                ProjectStatusId = statusInDatabase.Where(x => x.Code == "APP").First().ProjectStatusId,
                WpStreet = "Gaston Geenslaan",
                WpHouseNr = "11",
                WpBusNr = "B4",
                WpCity = "Leuven",
                WpZipCode = "3000",
                WpCountry = "Belgium",
                AssignmentDescription = "We zoeken een bekwame en gepassioneerde developer bedreven in Java om ons team te versterken met een IT project in de zorgsector.",
                TechnicalDetails = "Java",
                Conditions = null,
                ResearchTopicTitle = "Java voor beeldverwerking",
                ResearchTopicDescription = "Voor een toepassing in de beeldverwerking moeten geschikte Java libraries uitgezocht worden.",
                TotalInternsRequired = 2,
                ContactStudentName = null,
                AcademicYear = "2020-2021",
                Remark = null,
                CreatedAt = new DateTime(2020, 8, 21),
                InternalFeedback = null,
                CountTotalAssignedReviewers = 1,
                SentToReviewersAt = DateTime.Now,
                Completed = true,
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "JAV").First().EnvironmentId
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S2").First().PeriodId
                    },
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S1").First().PeriodId
                    },
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "AO").First().SpecialisationId
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "JI").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "RE").First().ExpectationId
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {
                    new InternshipAssignedUser
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId
                    }
                },
                InternshipReviewer = new List<InternshipReviewer>
                {
                    new InternshipReviewer
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "REV").First().RoleId).ToList()[0].UserId
                    }
                },
                UserFavourites = new List<UserFavourites>
                {
                    new UserFavourites
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId
                    }
                },
                UserInternships = new List<UserInternships>
                {
                    new UserInternships
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId,
                        HireRequested = true,
                        HireConfirmed = true,
                        HireApproved = true,
                        EvaluatedAt = DateTime.Now
                    }
                },
                ShowInEvent = true
            });

            // 3It: Approved internship with no student attached to favourites and 1 student attached to userinternships (applied - hire requested nadconfirmed)
            internshipsToAdd.Add(new Internship
            {
                CompanyId = companiesInDatabase.ToList()[0].CompanyId,
                ContactPersonId = contactsInDatabase.First().ContactId,
                PromotorFirstname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Firstname : contactsInDatabase[0].Firstname,
                PromotorSurname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Surname : contactsInDatabase[0].Surname,
                PromotorFunction = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Function : contactsInDatabase[0].Function,
                PromotorEmail = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Email : contactsInDatabase[0].Email,
                ProjectStatusId = statusInDatabase.Where(x => x.Code == "APP").First().ProjectStatusId,
                WpStreet = "Nijverheidsstraat",
                WpHouseNr = "48",
                WpBusNr = "B",
                WpCity = "Westerlo",
                WpZipCode = "2260",
                WpCountry = "Belgium",
                AssignmentDescription = "In deze stage ben je verantwoordelijk voor de bouw van een website. Bakend draait in C#, frontend draait in Angular.",
                TechnicalDetails = "C#, Angular",
                Conditions = null,
                ResearchTopicTitle = "Angular vs Vue vs React",
                ResearchTopicDescription = "Voor- en nadelen onderzoeken tussen Angular, Vue en React.",
                TotalInternsRequired = 1,
                ContactStudentName = null,
                AcademicYear = "2020-2021",
                Remark = null,
                CreatedAt = new DateTime(2020, 9, 7),
                InternalFeedback = null,
                CountTotalAssignedReviewers = 1,
                SentToReviewersAt = DateTime.Now,
                Completed = true,
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "WEB").First().EnvironmentId
                    },
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "DOT").First().EnvironmentId
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S2").First().PeriodId
                    },
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S1").First().PeriodId
                    },
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "AO").First().SpecialisationId
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "JI").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "RE").First().ExpectationId
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {
                    new InternshipAssignedUser
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId
                    }
                },
                InternshipReviewer = new List<InternshipReviewer>
                {
                    new InternshipReviewer
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "REV").First().RoleId).ToList()[0].UserId
                    }
                },
                UserFavourites = new List<UserFavourites>
                {
                    new UserFavourites
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId
                    }
                },
                UserInternships = new List<UserInternships>
                {
                    new UserInternships
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId,
                        HireRequested = true,
                        HireConfirmed = true,
                        HireApproved = true,
                        EvaluatedAt = DateTime.Now
                    }
                },
                ShowInEvent = true
            });

            // Cronos Group: Approved internship with no student attached to favourites and 1 student attached to userinternships (applied - hire requested nadconfirmed)
            internshipsToAdd.Add(new Internship
            {
                CompanyId = companiesInDatabase.ToList()[1].CompanyId,
                ContactPersonId = contactsInDatabase.First().ContactId,
                PromotorFirstname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Firstname : contactsInDatabase[0].Firstname,
                PromotorSurname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Surname : contactsInDatabase[0].Surname,
                PromotorFunction = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Function : contactsInDatabase[0].Function,
                PromotorEmail = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Email : contactsInDatabase[0].Email,
                ProjectStatusId = statusInDatabase.Where(x => x.Code == "APP").First().ProjectStatusId,
                WpStreet = "Gaston Geenslaan",
                WpHouseNr = "11",
                WpBusNr = "B4",
                WpCity = "Leuven",
                WpZipCode = "3000",
                WpCountry = "Belgium",
                AssignmentDescription = "We verwelkomen graag een stagiair doe mee wil werken aan een gloednieuwe educatieve game rond tandzorg.",
                TechnicalDetails = "C#, Unity",
                Conditions = null,
                ResearchTopicTitle = "Physics in games",
                ResearchTopicDescription = "Onderzoek naar geschikte Unity packages om realtime physics te simuleren in games.",
                TotalInternsRequired = 1,
                ContactStudentName = null,
                AcademicYear = "2020-2021",
                Remark = null,
                CreatedAt = new DateTime(2020, 10, 16),
                InternalFeedback = null,
                CountTotalAssignedReviewers = 1,
                SentToReviewersAt = DateTime.Now,
                Completed = true,
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "DOT").First().EnvironmentId
                    },
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "TES").First().EnvironmentId
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S2").First().PeriodId
                    },
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S1").First().PeriodId
                    },
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "SN").First().SpecialisationId
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "JI").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "RE").First().ExpectationId
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {
                    new InternshipAssignedUser
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId
                    }
                },
                InternshipReviewer = new List<InternshipReviewer>
                {
                    new InternshipReviewer
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "REV").First().RoleId).ToList()[0].UserId
                    }
                },
                UserFavourites = new List<UserFavourites>
                {
                    new UserFavourites
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId
                    }
                },
                UserInternships = new List<UserInternships>
                {
                    new UserInternships
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId,
                        HireRequested = true,
                        HireConfirmed = true,
                        HireApproved = true,
                        EvaluatedAt = DateTime.Now
                    }
                },
                ShowInEvent = true
            });

            // Cronos Group: Approved internship with no student attached to favourites and 1 student attached to userinternships (applied - hire requested nadconfirmed)
            internshipsToAdd.Add(new Internship
            {
                CompanyId = companiesInDatabase.ToList()[1].CompanyId,
                ContactPersonId = contactsInDatabase.First().ContactId,
                PromotorFirstname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Firstname : contactsInDatabase[0].Firstname,
                PromotorSurname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Surname : contactsInDatabase[0].Surname,
                PromotorFunction = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Function : contactsInDatabase[0].Function,
                PromotorEmail = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Email : contactsInDatabase[0].Email,
                ProjectStatusId = statusInDatabase.Where(x => x.Code == "APP").First().ProjectStatusId,
                WpStreet = "Gaston Geenslaan",
                WpHouseNr = "11",
                WpBusNr = "B4",
                WpCity = "Leuven",
                WpZipCode = "3000",
                WpCountry = "Belgium",
                AssignmentDescription = "We hebben wel wat werk te doen voor een stagiair. Helaas is mijn inspiratie op.",
                TechnicalDetails = "C#, Python",
                Conditions = null,
                ResearchTopicTitle = "Backend applicatie in Linux",
                ResearchTopicDescription = "We verwachten dat je uitzoekt hoe we onze backend kunnen overzetten naar een Linux omgeving. Welke configuratie is hier voor nodig?",
                TotalInternsRequired = 1,
                ContactStudentName = null,
                AcademicYear = "2020-2021",
                Remark = null,
                CreatedAt = new DateTime(2020, 10, 16),
                InternalFeedback = null,
                CountTotalAssignedReviewers = 1,
                SentToReviewersAt = DateTime.Now,
                Completed = true,
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "EICT").First().EnvironmentId
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S2").First().PeriodId
                    },
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S1").First().PeriodId
                    },
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "EICT").First().SpecialisationId
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "JI").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "RE").First().ExpectationId
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {
                    new InternshipAssignedUser
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId
                    }
                },
                InternshipReviewer = new List<InternshipReviewer>
                {
                    new InternshipReviewer
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "REV").First().RoleId).ToList()[0].UserId
                    }
                },
                UserFavourites = new List<UserFavourites>
                {
                    new UserFavourites
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId
                    }
                },
                UserInternships = new List<UserInternships>
                {
                    new UserInternships
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId,
                        HireRequested = true,
                        HireConfirmed = true,
                        HireApproved = true,
                        EvaluatedAt = DateTime.Now
                    }
                },
                ShowInEvent = true
            });

            // Cegeka: Approved internship with no student attached to favourites and 1 student attached to userinternships (applied - hire requested nadconfirmed)
            internshipsToAdd.Add(new Internship
            {
                CompanyId = companiesInDatabase.ToList()[6].CompanyId,
                ContactPersonId = contactsInDatabase.First().ContactId,
                PromotorFirstname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Firstname : contactsInDatabase[0].Firstname,
                PromotorSurname = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Surname : contactsInDatabase[0].Surname,
                PromotorFunction = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Function : contactsInDatabase[0].Function,
                PromotorEmail = contactsInDatabase.Count > 1 ? contactsInDatabase[1].Email : contactsInDatabase[0].Email,
                ProjectStatusId = statusInDatabase.Where(x => x.Code == "APP").First().ProjectStatusId,
                WpStreet = "Universiteitslaan",
                WpHouseNr = "9",
                WpBusNr = "1",
                WpCity = "Hasselt",
                WpZipCode = "3500",
                WpCountry = "Belgium",
                AssignmentDescription = "Voor een klant mag je als stagiair meehelpen met de bouw van een mobile banking applicatie voor Android.",
                TechnicalDetails = "C#, Android",
                Conditions = null,
                ResearchTopicTitle = "Auth2.0: nuttig voor banking applicaties?",
                ResearchTopicDescription = "Je zoekt uit of Auth2.0 nuttig kan zijn voor mobile banking applicatie.",
                TotalInternsRequired = 1,
                ContactStudentName = null,
                AcademicYear = "2020-2021",
                Remark = null,
                CreatedAt = new DateTime(2020, 1, 30),
                InternalFeedback = null,
                CountTotalAssignedReviewers = 1,
                SentToReviewersAt = DateTime.Now,
                Completed = true,
                InternshipEnvironment = new List<InternshipEnvironment>
                {
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "MOB").First().EnvironmentId
                    },
                    new InternshipEnvironment
                    {
                        EnvironmentId = environmentsInDatabase.Where(x => x.Code == "DOT").First().EnvironmentId
                    }
                },
                InternshipEnvironmentOthers = null,
                InternshipPeriod = new List<InternshipPeriod>
                {
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S2").First().PeriodId
                    },
                    new InternshipPeriod
                    {
                        PeriodId = periodsInDatabase.Where(x => x.Code == "S1").First().PeriodId
                    },
                },
                InternshipSpecialisation = new List<InternshipSpecialisation>
                {
                    new InternshipSpecialisation
                    {
                        SpecialisationId = specialisationsInDatabase.Where(x => x.Code == "AO").First().SpecialisationId
                    }
                },
                InternshipExpectation = new List<InternshipExpectation>
                {
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "JI").First().ExpectationId
                    },
                    new InternshipExpectation
                    {
                        ExpectationId = expectationsInDatabase.Where(x => x.Code == "RE").First().ExpectationId
                    }
                },
                InternshipAssignedUser = new List<InternshipAssignedUser>
                {
                    new InternshipAssignedUser
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId
                    }
                },
                InternshipReviewer = new List<InternshipReviewer>
                {
                    new InternshipReviewer
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "REV").First().RoleId).ToList()[0].UserId
                    }
                },
                UserFavourites = new List<UserFavourites>
                {
                    new UserFavourites
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId
                    }
                },
                UserInternships = new List<UserInternships>
                {
                    new UserInternships
                    {
                        UserId = usersInDatabase.Where(x => x.RoleId == rolesInDatabase.Where(x => x.Code == "STU").First().RoleId).ToList()[0].UserId,
                        HireRequested = true,
                        HireConfirmed = true,
                        HireApproved = true,
                        EvaluatedAt = DateTime.Now
                    }
                },
                ShowInEvent = true
            });


            if (shouldOverwrite)
            {
                _RepositoryWrapper.Internship.DeleteRange(internshipsInDatabase);
                _RepositoryWrapper.Save();
            }
            else
            {
                // Look for users that are not yet in the database.
                internshipsToAdd = internshipsToAdd.Where(newInternship => !internshipsInDatabase.Any(dbObject => newInternship.AssignmentDescription == dbObject.AssignmentDescription)).ToList();
            }

            foreach (var internship in internshipsToAdd)
            {
                Console.WriteLine($" - Topic: {internship.ResearchTopicTitle}, Status: {internship.ProjectStatusId}");
            }

            _RepositoryWrapper.Internship.CreateRange(internshipsToAdd);
            _RepositoryWrapper.Save();
        }
    }
}
