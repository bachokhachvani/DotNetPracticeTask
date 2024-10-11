using System.Net;
using Serilog.Events;
using WebApplication1.Data.DTOs;

namespace WebApplication1.Exceptions.ExceptionMiddleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Serilog.ILogger _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, Serilog.ILogger logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            _logger.Write(LogEventLevel.Information, "Request: {Method} {Path}", context.Request.Method,
                context.Request.Path);
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.Write(LogEventLevel.Error, exception, exception.Message);
        ExceptionResponseDTO response = exception switch
        {
            ApplicationException _ => new ExceptionResponseDTO(HttpStatusCode.BadRequest,
                "Application exception occurred."),
            KeyNotFoundException _ => new ExceptionResponseDTO(HttpStatusCode.NotFound, "The request key not found."),
            UnauthorizedAccessException _ => new ExceptionResponseDTO(HttpStatusCode.Unauthorized, "Unauthorized."),
            UserNotFoundException ex => new ExceptionResponseDTO(HttpStatusCode.NotFound, ex.Message),
            ArgumentException ex=>new ExceptionResponseDTO(HttpStatusCode.BadRequest, ex.Message),
            _ => new ExceptionResponseDTO(HttpStatusCode.InternalServerError,
                "Internal server error. Please retry later.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)response.StatusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}