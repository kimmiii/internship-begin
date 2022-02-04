using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;

namespace StagebeheerAPI.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private StagebeheerDBContext _repoContext;

        private IInternshipRepository _internship;
        private IEnvironmentRepository _environment;
        private IExpectationRepository _expectation;
        private IPeriodRepository _period;
        private ISpecialisationRepository _specialisation;
        private IProjectStatusRepository _projectStatusRepository;

        private ICompanyRepository _companyRepository;
        private IContactRepository _contactRepository;

        private ICountryRepository _country;
        private IUserRepository _user;
        private IRoleRepository _role;

        private IInternshipEnvironmentRepository _internshipEnvironment;
        private IInternshipPeriodRepository _internshipPeriod;
        private IInternshipExpectationRepository _internshipExpectation;
        private IInternshipSpecialisationRepository _internshipSpecialisation;
        private IInternshipAssignedUserRepository _internshipAssignedUser;
        private IInternshipReviewerRepository _internshipReviewer;
        private IUserFavouritesRepository _userFavourites;
        private IUserInternshipsRepository _userInternships;

        public IUserRepository User
        {
            get
            {
                if(_user == null)
                {
                    _user = new UserRepository(_repoContext);
                }
                return _user;
            }
        }

        public IInternshipRepository Internship
        {
            get
            {
                if(_internship == null)
                {
                    _internship = new InternshipRepository(_repoContext);
                }
                return _internship;
            }
        }

        public IEnvironmentRepository Environment
        {
            get
            {
                if (_environment == null)
                {
                    _environment = new EnvironmentRepository(_repoContext);
                }
                return _environment;
            }
        }

        public IExpectationRepository Expectation
        {
            get
            {
                if (_expectation == null)
                {
                    _expectation = new ExpectationRepository(_repoContext);
                }
                return _expectation;
            }
        }

        public IPeriodRepository Period
        {
            get
            {
                if (_period == null)
                {
                    _period = new PeriodRepository(_repoContext);
                }
                return _period;
            }
        }

        public ISpecialisationRepository Specialisation
        {
            get
            {
                if (_specialisation == null)
                {
                    _specialisation = new SpecialisationRepository(_repoContext);
                }
                return _specialisation;
            }
        }

        public IInternshipEnvironmentRepository InternshipEnvironment
        {
            get
            {
                if (_internshipEnvironment == null)
                {
                    _internshipEnvironment = new InternshipEnvironmentRepository(_repoContext);
                }
                return _internshipEnvironment;
            }
        }

        public IInternshipPeriodRepository InternshipPeriod
        {
            get
            {
                if (_internshipPeriod == null)
                {
                    _internshipPeriod = new InternshipPeriodRepository(_repoContext);
                }
                return _internshipPeriod;
            }
        }

        public IInternshipExpectationRepository InternshipExpectation
        {
            get
            {
                if (_internshipExpectation == null)
                {
                    _internshipExpectation = new InternshipExpectationRepository(_repoContext);
                }
                return _internshipExpectation;
            }
        }

        public IInternshipSpecialisationRepository InternshipSpecialisation
        {
            get
            {
                if (_internshipSpecialisation == null)
                {
                    _internshipSpecialisation = new InternshipSpecialisationRepository(_repoContext);
                }
                return _internshipSpecialisation;
            }
        }

        public IInternshipAssignedUserRepository InternshipAssignedUser
        {
            get
            {
                if (_internshipAssignedUser == null)
                {
                    _internshipAssignedUser = new InternshipAssignedUserRepository(_repoContext);
                }
                return _internshipAssignedUser;
            }
        }

        public IInternshipReviewerRepository InternshipReviewer
        {
            get
            {
                if (_internshipReviewer == null)
                {
                    _internshipReviewer = new InternshipReviewerRepository(_repoContext);
                }
                return _internshipReviewer;
            }
        }

        public IUserFavouritesRepository UserFavourites
        {
            get
            {
                if (_userFavourites == null)
                {
                    _userFavourites = new UserFavouritesRepository(_repoContext);
                }
                return _userFavourites;
            }
        }

        public IUserInternshipsRepository UserInternships
        {
            get
            {
                if (_userInternships == null)
                {
                    _userInternships = new UserInternshipsRepository(_repoContext);
                }
                return _userInternships;
            }
        }
        public ICountryRepository Country
        {
            get
            {
                if(_country == null)
                {
                    _country = new CountryRepository(_repoContext);
                }
                return _country;
            }
        }

        public IRoleRepository Role {
            get {
                if (_role == null)
                {
                    _role = new RoleRepository(_repoContext);
                }
                return _role;
            }
        }

        public IProjectStatusRepository ProjectStatus {
            get {
                if (_projectStatusRepository == null)
                {
                    _projectStatusRepository = new ProjectStatusRepository(_repoContext);
                }
                return _projectStatusRepository;
            }
        }

        public ICompanyRepository Company {
            get {
                if (_companyRepository == null)
                {
                    _companyRepository = new CompanyRepository(_repoContext);
                }
                return _companyRepository;
            }
        }

        public IContactRepository Contact {
            get {
                if (_contactRepository == null)
                {
                    _contactRepository = new ContactRepository(_repoContext);
                }
                return _contactRepository;
            }
        }

        public RepositoryWrapper(StagebeheerDBContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}
