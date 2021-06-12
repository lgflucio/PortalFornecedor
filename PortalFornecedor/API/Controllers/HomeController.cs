using ExceptionHandler.Extensions;
using ExceptionHandler.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.ViewModels;
using Shared.Enums;

namespace API.Controllers
{
    [Route("[controller]"), ApiController]
    public class HomeController : ControllerBase
    {
        //private readonly ICiaAppService _serviceCia;
        //private readonly IUsuariosPrefeiturasAppService _serviceUsuariosPrefeitura;
        //public HomeController(ICiaAppService serviceCia,
        //                       IUsuariosPrefeiturasAppService serviceUsuariosPrefeitura)
        //{
        //    _serviceCia = serviceCia;
        //    _serviceUsuariosPrefeitura = serviceUsuariosPrefeitura;
        //}

        ///// <summary>
        ///// Obter mensagem padrão
        ///// </summary>
        ///// <response code="200">A Lista foi impressa com sucesso</response>
        ///// <response code="400">Nao foi encontrado nenhuma resposta para a requisição</response>
        ///// <response code="500">Ocorreu um erro ao obter usuário</response>
        ///// <returns></returns>
        ////[Authorize(Roles = "admin")]
        //[HttpGet, Authorize]
        //[ProducesResponseType(typeof(CiaViewModel), StatusCodes.Status200OK)]
        //public IActionResult Get()
        //{
        //    if (!ModelState.IsValid)
        //        throw new ApiException(ModelState.GetAllErrorsToString(), ApiErrorCodes.MODNOTVALD);

        //    return Ok(_serviceCia.Get());
        //}

        ////[Authorize(Roles = "admin")]
        //[HttpPost("usuarios-prefeituras")]
        ////[ProducesResponseType(typeof(CiaViewModel), StatusCodes.Status200OK)]
        //public IActionResult Post(UsuarioPrefeituraCreateViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        throw new ApiException(ModelState.GetAllErrorsToString(), ApiErrorCodes.MODNOTVALD);

        //    return Ok(_serviceUsuariosPrefeitura.Post(model));
        //}
    }
}
