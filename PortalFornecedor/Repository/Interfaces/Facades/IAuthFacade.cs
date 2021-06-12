using Repository.DTOs;
using System.Security.Claims;

namespace Repository.Interfaces.Facades
{
    public interface IAuthFacade
    {
        ClaimsIdentity GetClaimsIdentityByContextUser(ContextUserDTO user, string authenticationType = "Bearer");
    }
}
