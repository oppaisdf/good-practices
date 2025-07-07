using MyShop.Domain.Repositories;

namespace MyShop.Infrastructure.Persistence;

public class UnitOfWork(Context ctx) : IUnitOfWork
{
    public Task CommitAsync(CancellationToken ct = default)
    => ctx.SaveChangesAsync(ct);
}