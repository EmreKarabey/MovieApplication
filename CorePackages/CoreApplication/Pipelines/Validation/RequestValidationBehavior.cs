using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCrossingCuttingConcerns.Exceptions.TypeOf;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using ValidationException = CoreCrossingCuttingConcerns.Exceptions.TypeOf.ValidationException;

namespace CoreApplication.Pipelines.Validation
{
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            ValidationContext<object> validationContext = new(request);

            IEnumerable<ValidationExceptionModel> validationExceptions = _validators.
                Select(n => n.Validate(validationContext)).
                SelectMany(n => n.Errors).Where(failure => failure != null).
                GroupBy(keySelector: p => p.PropertyName,
                resultSelector: (propertyName, errors) => new ValidationExceptionModel
                {
                    Property = propertyName,
                    Errors = errors.Select(n => n.ErrorMessage)
                }).ToList();

            if (validationExceptions.Any()) throw new ValidationException(validationExceptions);

            TResponse response = await next();

            return response;
        }
    }
}
