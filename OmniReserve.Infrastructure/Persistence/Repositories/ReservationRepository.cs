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

    public Task<Reservation?> GetReservationWithDetailsAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}