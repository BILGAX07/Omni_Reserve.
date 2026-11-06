using MediatR;
using OmniReserve.Application.Common.Interfaces;

namespace OmniReserve.Application.Reservations.Queries.GetAllReservations;

public class GetAllReservationsQueryHandler
    : IRequestHandler<GetAllReservationsQuery, List<ReservationDto>>
{
    private readonly IReservationRepository _reservationRepository;

    public GetAllReservationsQueryHandler(
        IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public async Task<List<ReservationDto>> Handle(
        GetAllReservationsQuery request,
        CancellationToken cancellationToken)
    {
        var reservations =
            await _reservationRepository.GetAllAsync();

        var response = reservations.Select(reservation =>
            new ReservationDto
            {
                Id = reservation.Id,
                UserEmail = reservation.User.Email.Value,
                RoomNumber = reservation.Room.RoomNumber,
                StartDate = reservation.CheckInDate,
                EndDate = reservation.CheckOutDate
            }).ToList();

        return response;
    }
}