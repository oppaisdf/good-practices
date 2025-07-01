namespace MyShop.Domain.Repositories;

public interface IUserRepository
{
    Task<IReadOnlyList<Entities.User>> ToListAsync(CancellationToken ct = default);
}