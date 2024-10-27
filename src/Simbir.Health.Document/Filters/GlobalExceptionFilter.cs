using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Simbir.Health.Document.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Simbir.Health.Document.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var statusCode = context.Exception switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                ServiceUnavailableException => StatusCodes.Status503ServiceUnavailable,
                ApiException or ValidationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            context.Result = new ObjectResult(new
            {
                error = context.Exception.Message
            })
            {
                StatusCode = statusCode
            };
        }
    }
}
