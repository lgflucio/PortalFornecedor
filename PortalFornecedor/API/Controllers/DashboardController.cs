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
    public class DashboardController : ControllerBase
    {
        //private readonly IDashboardAppService _service;
        //public DashboardController(IDashboardAppService service)
        //{
        //    _service = service;
        //}

        //[HttpGet]
        //[ProducesResponseType(typeof(MunicipiosViewModel), StatusCodes.Status200OK)]
        //public IActionResult Get()
        //{
        //    if (!ModelState.IsValid)
        //        throw new ApiException(ModelState.GetAllErrorsToString(), ApiErrorCodes.MODNOTVALD);
        //    return Ok(_service.Get());
        //}

    }
}
