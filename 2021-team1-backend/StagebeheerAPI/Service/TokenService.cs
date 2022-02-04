using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StagebeheerAPI.Contracts;
using StagebeheerAPI.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StagebeheerAPI.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(
            IConfiguration configuration,
            UserManager<User> userManager)
        {
            _configuration = configuration;
        }

        public async Task<string> GenerateJSONWebTokenAsync(User user)
        {
            //var userClaims = await _userManager.GetClaimsAsync(user);
            var companyId = user.Company != null ? user.Company.CompanyId.ToString() : string.Empty;
            var roleCode = user.Role != null ? user.Role.Code ?? string.Empty : string.Empty;
            var roleDescription = user.Role != null ? user.Role.Description ?? string.Empty : string.Empty;
            var isCompanyActivated = user.Company != null ? user.Company.Activated.ToString(): "False";
            var companyName = user.Company != null ? user.Company.Name?? string.Empty: string.Empty;

            var allClaims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.NameId, user.UserId.ToString(), ClaimValueTypes.Integer32),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.UserEmailAddress, ClaimValueTypes.Email),
                    new Claim("companyId", companyId, ClaimValueTypes.Integer32),
                    new Claim("RoleId", user.RoleId.ToString(), ClaimValueTypes.Integer32),
                    new Claim("firstName", user.UserFirstName?? string.Empty),
                    new Claim("surname", user.UserSurname?? string.Empty),
                    new Claim("roleCode", roleCode),
                    new Claim("roleDescription", roleDescription),
                    new Claim("isUserActivated", user.Activated.ToString(), ClaimValueTypes.Boolean),
                    new Claim("cvPresent", user.CvPresent.ToString(), ClaimValueTypes.Boolean),
                    new Claim("isCompanyActivated", isCompanyActivated, ClaimValueTypes.Boolean),
                    new Claim("companyName", companyName)
            }.ToList();

            var key = _configuration["Jwt:Key"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var issuer = _configuration["Jwt:Issuer"];
            var token = new JwtSecurityToken(
                issuer, 
                issuer,
                allClaims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

     
    }

}
