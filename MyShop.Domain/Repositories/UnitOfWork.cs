namespace MyShop.Domain.Repositories;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken ct = default);
}
