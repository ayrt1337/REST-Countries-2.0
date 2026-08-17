using System.Net;
using System.Text.Json;
using Countries.Application.DTOs.Responses;

namespace Countries.WebApp.Bootstrap;

public sealed class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json; charset=utf-8";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var errorResponse = ResultResponse<object?>.CreateInternalServerError(
            message: "An internal server error occurred. Please try again later."
        );

        var json = JsonSerializer.Serialize(errorResponse, SerializerOptions);
        await context.Response.WriteAsync(json);
    }
}
