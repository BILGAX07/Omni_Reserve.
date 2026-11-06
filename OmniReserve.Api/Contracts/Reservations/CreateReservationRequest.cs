namespace OmniReserve.Api.Contracts.Reservations;

public class CreateReservationRequest
{
    public Guid RoomId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}