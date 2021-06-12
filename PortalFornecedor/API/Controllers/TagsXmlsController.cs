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
    [Route("[controller]")]
    [ApiController]
    public class TagsXmlsController : ControllerBase
    {
        //private readonly ITagsXmlsAppService _service;
        //public TagsXmlsController(ITagsXmlsAppService service)
        //{
        //    _service = service;
        //}

        //[Authorize]
        //[HttpPost]
        //[ProducesResponseType(typeof(TagsXmlsViewModel), StatusCodes.Status200OK)]
        //public IActionResult Post(TagsXmlsViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        throw new ApiException(ModelState.GetAllErrorsToString(), ApiErrorCodes.MODNOTVALD);

        //    return Ok(_service.Post(model));
        //}
    }
}
