using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace api.Extension
{
    public static class ClaimExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            //"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname" = UserName = ClaimTypes.GivenName

            return user.Claims.SingleOrDefault(x => x.Type.Equals(ClaimTypes.GivenName)).Value; 
        }
    }
}