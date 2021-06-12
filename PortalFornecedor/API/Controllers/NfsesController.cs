using ExceptionHandler.Extensions;
using ExceptionHandler.Helpers;
using Microsoft.AspNetCore.Authorization;
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
    public class NfsesController : ControllerBase
    {
        //private readonly INfseAppService _service;
        //public NfsesController(INfseAppService service)
        //{
        //    _service = service;
        //}

        //[HttpGet]
        //[ProducesResponseType(typeof(NfseViewModel), StatusCodes.Status200OK)]
        //public IActionResult Get()
        //{
        //    if (!ModelState.IsValid)
        //        throw new ApiException(ModelState.GetAllErrorsToString(), ApiErrorCodes.MODNOTVALD);
        //    var ok = _service.Get();
        //    return Ok(_service.Get());
        //}

        //[HttpGet("{id}")]
        //[ProducesResponseType(typeof(NfseDetalhesViewModel), StatusCodes.Status200OK)]
        //public IActionResult Get(int id)
        //{
        //    if (!ModelState.IsValid)
        //        throw new ApiException(ModelState.GetAllErrorsToString(), ApiErrorCodes.MODNOTVALD);

        //    return Ok(_service.GetById(id));
        //}

        //[HttpPost]
        //[ProducesResponseType(typeof(NfseViewModel), StatusCodes.Status200OK)]
        //public IActionResult GetByFilters(NfseFiltersViewModel filters)
        //{
        //    if (!ModelState.IsValid)
        //        throw new ApiException(ModelState.GetAllErrorsToString(), ApiErrorCodes.MODNOTVALD);
        //    return Ok(_service.GetByFilters(filters));
        //}

    }
}
