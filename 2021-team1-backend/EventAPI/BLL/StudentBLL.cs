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
    public interface IStudentBLL
    {
        Task<User> GetByIdAsync(int id);
    }

    public class StudentBLL : IStudentBLL
    {
        private readonly IStudentRepository _studentRepository;

        public StudentBLL(
            IMapper mapper,
            IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            return student;
        }
    }
}
