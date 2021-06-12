using Authenticate.Facades;
using BreakCaptcha.Factory;
using BreakCaptcha.Interfaces;
using Data.Context;
using Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Repository.Interfaces;
using Repository.Interfaces.Facades;
using Services.Interfaces;
using Services.Mapper;
using Services.Services;
using WebDrivers.Factorys;
using WebDrivers.Interfaces;

namespace IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddDbContext<RFEContext>();
            services.AddDbContext<PREFEITURASContext>();

            ///FACADES
            services.AddScoped<IAuthFacade, AuthFacade>();
            services.AddScoped<ITokenFacade, TokenFacade>();

            //SERVICES
            services.AddScoped<IAuthenticateAppService, AuthenticateAppService>();
            services.AddScoped<IPortalNfseAppService, PortalNfseAppService>();
            services.AddScoped<IConversoesAppService, ConversoesAppService>();

            //REPOSITORY
            services.AddScoped<IPortalNfseRepository, PortalNfseRepository>();
            services.AddScoped<IPortalNfseAnexosRepository, PortalNfseAnexosRepository>();
            services.AddScoped<IPortalNfseItemRepository, PortalNfseItemRepository>();
            services.AddScoped<IPortalFornecedorRepository, PortalFornecedorRepository>();
            services.AddScoped<IConversoesRepository, ConversoesRepository>();

            //Mappers
            services.AddScoped<IMapperObj, MapperObj>();

            //Factorys
            services.AddScoped<IWebDriverFactory, WebDriverFactory>();
            services.AddScoped<IFirefoxSetup, FirefoxSetup>();
            services.AddScoped<IBreakCaptchaFactory, BreakCaptchaFactory>();
        }
    }
}
