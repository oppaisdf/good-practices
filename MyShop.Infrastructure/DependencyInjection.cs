using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyShop.Domain.Repositories;
using MyShop.Infrastructure.Persistence;
using MyShop.Infrastructure.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration config)
    {
        var conn = Environment.GetEnvironmentVariable("DefaultConnection")
               ?? config.GetConnectionString("DefaultConnection")
               ?? throw new InvalidOperationException("[+] Missing connection string.");
        services.AddDbContext<Context>(o => o.UseMySql(conn, ServerVersion.AutoDetect(conn)));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
