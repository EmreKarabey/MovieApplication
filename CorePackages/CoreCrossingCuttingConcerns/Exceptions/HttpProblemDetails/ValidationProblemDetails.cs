using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreCrossingCuttingConcerns.Exceptions.HttpProblemDetails
{
    public class ValidationProblemDetails:ProblemDetails
    {
        public ValidationProblemDetails(string message)
        {
            Title = "Rule Violation";
            Detail = message;
            Status = StatusCodes.Status400BadRequest;
            Type = "http://example.com";
        }
    }
}
