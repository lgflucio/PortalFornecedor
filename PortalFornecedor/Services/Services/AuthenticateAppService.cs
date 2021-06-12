using Data.Repositories;
using Repository.DTOs;
using Repository.Interfaces.Facades;
using Services.Interfaces;
using Services.ViewModels;

namespace Services.Services
{

    public class AuthenticateAppService : IAuthenticateAppService
    {
        private readonly ITokenFacade _tokenFacade;
        public AuthenticateAppService(ITokenFacade tokenFacade)
        {
            _tokenFacade = tokenFacade;
        }
        public AuthReturnDTO Authenticate(AuthenticateViewModel model)
        {
            var user = AuthenticateRepository.Get(model.Login, model.Password);

            if (user == null)
                return new AuthReturnDTO
                {
                    IsAuthenticated = false,
                    TokenType = "Bearer",
                    User = new ContextUserDTO
                    {
                        Login = "Usuario ou senha inválidos"
                    }
                };

            ContextUserDTO _user = new ContextUserDTO
            {
                Id = user.Id,
                Login = user.Login,
                Name = user.Password,
                Role = user.Role
            };

            return new AuthReturnDTO
            {
                IsAuthenticated = true,
                TokenType = "Bearer",
                Token = _tokenFacade.GenerateToken(_user),
                User = _user
            };
        }
    }

}
