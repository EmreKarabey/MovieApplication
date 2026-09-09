using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CoreCrossingCuttingConcerns.Exceptions.Handlers;
using CoreCrossingCuttingConcerns.Logging;
using CoreCrossingCuttingConcerns.SeriLog;
using Microsoft.AspNetCore.Http;

namespace CoreCrossingCuttingConcerns.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HttpExceptionHandler _handlers;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LoggerServiceBase _loggerServiceBase;

        public ExceptionMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor, LoggerServiceBase loggerServiceBase)
        {
            _next = next;
            _handlers = new HttpExceptionHandler();
            _httpContextAccessor = httpContextAccessor;
            _loggerServiceBase = loggerServiceBase;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await LogException(context, exception);
                await HandlerExceptionAsync(context.Response, exception);
            }
        }

        private Task LogException(HttpContext httpContent, Exception exception)
        {
            List<LogParameter> logParameters = new()
            {
                new LogParameter
                {
                    Type=httpContent.GetType().Name,
                    Value=exception.ToString()
                }
            };

            LogDetailWithException logDetailWithException = new()
            {
                ExceptionMessage = exception.Message,
                MethodName = _next.Method.Name,
                LogParameters = logParameters,
                User = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "?"
            };

            _loggerServiceBase.Error(JsonSerializer.Serialize(logDetailWithException));

            return Task.CompletedTask;
        }

        private Task HandlerExceptionAsync(HttpResponse response, Exception exception)
        {
            response.ContentType = "application/json";
            _handlers.responseMessage = response;
            return _handlers.HandlerExceptionAsync(exception);
        }
    }
}
