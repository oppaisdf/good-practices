namespace MyShop.Domain.Repositories;

public interface IUserRepository
{
    Task<IReadOnlyList<User>> ToListAsync(CancellationToken ct = default);
}