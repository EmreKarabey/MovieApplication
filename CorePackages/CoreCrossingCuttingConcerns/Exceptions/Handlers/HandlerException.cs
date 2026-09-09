using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCrossingCuttingConcerns.Exceptions.Extensions;
using CoreCrossingCuttingConcerns.Exceptions.HttpProblemDetails;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using Microsoft.AspNetCore.Http;

namespace CoreCrossingCuttingConcerns.Exceptions.Handlers
{
    public class HttpExceptionHandler : ExceptionHandler
    {
        private HttpResponse? _responseMessage;

        public HttpResponse responseMessage
        {
            get => _responseMessage ?? throw new ArgumentNullException(nameof(_responseMessage));
            set=>_responseMessage = value;
        }
        protected override Task HandlerException(BusinessException businessException)
        {
            responseMessage.StatusCode = StatusCodes.Status400BadRequest;
            string details = new BusinessProblemDetails(businessException.Message).AsJson();
            return responseMessage.WriteAsync(details);
        }

        protected override Task HandlerException(ValidationException validationException)
        {
            responseMessage.StatusCode = StatusCodes.Status400BadRequest;
            string details = new ValidationProblemDetails(validationException.Message).AsJson();
            return responseMessage.WriteAsync(details);
        }

        protected override Task HandlerException(Exception exception)
        {
            responseMessage.StatusCode = StatusCodes.Status400BadRequest;
            string details = new InternalProblemDetails(exception.Message).AsJson();
            return responseMessage.WriteAsync(details);
        }
    }
}
