using MediatR;

namespace OmniReserve.Application.Reservations.Queries.GetAllReservations;

public record GetAllReservationsQuery
    : IRequest<List<ReservationDto>>;