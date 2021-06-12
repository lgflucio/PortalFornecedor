using ExceptionHandler.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.DTOs;
using Services.Interfaces;
using Services.ViewModels;
using Shared.Enums;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateAppService _service;
        public AuthenticateController(IAuthenticateAppService service)
        {
            _service = service;
        }

        [ProducesResponseType(typeof(AuthReturnDTO), StatusCodes.Status200OK)]
        [HttpPost, AllowAnonymous]
        public IActionResult Authenticate(AuthenticateViewModel model)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ApiErrorCodes.MODNOTVALD);

            AuthReturnDTO _result = _service.Authenticate(model);

            return Ok(_result);
        }
    }
}
