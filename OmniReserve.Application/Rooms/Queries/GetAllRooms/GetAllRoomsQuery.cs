
using MediatR;

namespace OmniReserve.Application.Rooms.Queries.GetAllRooms;

public class GetAllRoomsQuery : IRequest<List<RoomDto>>
{
}
