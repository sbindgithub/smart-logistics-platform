using SmartLogistics.Application.Common.Exceptions;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace SmartLogistics.Api.Middleware;

public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionMiddleware> logger,
    IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            await WriteError(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (ConflictException ex)
        {
            await WriteError(context, HttpStatusCode.Conflict, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            var message = _env.IsDevelopment()
                ? ex.Message
                : "An unexpected error occurred.";

            var details = _env.IsDevelopment()
                ? ex.StackTrace
                : null;

            await WriteError(
                context,
                HttpStatusCode.InternalServerError,
                message,
                details);
        }

    }

    private static async Task WriteError(
    HttpContext context,
    HttpStatusCode statusCode,
    string message,
    string? details = null)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        await context.Response.WriteAsJsonAsync(new
        {
            error = message,
            details,
            traceId = Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier
        });
    }

}
