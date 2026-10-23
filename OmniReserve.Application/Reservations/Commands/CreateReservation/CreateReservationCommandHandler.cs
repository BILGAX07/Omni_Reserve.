using MediatR;
using OmniReserve.Application.Common.Interfaces;
using OmniReserve.Domain.Entities;

namespace OmniReserve.Application.Reservations.Commands.CreateReservation;

public class CreateReservationCommandHandler
    : IRequestHandler<CreateReservationCommand, Guid>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IRoomRepository _roomRepository;

    public CreateReservationCommandHandler(
        IReservationRepository reservationRepository,
        IRoomRepository roomRepository)
    {
        _reservationRepository = reservationRepository;
        _roomRepository = roomRepository;
    }

    public async Task<Guid> Handle(
        CreateReservationCommand request,
        CancellationToken cancellationToken)
    {
        bool isAvailable =
            await _reservationRepository.IsRoomAvailableAsync(
                request.RoomId,
                request.StartDate,
                request.EndDate);

        if (!isAvailable)
        {
            throw new Exception(
                "La habitación seleccionada ya está ocupada en el rango de fechas solicitado.");
        }

        var room = await _roomRepository.GetByIdAsync(request.RoomId);

        if (room is null)
        {
            throw new Exception("La habitación no existe.");
        }

        var reservation = new Reservation(
            request.UserId,
            request.RoomId,
            request.StartDate,
            request.EndDate,
            room.PricePerNight);

        await _reservationRepository.AddAsync(reservation);

        return reservation.Id;
    }
}