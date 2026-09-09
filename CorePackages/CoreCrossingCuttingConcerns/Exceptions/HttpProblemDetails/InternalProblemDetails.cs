using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreCrossingCuttingConcerns.Exceptions.HttpProblemDetails
{
    public class InternalProblemDetails:ProblemDetails
    {
        public InternalProblemDetails(string message)
        {
            Title = "Internal Server Error";
            Detail = "Internal Server Error";
            Status = StatusCodes.Status400BadRequest;
            Type= "http://example.com";
        }
    }
}
