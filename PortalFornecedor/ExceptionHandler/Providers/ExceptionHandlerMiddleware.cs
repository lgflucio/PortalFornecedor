using ExceptionHandler.DTOs;
using ExceptionHandler.Extensions;
using ExceptionHandler.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ExceptionHandler.Providers
{
    public static class ExceptionHandlerMiddleware
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = async context =>
                {
                    IExceptionHandlerPathFeature _exceptionHandler = context.Features.Get<IExceptionHandlerPathFeature>();
                    if (_exceptionHandler == null)
                        return;

                    
                    ApiErrorCodes _errorCode = _exceptionHandler.Error is ApiException
                                                ? ((ApiException)_exceptionHandler.Error).ApiErrorCode
                                                : ApiErrorCodes.UNEXPC;

                    IEnumerable<string> _exceptionMessages = new List<string> { _exceptionHandler.Error.Message };
                    string _requestData = string.Empty;

                    int _httpStatusCode = StatusCodes.Status500InternalServerError;

                    _requestData = _exceptionHandler.Error is ApiException
                                                 ? ((ApiException)_exceptionHandler.Error).RequestData?.ToJson()
                                                 : string.Empty;

                    if (_exceptionHandler.Error is WebException && (_exceptionHandler.Error as WebException)?.Response != null)
                    {
                        HttpWebResponse _httpWebResponse = (_exceptionHandler.Error as WebException)?.Response as HttpWebResponse;

                        using (Stream responseStream = _httpWebResponse.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
                            {
                                reader.ReadToEnd().TryDeserializeJson(out ExceptionDetailsDTO _exception);

                                _exceptionMessages = _exception?.Messages ?? new List<string> { _exception?.Message };
                                _httpStatusCode = _exception != null
                                    ? _exception.HttpStatusCode
                                    : (int)_httpWebResponse.StatusCode;

                            }
                        }
                    }
                    else if (_exceptionHandler.Error is ApiException _exception)
                    {
                        _httpStatusCode = (int)_exception.StatusCode;
                        IEnumerable<string> _customMessages = ((ApiException)_exceptionHandler.Error)?.Messages;
                        _exceptionMessages = _customMessages ?? new List<string> { _exceptionHandler.Error.Message };
                    }
                    else
                        _exceptionMessages = new List<string> { _exceptionHandler.Error.Message };

                    context.Response.StatusCode = _httpStatusCode;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(new ExceptionDetailsDTO
                    {
                        Code = _errorCode.ToString(),
                        HttpStatusCode = _httpStatusCode,
                        Messages = _exceptionMessages
                    }.ToJson());
                }
            });
        }
    }
}