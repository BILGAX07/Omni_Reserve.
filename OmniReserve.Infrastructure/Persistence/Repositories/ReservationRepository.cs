using Microsoft.EntityFrameworkCore;
using OmniReserve.Application.Common.Interfaces;
using OmniReserve.Domain.Entities;

namespace OmniReserve.Infrastructure.Persistence.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly ApplicationDbContext _context;

    public ReservationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Reservation?> GetReservationWithDetailsAsync(Guid id)
    {
        return await _context.Reservations
            .Include(reservation => reservation.User)
            .Include(reservation => reservation.Room)
            .FirstOrDefaultAsync(reservation => reservation.Id == id);
    }
    
    // Verifica si existe alguna reservación que se superponga con el rango solicitado.
    public async Task<bool> IsRoomAvailableAsync(
    Guid roomId,
    DateTime start,
    DateTime end)
{
    return !await _context.Reservations
        .AnyAsync(r =>
            r.RoomId == roomId &&
            r.CheckInDate < end &&
            r.CheckOutDate > start);
}
}