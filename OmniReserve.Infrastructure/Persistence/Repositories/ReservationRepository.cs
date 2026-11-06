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

    public async Task<bool> IsRoomAvailableAsync(
        Guid roomId,
        DateTime start,
        DateTime end)
    {
        return !await _context.Reservations
            .AnyAsync(reservation =>
                reservation.RoomId == roomId &&
                reservation.CheckInDate < end &&
                reservation.CheckOutDate > start);
    }

    public async Task<List<Reservation>> GetAllAsync()
    {
        return await _context.Reservations
            .Include(reservation => reservation.User)
            .Include(reservation => reservation.Room)
            .ToListAsync();
    }

    public async Task AddAsync(Reservation reservation)
    {
        await _context.Reservations.AddAsync(reservation);
        await _context.SaveChangesAsync();
    }
}