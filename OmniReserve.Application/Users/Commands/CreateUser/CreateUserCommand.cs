using MediatR;

namespace OmniReserve.Application.Users.Commands.CreateUser;

public record CreateUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password
) : IRequest<Guid>;