using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.DTOs;
using Repository.Interfaces.Facades;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authenticate.Facades
{
    public class TokenFacade : ITokenFacade
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthFacade _facadeAuth;
        public TokenFacade(IConfiguration configuration, IAuthFacade facadeAuth)
        {
            _configuration = configuration;
            _facadeAuth = facadeAuth;
        }
        public string GenerateToken(ContextUserDTO user)
        {
            try
            {
                var _tokenHandler = new JwtSecurityTokenHandler();
                var _key = Encoding.ASCII.GetBytes(TokenConfigurationsDTO.Secret);

                var _tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Login.ToString()),
                        new Claim(ClaimTypes.Role, user.Role.ToString())
                    }, "Bearer"),
                    Expires = DateTime.UtcNow.AddHours(24),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature),
                    

                };

                var _generatedToken = _tokenHandler.CreateToken(_tokenDescriptor);

                return _tokenHandler.WriteToken(_generatedToken);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
