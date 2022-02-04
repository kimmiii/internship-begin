using AutoMapper;
using EventAPI.DAL.Repositories;
using EventAPI.Domain.Models;
using EventAPI.Domain.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Environment = EventAPI.Domain.Models.Environment;

namespace EventAPI.BLL
{
    public interface IInternshipBll
    {
        Task<IList<Internship>> GetAsync();
        Task<IList<InternshipVM>> GetAsyncVm();
        Task<Internship> GetByIdAsync(int id);
        Task<InternshipVM> GetByIdAsyncVm(int id);
        Task<IList<Internship>> GetByCompanyAsync(int id);
        Task<IList<InternshipVM>> GetByCompanyAsyncVm(int id);
        Task<IList<Internship>> GetByAcademicYearAsync(string academicYear);
        Task<IList<InternshipVM>> GetByAcademicYearAsyncVm(string academicYear);
        Task<IList<Internship>> GetInternshipsFromEventByCompanyAsync(string academicYear, int companyId);
        Task<IList<InternshipVM>> GetInternshipsFromEventByCompanyAsyncVm(string academicYear, int companyId);
        Task<Internship> UpdateInternshipAsync(Internship internship);
        Task<InternshipVM> UpdateInternshipAsyncVm(InternshipVM internshipVm);
        Task<List<Internship>> UpdateInternshipsMultiAsync(List<Internship> internships);
        Task<List<InternshipVM>> UpdateInternshipsMultiAsyncVm(List<InternshipVM> internshipVm);
        Task<List<Specialisation>> GetSpecializationsForFilterAsync();
        Task<List<SpecialisationFilterVM>> GetSpecializationsForFilterAsyncVm();
        Task<List<Location>> GetLocationsForFilterAsync();
        Task<List<LocationFilterVM>> GetLocationsForFilterAsyncVm();
        Task<List<Environment>> GetEnvironmentsForFilterAsync();
        Task<List<EnvironmentFilterVm>> GetEnvironmentsForFilterAsyncVm();
        Task<List<Contact>> GetContactsAsync();
        Task<List<ContactVM>> GetContactsAsyncVm();
    }

    public class InternshipBll : IInternshipBll
    {
        private readonly IInternshipRepository _internshipRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IEnvironmentRepository _environmentRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;
        private readonly ISpecializationRepository _specializationRepository;

        public InternshipBll(
            IMapper mapper,
            IInternshipRepository internshipRepository,
            ISpecializationRepository specializationRepository,
            ILocationRepository locationRepository,
            IEnvironmentRepository environmentRepository,
            IContactRepository contactRepository)
        {
            _mapper = mapper;
            _internshipRepository = internshipRepository;
            _specializationRepository = specializationRepository;
            _locationRepository = locationRepository;
            _environmentRepository = environmentRepository;
            _contactRepository = contactRepository;
        }

        public async Task<IList<Internship>> GetAsync()
        {
            var internships = await _internshipRepository.GetAsync();
            return internships.ToList();
        }

        public async Task<IList<InternshipVM>> GetAsyncVm()
        {
            var internships = await GetAsync();
            return _mapper.Map<List<InternshipVM>>(internships);
        }

        public async Task<Internship> GetByIdAsync(int id)
        {
            var internship = await _internshipRepository.GetByIdAsync(id);
            return internship;
        }

        public async Task<InternshipVM> GetByIdAsyncVm(int id)
        {
            var internship = await GetByIdAsync(id);
            return _mapper.Map<InternshipVM>(internship);
        }

        public async Task<IList<Internship>> GetByCompanyAsync(int id)
        {
            var internships = await _internshipRepository.GetByCompanyAsync(id);
            return internships.ToList();
        }

        public async Task<IList<InternshipVM>> GetByCompanyAsyncVm(int id)
        {
            var internships = await GetByCompanyAsync(id);
            return _mapper.Map<List<InternshipVM>>(internships);
        }

        public async Task<IList<Internship>> GetByAcademicYearAsync(string academicYear)
        {
            var internships = await _internshipRepository.GetByAcademicYearAsync(academicYear);
            return internships.ToList();
        }

