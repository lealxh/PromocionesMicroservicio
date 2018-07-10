using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Promociones.Domain.Core;

namespace Promociones.Presentation.Api.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var code = HttpStatusCode.InternalServerError;

            if (context.Exception is EntityNotFoundException)
            {
                code = HttpStatusCode.NotFound;
            }
            if (context.Exception is InvalidOperationException || context.Exception is ArgumentException)
            {
                code = HttpStatusCode.BadRequest ;
            }
            if (context.Exception is NotImplementedException)
            {
                code = HttpStatusCode.NotImplemented;
            }
            if (context.Exception is TimeoutException)
            {
                code = HttpStatusCode.ServiceUnavailable;
            }

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)code;
            context.Result = new JsonResult(new
            {
                error = new[] { context.Exception.Message },
                stackTrace = context.Exception.StackTrace
            });
        }
    }
}
