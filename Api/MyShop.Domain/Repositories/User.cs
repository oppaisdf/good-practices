namespace MyShop.Domain.Repositories;

public interface IUserRepository
{
    Task<IReadOnlyList<Entities.User>> ToListAsync(CancellationToken ct = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);
    Task AddAsync(Entities.User user, CancellationToken ct = default);
}