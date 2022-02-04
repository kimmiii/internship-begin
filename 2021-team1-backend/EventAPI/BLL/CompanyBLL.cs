using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EventAPI.DAL.Repositories;
using EventAPI.Domain.Models;
using EventAPI.Domain.ViewModels;

namespace EventAPI.BLL
{
    public interface ICompanyBll
    {
        Task<IList<Company>> GetAsync();
        Task<IList<CompanyVM>> GetAsyncVM();
        Task<Company> GetByIdAsync(int id);
        Task<Company> GetByIdAsync();
        Task<CompanyVM> GetByIdAsyncVM();
        Task<IList<CompanyFilterVM>> GetCompaniesForFilterAsyncVM();
    }

    public class CompanyBll : ICompanyBll
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompanyBll(
            IMapper mapper,
            ICompanyRepository companyRepository
        )
        {
            _mapper = mapper;
            _companyRepository = companyRepository;
        }

        public async Task<IList<Company>> GetAsync()
        {
            var companies = await _companyRepository.GetAsync();
            return companies.ToList();
        }

        public async Task<IList<CompanyVM>> GetAsyncVM()
        {
            var companies = await GetAsync();
            return _mapper.Map<List<CompanyVM>>(companies);
        }

        public async Task<Company> GetByIdAsync()
        {
            var company = await _companyRepository.GetByIdAsync();
            return company;
        }

        public async Task<Company> GetByIdAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            return company;
        }

        public async Task<CompanyVM> GetByIdAsyncVM()
        {
            var company = await GetByIdAsync();
            return _mapper.Map<CompanyVM>(company);
        }

        public async Task<IList<CompanyFilterVM>> GetCompaniesForFilterAsyncVM()
        {
            var companies = await GetAsync();
            return _mapper.Map<List<CompanyFilterVM>>(companies);
        }
    }
}