using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace EventAPI.Helpers
{
    public static class Helper
    {
        public static T GetClaimFromToken<T>(IHttpContextAccessor httpContextAccessor, string claimType)
        {
            var token = httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()
                .Replace("Bearer ", string.Empty, StringComparison.CurrentCultureIgnoreCase);
            var decryptedToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var value = decryptedToken.Claims.First(c => c.Type == $"{claimType}").Value;

            return (T) Convert.ChangeType(value, typeof(T));
        }
    }
}