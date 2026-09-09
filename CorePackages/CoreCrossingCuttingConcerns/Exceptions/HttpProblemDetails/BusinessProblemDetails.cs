using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreCrossingCuttingConcerns.Exceptions.HttpProblemDetails
{
    public class BusinessProblemDetails : ProblemDetails
    {
        public BusinessProblemDetails(string message)
        {
            Type = "Rule Violation";
            Detail = message;
            Status = StatusCodes.Status400BadRequest;
            Type = "http://example.com";
        }
    }
}
