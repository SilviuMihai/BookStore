using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public static class ClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
        {
            //has string type(claim type) and string value(claim value)
            new Claim("Create Role","Create Role"),
            new Claim("Edit Role","Edit Role"),
            new Claim("Delete Role","Delete Role")
        };
    }
}
