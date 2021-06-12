using Repository.DTOs;
using Repository.Interfaces.Facades;
using System;
using System.Security.Claims;

namespace Authenticate.Facades
{
    public class AuthFacade : IAuthFacade
    {
        public ClaimsIdentity GetClaimsIdentityByContextUser(ContextUserDTO user, string authenticationType = "Bearer")
        {
            try
            {
                return new ClaimsIdentity(new Claim[]
                {   
                    new Claim(ClaimTypes.Name, user.Login),
                    new Claim(ClaimTypes.Role, user.Role),
                }, authenticationType);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
