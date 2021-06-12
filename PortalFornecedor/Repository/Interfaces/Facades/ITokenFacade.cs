using Repository.DTOs;

namespace Repository.Interfaces.Facades
{
    public interface ITokenFacade
    {
        string GenerateToken(ContextUserDTO user);
    }
}
