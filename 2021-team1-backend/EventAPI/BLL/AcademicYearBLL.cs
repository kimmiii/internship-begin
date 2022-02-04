using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EventAPI.DAL.Repositories;
using EventAPI.Domain.Models;
using EventAPI.Domain.ViewModels;

namespace EventAPI.BLL
{
    public interface IAcademicYearBll
    {
        Task<IList<AcademicYear>> GetAsync();
        Task<IList<AcademicYearVM>> GetAsyncVm();
        Task<AcademicYear> GetByIdAsync(Guid id);
        Task<AcademicYearVM> GetByIdAsyncVm(Guid id);
        Task<AcademicYear> CreateAsync();
        Task<AcademicYearVM> CreateAsyncVm();
    }

    public class AcademicYearBll : IAcademicYearBll
    {
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly IMapper _mapper;

        public AcademicYearBll(
            IMapper mapper,
            IAcademicYearRepository academicYearRepository
        )
        {
            _mapper = mapper;
            _academicYearRepository = academicYearRepository;
        }

        public async Task<IList<AcademicYear>> GetAsync()
        {
            var academicYears = await _academicYearRepository.GetAsync();
            return academicYears.ToList();
        }

        public async Task<IList<AcademicYearVM>> GetAsyncVm()
        {
            var academicYears = await GetAsync();
            return _mapper.Map<List<AcademicYearVM>>(academicYears);
        }

        public async Task<AcademicYear> GetByIdAsync(Guid id)
        {
            var academicYear = await _academicYearRepository.GetByIdAsync(id);
            return academicYear;
        }

        public async Task<AcademicYearVM> GetByIdAsyncVm(Guid id)
        {
            var academicYear = await GetByIdAsync(id);
            return _mapper.Map<AcademicYearVM>(academicYear);
        }

        public async Task<AcademicYear> CreateAsync()
        {
            var academicYear = await _academicYearRepository.CreateAsync();
            return academicYear;
        }

        public async Task<AcademicYearVM> CreateAsyncVm()
        {
            var academicYear = await CreateAsync();
            return _mapper.Map<AcademicYearVM>(academicYear);
        }
    }
}