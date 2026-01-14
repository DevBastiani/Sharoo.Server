using Sharoo.Server.Domain.Exceptions;
using System.Net;

namespace Sharoo.Server.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new { message = exception.Message };

            return exception switch
            {
                UserNotFoundException =>
                    SetResponse(context, HttpStatusCode.NotFound, response),

                TodoNotFoundException =>
                    SetResponse(context, HttpStatusCode.NotFound, response),

                ArgumentException =>
                    SetResponse(context, HttpStatusCode.BadRequest, response),

                _ => SetResponse(context, HttpStatusCode.InternalServerError,
                    new { message = "Ocorreu um erro interno no servidor." })
            };
        }

        private static Task SetResponse(HttpContext context, HttpStatusCode statusCode, object response)
        {
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsJsonAsync(response);
        }
    }
}