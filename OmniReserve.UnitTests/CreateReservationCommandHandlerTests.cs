using NSubstitute;
using OmniReserve.Application.Common.Interfaces;
using OmniReserve.Application.Reservations.Commands.CreateReservation;
using OmniReserve.Domain.Entities;
using OmniReserve.Domain.Enums;

namespace OmniReserve.UnitTests;

public class CreateReservationCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenValidCommand_ReturnsNewReservationId()
    {
        // Arrange
        var reservationRepoMock = Substitute.For<IReservationRepository>();
        var roomRepoMock = Substitute.For<IRoomRepository>();

        var roomId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        reservationRepoMock
            .IsRoomAvailableAsync(
                Arg.Any<Guid>(),
                Arg.Any<DateTime>(),
                Arg.Any<DateTime>())
            .Returns(true);

        var room = new Room("101", RoomType.Single, 100m);

        roomRepoMock
            .GetByIdAsync(roomId)
            .Returns(room);

        var handler = new CreateReservationCommandHandler(
            reservationRepoMock,
            roomRepoMock);

        var command = new CreateReservationCommand(
            userId,
            roomId,
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(3));

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);

            //asd

        // Assert
        Assert.NotEqual(Guid.Empty, result);

        await reservationRepoMock
            .Received(1)
            .AddAsync(Arg.Any<Reservation>());

        
    }
}