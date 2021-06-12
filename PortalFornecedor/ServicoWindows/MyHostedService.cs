using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PrefeiturasScrapping.Interfaces;
using PrefeiturasWebServices.Interfaces;
using Repository.DTOs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WebDrivers.Interfaces;
using IUberlandiaAppService = PrefeiturasScrapping.Interfaces.IUberlandiaAppService;

public class MyHostedService : IMyHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IWebDriverFactory _driver;
    private readonly IRioDeJaneiroAppService _rioDeJaneiroAppService;
    private readonly IMapper _mapper;

    #region configs
    private IConfiguration _config { get; }
    private readonly PrefeiturasConfigDTO _settings;
    #endregion

    public MyHostedService( IServiceScopeFactory scopeFactory, 
                            IWebDriverFactory driver,
                            IRioDeJaneiroAppService rioDeJaneiroAppService,
                            IMapper mapper,
                            IConfiguration _config)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
        _driver = driver;
        _rioDeJaneiroAppService = rioDeJaneiroAppService;

        #region Configs
        this._config = _config;
        _settings = new PrefeiturasConfigDTO();
        _config.GetSection("PrefeiturasConfig").Bind(_settings);
        #endregion
    }

    public void DoWork()
    {
        PrefeiturasConfigDTO config = _settings;
        config?.Consultas?.Where(consulta => consulta.Executar).ToList().ForEach(consulta =>
        {
            ExecuteConsulta(consulta);
        });
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    private void ExecuteConsulta(Consulta consulta)
    {
        switch (consulta.TipoConsulta)
        {

            //case "Barueri":
            //    using (var scope = _scopeFactory.CreateScope())
            //    {
            //        IBarueriAppService BarueriAppService = scope.ServiceProvider.GetRequiredService<IBarueriAppService>();
            //        BarueriAppService.Get();
            //    }

            //    break;

            //case "Blumenau":
            //    using (var scope = _scopeFactory.CreateScope())
            //    {
            //        IBlumenauAppService BlumenauAppService = scope.ServiceProvider.GetRequiredService<IBlumenauAppService>();
            //        BlumenauAppService.Get(consulta.PeriodoDeConsulta, consulta.DiretorioDownload);
            //    }

            //    break;

            //case "Campinas":
            //    using (var scope = _scopeFactory.CreateScope())
            //    {
            //        ICampinasAppService CampinasAppService = scope.ServiceProvider.GetRequiredService<ICampinasAppService>();
            //        CampinasAppService.Get();
            //    }

            //    break;

            //case "Chapeco":
            //    using (var scope = _scopeFactory.CreateScope())
            //    {
            //        IChapecoAppService ChapecoAppService = scope.ServiceProvider.GetRequiredService<IChapecoAppService>();
            //        ChapecoAppService.Get();
            //    }

            //    break;

            //case "Guarulhos":
            //    break;

            //case "Manaus":
            //    using (var scope = _scopeFactory.CreateScope())
            //    {
            //        IManausAppService ManausAppService = scope.ServiceProvider.GetRequiredService<IManausAppService>();
            //        ManausAppService.Get(consulta.PeriodoDeConsulta);
            //    }

            //    break;

            //case "PortoAlegre":
            //    using (var scope = _scopeFactory.CreateScope())
            //    {
            //        IPortoAlegreAppService PortoAlegreAppService = scope.ServiceProvider.GetRequiredService<IPortoAlegreAppService>();
            //        PortoAlegreAppService.Get(360000);
            //    }

            //    break;

            //    case "RioDeJaneiro":
            //    using (var scope = _scopeFactory.CreateScope())
            //    {
            //        IRioDeJaneiroAppService RioDeJaneiroAppService = scope.ServiceProvider.GetRequiredService<IRioDeJaneiroAppService>();
            //        RioDeJaneiroAppService.Start(consulta.PeriodoDeConsulta, consulta.DiretorioDownload);
            //    }

            //    break;

            //case "Santos":
            //    using (var scope = _scopeFactory.CreateScope())
            //    {
            //        ISantosAppService SantosAppService = scope.ServiceProvider.GetRequiredService<ISantosAppService>();
            //        SantosAppService.Get();
            //    }

            //    break;

            //case "SaoPaulo":
            //    using (var scope = _scopeFactory.CreateScope())
            //    {
            //        ISaoPauloAppService SaoPauloAppService = scope.ServiceProvider.GetRequiredService<ISaoPauloAppService>();
            //        SaoPauloAppService.Get(consulta.PeriodoDeConsulta);
            //    }

            //    break;

            //case "Uberlandia":
            //    using (var scope = _scopeFactory.CreateScope())
            //    {
            //        IUberlandiaAppService UberlandiaAppService = scope.ServiceProvider.GetRequiredService<IUberlandiaAppService>();
            //        UberlandiaAppService.Get(consulta.PeriodoDeConsulta);
            //    }

            //    break;

            default:
                return;
        }
    }
}
