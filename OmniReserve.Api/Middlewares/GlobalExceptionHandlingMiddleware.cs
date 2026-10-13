using Microsoft.AspNetCore.Mvc;
using ValidationException =
    OmniReserve.Application.Common.Exceptions.ValidationException;

namespace OmniReserve.Api.Middlewares;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlingMiddleware> logger)
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
            _logger.LogError(
                ex,
                "Ocurrió un error durante la solicitud.");

            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        if (exception is ValidationException validationEx)
        {
            context.Response.StatusCode =
                StatusCodes.Status400BadRequest;

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Error de Validación"
            };

            problemDetails.Extensions.Add(
                "errors",
                validationEx.Errors);

            await context.Response.WriteAsJsonAsync(problemDetails);

            return;
        }

        context.Response.StatusCode =
            StatusCodes.Status500InternalServerError;

        var genericProblemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Error interno del servidor",
            Detail = "Ocurrió un error inesperado."
        };

        await context.Response.WriteAsJsonAsync(
            genericProblemDetails);
    }
}