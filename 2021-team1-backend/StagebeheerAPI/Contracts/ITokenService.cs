using System.Threading.Tasks;
using StagebeheerAPI.Models;

namespace StagebeheerAPI.Contracts
{
    public interface ITokenService
    {
        Task<string> GenerateJSONWebTokenAsync(User user);
    }
}
