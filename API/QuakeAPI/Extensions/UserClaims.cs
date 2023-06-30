using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace QuakeAPI.Extensions
{
    public static class UserClaims
    {
        public static int Id(this System.Security.Claims.ClaimsPrincipal user) 
        {
            var sub = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if(string.IsNullOrEmpty(sub?.Value))
                throw new ArgumentNullException("User claims does not contain Id");

            return Int32.Parse(sub.Value);
        }
    }
}