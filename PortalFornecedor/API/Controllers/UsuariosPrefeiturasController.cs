using ExceptionHandler.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.DTOs;
using Services.Interfaces;
using Services.ViewModels;
using Shared.Enums;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsuariosPrefeiturasController : ControllerBase
    {
        //private readonly IUsuariosPrefeiturasAppService _service;
        //public UsuariosPrefeiturasController(IUsuariosPrefeiturasAppService service)
        //{
        //    _service = service;
        //}

        //[HttpPost, AllowAnonymous]
        //public IActionResult Post(UsuarioPrefeituraCreateViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        throw new ApiException(ApiErrorCodes.MODNOTVALD);

        //    UsuarioPrefeituraCreateViewModel _result = _service.Post(model);

        //    return Created($"Usuário criado.",_result);
        //}
    }
}
