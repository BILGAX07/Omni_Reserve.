using MediatR;

namespace OmniReserve.Application.Reservations.Commands.CreateReservation;

public record CreateReservationCommand(
    Guid UserId,
    Guid RoomId,
    DateTime StartDate,
    DateTime EndDate
) : IRequest<Guid>;