using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StagebeheerAPI.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class AppUserStore : IUserStore<User>, IUserPasswordStore<User>
    {
        private IRepositoryWrapper _repoWrapper;

        public AppUserStore(IRepositoryWrapper repoWrapper)
        {
            _repoWrapper = repoWrapper;
        }

        #region IUserStore
        public Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {

            _repoWrapper.User.Create(new User
            {
                UserEmailAddress = user.UserEmailAddress,
                UserPass = user.UserPass,
                RegistrationDate = user.RegistrationDate,
                Activated = user.Activated,
                CvPresent = user.CvPresent,
                RoleId = user.RoleId
            });
            _repoWrapper.Save();

            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {

            var appUser = _repoWrapper.User.FindByCondition(u => u.UserId == user.UserId).Include(u => u.Role).First();

            if (appUser != null)
            {
                _repoWrapper.User.Delete(appUser);
            }
            _repoWrapper.Save();

            return Task.FromResult(IdentityResult.Success);
        }

        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return Task.FromResult(_repoWrapper.User.FindByCondition(u => u.UserId == Convert.ToInt32(userId))
                                                    .Include(u => u.Role)
                                                    .Include(u => u.Company)
                                                    .FirstOrDefault());
        }

        public Task<User> FindByNameAsync(string userEmail, CancellationToken cancellationToken)
        {
            return Task.FromResult(_repoWrapper.User.FindByCondition(u => u.UserEmailAddress == userEmail)
                                                    .Include(u => u.Role)
                                                    .Include(u => u.Company)
                                                    .FirstOrDefault());
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserEmailAddress);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserId.ToString());
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserEmailAddress);
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            user.UserEmailAddress = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(User user, string userEmail, CancellationToken cancellationToken)
        {
            user.UserEmailAddress = userEmail;
            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            var appUser = _repoWrapper.User.FindByCondition(u => u.UserId == user.UserId).Include(u => u.Role).FirstOrDefault();

            if (appUser != null)
            {
                appUser.UserEmailAddress = user.UserEmailAddress;
                appUser.UserPass = user.UserPass;
            }

            return Task.FromResult(IdentityResult.Success);
        }

        #endregion

        #region IUserPasswordStore
        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserPass != null);
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserPass);
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.UserPass = passwordHash;
            return Task.CompletedTask;
        }

        #endregion
    }
}
