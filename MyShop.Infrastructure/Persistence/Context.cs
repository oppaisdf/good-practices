using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;

namespace MyShop.Infrastructure.Persistence;

public class Context(
    DbContextOptions<Context> opts
) : DbContext(opts)
{
    public DbSet<User> Users => Set<User>();
}