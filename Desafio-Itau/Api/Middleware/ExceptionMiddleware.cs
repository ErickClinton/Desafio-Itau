using System.Text.Json;
using DesafioInvestimentosItau.Api.Models;
using DesafioInvestimentosItau.Application.Exceptions;

namespace DesafioInvestimentosItau.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            int statusCode = StatusCodes.Status500InternalServerError;
            string errorMessage = "Error intern in server.";
            string? details = ex.Message;

            if (ex is ApiException apiEx)
            {
                statusCode = apiEx.StatusCode;
                errorMessage = apiEx.Message;
                details = null; 
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var errorResponse = new ErrorResponse
            {
                StatusCode = statusCode,
                Path = context.Request.Path,
                Error = errorMessage,
                Details = ex is ApiException ? null : "An unexpected error occurred. Please contact support.",
                Timestamp = DateTime.UtcNow
            };

            var json = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(json);
        }
    }
}