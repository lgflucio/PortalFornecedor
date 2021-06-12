using ExceptionHandler.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Net;

namespace ExceptionHandler.Providers
{
    public class ApiVersionExceptionHandler : DefaultErrorResponseProvider
    {
        private const string UnsupportedApiVersionError = "UnsupportedApiVersion";

        public override IActionResult CreateResponse(ErrorResponseContext context)
        {
            switch (context.ErrorCode)
            {
                case UnsupportedApiVersionError:
                    throw new ApiException("Versão da Api não suportada", HttpStatusCode.BadRequest);
                default:
                    throw new ApiException("Erro no versionamento da Api", HttpStatusCode.BadRequest);
            }
        }
    }
}
