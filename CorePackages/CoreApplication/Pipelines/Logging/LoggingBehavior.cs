using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CoreCrossingCuttingConcerns.Logging;
using CoreCrossingCuttingConcerns.SeriLog;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CoreApplication.Pipelines.Logging
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, ILoggableRequest
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LoggerServiceBase _loggerServiceBase;

        public LoggingBehavior(IHttpContextAccessor httpContextAccessor, LoggerServiceBase loggerServiceBase)
        {
            _httpContextAccessor = httpContextAccessor; 
            _loggerServiceBase = loggerServiceBase;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            List<LogParameter> logParameters = new()
            {
                new LogParameter {Type=request.GetType().Name,Value=request}
            };

            LogDetail logDetail = new LogDetail()
            {
                MethodName = next.Method.Name,
                LogParameters = logParameters,
                User = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "?"
            };

            _loggerServiceBase.Verbose(JsonSerializer.Serialize(logDetail));
            return await next();
        }
    }
}
