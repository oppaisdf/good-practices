using MediatR;
using MyShop.Domain.Repositories;

namespace MyShop.Application.Users.Queries;

public record GetUsersQuery : IRequest<IReadOnlyList<UserDto>>;
public record UserDto(Guid Id, string Email, string FullName);

// Handler
public class GetUsersQueryHandler(IUserRepository repo)
        : IRequestHandler<GetUsersQuery, IReadOnlyList<UserDto>>
{
    public async Task<IReadOnlyList<UserDto>> Handle(
        GetUsersQuery q, CancellationToken ct)
    {
        var users = await repo.ToListAsync(ct).ConfigureAwait(false);
        return [.. users.Select(u => new UserDto(u.Id, u.Email, u.FullName))];
    }
}
