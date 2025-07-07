using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Application.Users.Commands;

namespace MyShop.API.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger
)
{
    public async Task Invoke(
        HttpContext context
    )
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[+] Unhandled exception");
            if (context.Response.HasStarted) throw;

            context.Response.ContentType = "application/json";

            switch (ex)
            {
                // --- 400: VALIDACIÓN ---problem
                case FluentValidation.ValidationException fvEx:
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;

                        // Agrupamos errores por propiedad
                        var errors = fvEx.Errors
                            .GroupBy(e => e.PropertyName)
                            .SelectMany(e => e.Select(x => x.ErrorMessage));

                        await context.Response.WriteAsJsonAsync(new
                        {
                            success = false,
                            message = string.Join(",", errors)
                        });
                        break;
                    }

                // --- 409: DUPLICADO ---
                case DuplicateEmailException dupEx:
                    {
                        context.Response.StatusCode = StatusCodes.Status409Conflict;
                        await context.Response.WriteAsJsonAsync(new
                        {
                            success = true,
                            message = dupEx.Message
                        });
                        break;
                    }

                // --- 503/504: BD ---
                case TimeoutException:
                case DbUpdateException:
                case MySqlConnector.MySqlException:
                    context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        success = false,
                        message = "Base de datos no disponible"
                    });
                    break;

                // --- 500: TODO LO DEMÁS ---
                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        success = false,
                        message = "Ocurrió un error inesperado"
                    });
                    break;
            }
        }
    }
}
