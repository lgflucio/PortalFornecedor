using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swagger.ViewModels;
using System;
using System.Text;

namespace Swagger.Providers
{
    public static class JWTConfiguration
    {
        public static IServiceCollection AddCustomJWTConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                var _tokenConfigurations =
                        configuration.GetSection("TokenConfigurations")
                            ?.Get<TokenConfigurationsViewModel>();

                if (_tokenConfigurations == null)
                    throw new ArgumentNullException("Não foi possível encontrar as configurações do Token JWT.");

                var _key = Encoding.ASCII.GetBytes(_tokenConfigurations.Secret);

                services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                    .AddJwtBearer(x =>
                    {
                        x.RequireHttpsMetadata = false;
                        x.SaveToken = true;
                        x.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(_key),
                            ValidateIssuer = false, //caso o servidor de autorização seja diferente
                            ValidateAudience = false, //seria o servidor que vai fazer o request da autorização pro Issuer
                            RequireExpirationTime = true
                        };
                    });

                return services;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}