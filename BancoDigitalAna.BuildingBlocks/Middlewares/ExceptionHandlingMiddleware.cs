using BancoDigitalAna.BuildingBlocks.Infrastructure;
using BancoDigitalAna.BuildingBlocks.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BancoDigitalAna.BuildingBlocks.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BaseException ex)
            {
                await HandleBaseException(context, ex);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task HandleBaseException (HttpContext context, BaseException exception)
        {
            _logger.LogError(exception, "[ERROR]: {Message}", exception.Message);

            var statusCode = exception.StatusCode;
            var response = new ErrorResponse(exception.Message, exception.ErrorCode);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
