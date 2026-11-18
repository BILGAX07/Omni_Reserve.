
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using OmniReserve.Application.Rooms.Commands.CreateRoom;
using OmniReserve.Application.Rooms.Queries.GetAllRooms;
using OmniReserve.Application.Rooms.Queries.GetRoomById;

namespace OmniReserve.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("GlobalLimit")]
public class RoomsController : ControllerBase
{
    private readonly ISender _sender;

    public RoomsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoom(
        CreateRoomCommand command)
    {
        var resultado = await _sender.Send(command);

        return Ok(resultado);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRooms(
        CancellationToken cancellationToken)
    {
        var query = new GetAllRoomsQuery();

        var result = await _sender.Send(
            query,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoom(Guid id)
    {
        var query = new GetRoomByIdQuery
        {
            RoomId = id
        };

        var result = await _sender.Send(query);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}
