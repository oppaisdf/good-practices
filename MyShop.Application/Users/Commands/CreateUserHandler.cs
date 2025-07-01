using MyShop.Domain.Repositories;
using MyShop.Domain.Entities;
using MediatR;

namespace MyShop.Application.Users.Commands;

public class CreateUserCommandHandler(IUserRepository repo, IUnitOfWork uow)
        : IRequestHandler<CreateUserCommand, Guid>
{
    public async Task<Guid> Handle(CreateUserCommand cmd, CancellationToken ct)
    {
        // Regla de negocio: email único
        if (await repo.ExistsByEmailAsync(cmd.Email, ct))
            throw new DuplicateEmailException(cmd.Email);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = cmd.Email,
            FullName = cmd.FullName
        };

        await repo.AddAsync(user, ct);
        await uow.CommitAsync(ct);

        return user.Id;
    }
}

// Excepción de dominio simple
public class DuplicateEmailException(string email)
        : Exception($"User with email '{email}' already exists.");
