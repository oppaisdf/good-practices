using MediatR;

namespace MyShop.Application.Users.Commands;

public record CreateUserCommand(string Email, string FullName) : IRequest<Guid>;
