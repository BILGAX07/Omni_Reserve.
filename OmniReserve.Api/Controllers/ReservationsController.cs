using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmniReserve.Api.Contracts.Reservations;
using OmniReserve.Application.Reservations.Commands.CreateReservation;

namespace OmniReserve.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReservationsController : ControllerBase
{
    private readonly ISender _sender;

    public ReservationsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReservation(
        [FromBody] CreateReservationRequest request,
        CancellationToken cancellationToken)
    {
        var userIdClaim = HttpContext.User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var command = new CreateReservationCommand(
            userId,
            request.RoomId,
            request.StartDate,
            request.EndDate);

        var result = await _sender.Send(command, cancellationToken);

        return Ok(result);
    }
}