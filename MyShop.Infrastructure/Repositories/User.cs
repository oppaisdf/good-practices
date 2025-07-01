using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Repositories;
using MyShop.Domain.Entities;
using MyShop.Infrastructure.Persistence;

namespace MyShop.Infrastructure.Repositories;

public class UserRepository(
    Context ctx
) : IUserRepository
{
    public async Task<IReadOnlyList<User>> ToListAsync(
        CancellationToken ct = default
    ) => await ctx.Users
        .AsNoTracking()
        .ToListAsync(ct)
        .ConfigureAwait(false);

    public async Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken ct = default
    ) => await ctx.Users
        .AnyAsync(u => u.Email == email, ct)
        .ConfigureAwait(false);

    public async Task AddAsync(
        User user,
        CancellationToken ct = default
    ) => await ctx.Users
        .AddAsync(user, ct)
        .ConfigureAwait(false);
}