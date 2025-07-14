using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Application.Features.Auth.Exceptions;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace MusicApp.Infrastructure.Middleware
{
    /// <summary>
    /// Catches all unhandled exceptions in the request pipeline
    /// and converts them into appropriate HTTP responses with RFC-7807 ProblemDetails.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Maps known exception types to HTTP status codes and ProblemDetails,
        /// then writes the JSON response.
        /// </summary>
        private static Task HandleExceptionAsync(HttpContext ctx, Exception ex)
        {
            // Determine status code based on exception type
            var status = ex switch
            {
                ClientAppUrlNotConfiguredException _ => HttpStatusCode.InternalServerError,
                InvalidConfirmationCodeException _ => HttpStatusCode.BadRequest,
                EmailConfirmationFailedException _ => HttpStatusCode.BadRequest,
                UserUpdateFailedException _ => HttpStatusCode.BadRequest,
                UserNotFoundException _ => HttpStatusCode.NotFound,
                InvalidCredentialsException _ => HttpStatusCode.Unauthorized,
                UnauthorizedActionException _ => HttpStatusCode.Unauthorized,
                InvalidRefreshTokenException _ => HttpStatusCode.Unauthorized,
                EmailNotConfirmedException _ => HttpStatusCode.Forbidden,
                EmailAlreadyConfirmedException _ => HttpStatusCode.Conflict,
                InvalidOperationException => HttpStatusCode.Conflict,
                FileNotFoundException => HttpStatusCode.NotFound,
                ArgumentException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };

            var problem = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = ex.Message,
                Status = (int)status,
                Detail = ex is AggregateException agg
                         ? string.Join("; ", agg.InnerExceptions.Select(i => i.Message))
                         : null
            };


            var result = JsonSerializer.Serialize(problem);

            ctx.Response.ContentType = "application/problem+json";
            ctx.Response.StatusCode = (int)status;
            return ctx.Response.WriteAsync(result);
        }
    }
}
