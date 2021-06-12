using ExceptionHandler.Extensions;
using ExceptionHandler.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.ViewModels;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PortalNfseController : ControllerBase
    {
        private readonly IPortalNfseAppService _service;
        public PortalNfseController(IPortalNfseAppService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PortalNfseViewModel), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.GetAllErrorsToString(), ApiErrorCodes.MODNOTVALD);
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PortalNfseDetalhesViewModel), StatusCodes.Status200OK)]
        public IActionResult Get(int id)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.GetAllErrorsToString(), ApiErrorCodes.MODNOTVALD);

            return Ok(_service.GetById(id));
        }

        [HttpGet("download/{id}")]
        public IActionResult DownloadFile(int id)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.GetAllErrorsToString(), ApiErrorCodes.MODNOTVALD);

            FileContentResult _result = _service.DownloadFile(id);
            if (_result == null)
            {
                return Ok("Não foi possível localizar o arquivo.");
            }
            else
            {
                return Ok(_result);
            }
        }
    }
}

