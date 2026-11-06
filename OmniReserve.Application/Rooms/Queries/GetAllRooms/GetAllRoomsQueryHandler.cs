
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using OmniReserve.Application.Common.Interfaces;

namespace OmniReserve.Application.Rooms.Queries.GetAllRooms;

public class GetAllRoomsQueryHandler
    : IRequestHandler<GetAllRoomsQuery, List<RoomDto>>
{
    private const string CACHE_KEY = "rooms_catalog";

    private readonly IRoomRepository _roomRepository;
    private readonly IDistributedCache _cache;

    public GetAllRoomsQueryHandler(
        IRoomRepository roomRepository,
        IDistributedCache cache)
    {
        _roomRepository = roomRepository;
        _cache = cache;
    }

    public async Task<List<RoomDto>> Handle(
        GetAllRoomsQuery request,
        CancellationToken cancellationToken)
    {
        var cachedRooms = await _cache.GetStringAsync(
            CACHE_KEY,
            cancellationToken);

        if (!string.IsNullOrEmpty(cachedRooms))
        {
            var cachedResponse =
                JsonSerializer.Deserialize<List<RoomDto>>(cachedRooms);

            if (cachedResponse is not null)
            {
                return cachedResponse;
            }
        }

        var rooms = await _roomRepository.GetAllAsync();

        var response = rooms.Select(room => new RoomDto
        {
            Id = room.Id,
            RoomNumber = room.RoomNumber,
            Type = room.Type.ToString(),
            Price = room.PricePerNight,
            IsAvailable = room.IsAvailable
        }).ToList();

        var serializedRooms = JsonSerializer.Serialize(response);

        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };

        await _cache.SetStringAsync(
            CACHE_KEY,
            serializedRooms,
            cacheOptions,
            cancellationToken);

        return response;
    }
}
