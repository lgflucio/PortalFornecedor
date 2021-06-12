using ExceptionHandler.Extensions;
using ExceptionHandler.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConversoesController : ControllerBase
    {
        private readonly IConversoesAppService _service;
        public ConversoesController(IConversoesAppService service)
        {
            _service = service;
        }
        [HttpPost("send/{id}")]
        public IActionResult FilaConversao(int id)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.GetAllErrorsToString(), ApiErrorCodes.MODNOTVALD);

            var _result = _service.FilaConversao(id);

            return Ok(_result);

        }
    }
}
