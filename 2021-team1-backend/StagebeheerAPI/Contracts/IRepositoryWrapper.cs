namespace StagebeheerAPI.Contracts
{
    public interface IRepositoryWrapper
    {
        IInternshipRepository Internship { get; }
        IEnvironmentRepository Environment { get; }
        IExpectationRepository Expectation { get; }
        IPeriodRepository Period { get; }
        ISpecialisationRepository Specialisation { get; }
        IProjectStatusRepository ProjectStatus { get; }

        ICompanyRepository Company { get; }
        IContactRepository Contact { get; }

        IUserRepository User { get; }
        IRoleRepository Role { get; }

        ICountryRepository Country { get; }

        IInternshipEnvironmentRepository InternshipEnvironment { get; }
        IInternshipPeriodRepository InternshipPeriod { get; }
        IInternshipExpectationRepository InternshipExpectation { get; }
        IInternshipSpecialisationRepository InternshipSpecialisation { get; }
        IInternshipReviewerRepository InternshipReviewer { get; }
        IInternshipAssignedUserRepository InternshipAssignedUser { get; }
        IUserFavouritesRepository UserFavourites { get; }
        IUserInternshipsRepository UserInternships { get; }
        void Save();
    }
}
