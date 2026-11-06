
using MediatR;
using OmniReserve.Application.Common.Interfaces;

namespace OmniReserve.Application.Rooms.Queries.GetAllRooms;

public class GetAllRoomsQueryHandler
    : IRequestHandler<GetAllRoomsQuery, List<RoomDto>>
{
    private readonly IRoomRepository _roomRepository;

    public GetAllRoomsQueryHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<List<RoomDto>> Handle(
        GetAllRoomsQuery request,
        CancellationToken cancellationToken)
    {
        var rooms = await _roomRepository.GetAllAsync();

        var response = rooms.Select(room => new RoomDto
        {
            Id = room.Id,
            RoomNumber = room.RoomNumber,
            Type = room.Type.ToString(),
            Price = room.PricePerNight,
            IsAvailable = room.IsAvailable
        }).ToList();

        return response;
    }
}
