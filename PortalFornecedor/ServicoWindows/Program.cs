using Data.Context;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrefeiturasWebServices.Interfaces;
using PrefeiturasWebServices.Services;
using Repository.Interfaces;
using Services.Interfaces;
using Services.Mapper;
using Services.Services;
using WebDrivers.Factorys;
using WebDrivers.Interfaces;
using BarueriAppService = PrefeiturasWebServices.Services.BarueriAppService;
using IBarueriAppService = PrefeiturasWebServices.Interfaces.IBarueriAppService;
using ICampinasAppService = PrefeiturasScrapping.Interfaces.ICampinasAppService;
using CampinasAppService = PrefeiturasScrapping.Services.CampinasAppService;
using PrefeiturasWebServices.Factorys;
using PrefeiturasScrapping.Interfaces;
using PrefeiturasScrapping.Services;
using API.Configurations;
using IUberlandiaAppService = PrefeiturasScrapping.Interfaces.IUberlandiaAppService;
using UberlandiaAppService = PrefeiturasScrapping.Services.UberlandiaAppService;
using BreakCaptcha.Interfaces;
using BreakCaptcha.Factory;
using ReceiverApi.Interfaces;
using ReceiverApi.Factory;

namespace ServicoWindows
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .UseDefaultServiceProvider(options => options.ValidateScopes = false)
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();

                    ConfigureServices(hostContext.Configuration, services);
                });
        private static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddDbContext<RFEContext>(options => options.UseSqlServer(configuration.GetConnectionString("RFEContext")));
            services.AddDbContext<PREFEITURASContext>(options => options.UseSqlServer(configuration.GetConnectionString("PREFEITURASContext")));
            
            ///SERVICES
            services.AddSingleton<IBarueriAppService, BarueriAppService>();
            services.AddSingleton<ICampinasAppService, CampinasAppService>();
            services.AddSingleton<ISaoPauloAppService, SaoPauloAppService>();
            services.AddSingleton<IPortoAlegreAppService, PortoAlegreAppService>();
            services.AddSingleton<IChapecoAppService, ChapecoAppService>();
            services.AddSingleton<IUberlandiaAppService, UberlandiaAppService>();
            services.AddSingleton<IManausAppService, ManausAppService>();
            services.AddSingleton<ISantosAppService, SantosAppService>();
            services.AddSingleton<IRioDeJaneiroAppService, RioDeJaneiroAppService>();
            services.AddSingleton<IBlumenauAppService, BlumenauAppService>();
            //services.AddSingleton<IPageRJComponente, PageRJComponente>();
            //services.AddScoped<IProcessarArquivosNFSe, ProcessarArquivosNFSe>();
            services.AddScoped<INfseAppService, NfseAppService>();
            services.AddScoped<INfsePrefeiturasAppService, NfsePrefeiturasAppService>();
            services.AddScoped<ICertificadoAppService, CertificadoAppService>();

            ///MAPPER
            services.AddAutoMapperSetup();
            services.AddScoped<IMapperObj, MapperObj>();

            ///REPOSITORY
            services.AddScoped<ICertificadosRepository, CertificadosRepository>();
            services.AddScoped<ICiaRepository, CiaRepository>();
            services.AddScoped<IItemNfseRepository, ItemNfseRepository>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<IHistoricoRepository, HistoricoRepository>();
            services.AddScoped<INfseRepository, NfseRepository>();
            services.AddScoped<INfsePrefeiturasRepository, NfsePrefeiturasRepository>();
            services.AddScoped<IRepositorioRepository, RepositorioRepository>();
            services.AddScoped<IMunicipioNfseRepository, MunicipiosNfseRepository>();
            services.AddScoped<IMyHostedService, MyHostedService>();
            services.AddScoped<ITagsXmlsRepository, TagsXmlsRepository>();
            services.AddScoped<IItensNfseRepository, ItensNfseRepository>();
            services.AddScoped<IContainerNotasRepository, ContainerNotasRepository>();
            //services.AddScoped<IPageRJComponenteRepository, PageRJComponenteRepository>();

            ///WebDrivers
            services.AddScoped<IFirefoxSetup, FirefoxSetup>();

            ///Factorys
            services.AddScoped<IWebDriverFactory, WebDriverFactory>();
            services.AddScoped<ISaoPauloFactory, SaoPauloFactory>();
            services.AddScoped<IBreakCaptchaFactory, BreakCaptchaFactory>();
            services.AddScoped<IReceiverApiFactory, ReceiverApiFactory>();

        }
    }
}
