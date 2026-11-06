namespace OmniReserve.Application.Reservations.Queries.GetAllReservations;

public class ReservationDto
{
    public Guid Id { get; set; }

    public string UserEmail { get; set; } = string.Empty;

    public string RoomNumber { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}