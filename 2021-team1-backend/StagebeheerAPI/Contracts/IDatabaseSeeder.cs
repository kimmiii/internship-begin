namespace StagebeheerAPI.Contracts
{
    interface IDatabaseSeeder
    {
        public void SeedAllTables(bool shouldOverwrite = false);
        public void SeedStaticTables(bool shouldOverwrite = false);
        public void SeedTestData(bool shouldOverwrite = false);
        public void SeedCountries(bool shouldOverwrite = false);
        public void SeedPeriods(bool shouldOverwrite = false);
        public void SeedExpectations(bool shouldOverwrite = false);
        public void SeedSpecializations(bool shouldOverwrite = false);
        public void SeedRoles(bool shouldOverwrite = false);
        public void SeedStatus(bool shouldOverwrite = false);
        public void SeedEnvironments(bool shouldOverwrite = false);
        public void SeedUser(bool shouldOverwrite = false);
        public void SeedCompanies(bool shouldOverwrite = false);
        public void SeedContacts(bool shouldOverwrite = false);
        public void SeedInternships(bool shouldOverwrite = false);
    }
}
