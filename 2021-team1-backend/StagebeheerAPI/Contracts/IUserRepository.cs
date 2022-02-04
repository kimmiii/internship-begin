using StagebeheerAPI.Models;
using System.Collections.Generic;

namespace StagebeheerAPI.Contracts
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        public string returnRoleCode(int userId);
        public string ReturnUserName(int userId);
        public string CheckReviewerHeader(List<string> headers);
        public string CheckStudentHeader(List<string> headers);
        public string CheckReviewerData(string[] rows, int commas);
        public string CheckStudentData(string[] rows, int commas);
    }
}
