using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Net;

namespace ExceptionHandler.Extensions
{
    public class ApiException : Exception
    {
        public ApiException() { }


        public ApiException(string message,
               HttpStatusCode statusCode = HttpStatusCode.InternalServerError,
               Exception innerException = null
               )
           : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        public ApiException(ApiErrorCodes apiErrorCode = ApiErrorCodes.UNEXPC, object requestData = null)
        {
            ApiErrorCode = apiErrorCode;
            RequestData = requestData;
        }
        public ApiException(IEnumerable<string> messages, object requestData = null)
        {
            Messages = messages;
            ApiErrorCode = ApiErrorCodes.UNEXPC;
            RequestData = requestData;
        }

        public ApiException(
            string message,
            ApiErrorCodes apiErrorCode = ApiErrorCodes.UNEXPC,
            Exception innerException = null
        )
            : base(message, innerException)
        {
            ApiErrorCode = apiErrorCode;
            Message = message;
        }

        public string Message { get; set; }

        public ApiErrorCodes ApiErrorCode { get; set; }

        public object RequestData { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public IEnumerable<string> Messages { get; set; }
    }
}
