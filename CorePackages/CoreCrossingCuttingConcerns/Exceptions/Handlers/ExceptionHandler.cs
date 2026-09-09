using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;

namespace CoreCrossingCuttingConcerns.Exceptions.Handlers
{
    public abstract class ExceptionHandler
    {
        public Task HandlerExceptionAsync(Exception exception)=>
            exception switch
        {
            BusinessException businessException=>HandlerException(businessException),
            ValidationException validationException => HandlerException(validationException),
            _=>HandlerException(exception)
        };

        protected abstract Task HandlerException(BusinessException businessException);
        protected abstract Task HandlerException(ValidationException validationException);
        protected abstract Task HandlerException(Exception exception);
    }
}
