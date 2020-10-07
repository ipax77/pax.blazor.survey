using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pax.blazor.survey.Services
{
    public class AuthenticationService
    {
        public static async Task<string> CheckAuth(AuthenticationStateProvider _auth)
        {
            string name = String.Empty;
            AuthenticationState authState = await _auth.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity.IsAuthenticated)
            {
                name = user.Identity.Name;
            }
            return name;
        }
    }
}