        public async Task<IList<InternshipVM>> GetByAcademicYearAsyncVm(string academicYear)
        {
            var internships = await GetByAcademicYearAsync(academicYear);
            return _mapper.Map<List<InternshipVM>>(internships);
        }

        public async Task<IList<Internship>> GetInternshipsFromEventByCompanyAsync(string academicYear, int companyId)
        {
            var internships =
                await _internshipRepository.GetInternshipsFromEventByCompanyAsync(academicYear, companyId);
            return internships.ToList();
        }

        public async Task<IList<InternshipVM>> GetInternshipsFromEventByCompanyAsyncVm(string academicYear,
            int companyId)
        {
            var internships = await GetInternshipsFromEventByCompanyAsync(academicYear, companyId);
            return _mapper.Map<List<InternshipVM>>(internships);
        }

        public async Task<Internship> UpdateInternshipAsync(Internship internship)
        {
            var internshipToUpdate = await _internshipRepository.GetByIdAsync(internship.InternshipId);
            internshipToUpdate = _mapper.Map(internship, internshipToUpdate);
            await _internshipRepository.UpdateAsync(internshipToUpdate);
            var updatedInternship = await _internshipRepository.GetByIdAsync(internship.InternshipId);
            return updatedInternship;
        }

        public async Task<InternshipVM> UpdateInternshipAsyncVm(InternshipVM internshipVm)
        {
            var internship = _mapper.Map<Internship>(internshipVm);
            internship = await UpdateInternshipAsync(internship);
            return _mapper.Map<InternshipVM>(internship);
        }

        public async Task<List<Internship>> UpdateInternshipsMultiAsync(List<Internship> internships)
        {
            // TODO refactor use UpdateInternshipAsync(Internship internship)
            var updatedList = new List<Internship>();
            foreach (var internship in internships)
            {
                var internshipToUpdate = await _internshipRepository.GetByIdAsync(internship.InternshipId);
                internshipToUpdate = _mapper.Map(internship, internshipToUpdate);
                await _internshipRepository.UpdateAsync(internshipToUpdate);
                var updatedInternship = await _internshipRepository.GetByIdAsync(internship.InternshipId);
                updatedList.Add(updatedInternship);
            }

            return updatedList;
        }

        public async Task<List<InternshipVM>> UpdateInternshipsMultiAsyncVm(List<InternshipVM> internshipVm)
        {
            var internship = _mapper.Map<List<Internship>>(internshipVm);
            internship = await UpdateInternshipsMultiAsync(internship);
            return _mapper.Map<List<InternshipVM>>(internship);
        }

        public async Task<List<Specialisation>> GetSpecializationsForFilterAsync()
        {
            var specializations = await _specializationRepository.GetAsync();
            return specializations.ToList();
        }

        public async Task<List<SpecialisationFilterVM>> GetSpecializationsForFilterAsyncVm()
        {
            var specializations = await GetSpecializationsForFilterAsync();
            return _mapper.Map<List<SpecialisationFilterVM>>(specializations);
        }

        public async Task<List<Location>> GetLocationsForFilterAsync()
        {
            var locations = await _locationRepository.GetAsync();
            return locations.ToList();
        }

        public async Task<List<LocationFilterVM>> GetLocationsForFilterAsyncVm()
        {
            var locations = await GetLocationsForFilterAsync();
            return _mapper.Map<List<LocationFilterVM>>(locations);
        }

        public async Task<List<Environment>> GetEnvironmentsForFilterAsync()
        {
            var environments = await _environmentRepository.GetAsync();
            return environments.ToList();
        }

        public async Task<List<EnvironmentFilterVm>> GetEnvironmentsForFilterAsyncVm()
        {
            var environments = await GetEnvironmentsForFilterAsync();
            return _mapper.Map<List<EnvironmentFilterVm>>(environments);
        }

        public async Task<List<Contact>> GetContactsAsync()
        {
            var contacts = await _contactRepository.GetAsync();
            return contacts.ToList();
        }

        public async Task<List<ContactVM>> GetContactsAsyncVm()
        {
            var contacts = await GetContactsAsync();
            return _mapper.Map<List<ContactVM>>(contacts);
        }
    }
}