using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace MyShop.API.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger
)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[+] Unhandled exception");

            context.Response.StatusCode = ex switch
            {
                TimeoutException => StatusCodes.Status504GatewayTimeout,
                DbUpdateException or MySqlException => StatusCodes.Status503ServiceUnavailable,
                _ => StatusCodes.Status500InternalServerError
            };

            var response = new
            {
                error = "Something went wrong :c",
                detail = ex.Message
            };

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
