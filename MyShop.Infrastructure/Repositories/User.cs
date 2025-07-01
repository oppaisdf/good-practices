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
}