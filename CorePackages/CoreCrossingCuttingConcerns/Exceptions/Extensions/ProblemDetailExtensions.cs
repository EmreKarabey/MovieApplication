using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoreCrossingCuttingConcerns.Exceptions.Extensions
{
    public static class ProblemDetailExtensions
    {
        public static string AsJson<TProblemDetail>(this TProblemDetail problemDetail) where TProblemDetail: ProblemDetails=> JsonSerializer.Serialize(problemDetail);
    }
}
