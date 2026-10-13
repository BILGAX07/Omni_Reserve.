using FluentValidation;

namespace OmniReserve.Application.Rooms.Queries.GetRoomById;

public class GetRoomByIdQueryValidator
    : AbstractValidator<GetRoomByIdQuery>
{
    public GetRoomByIdQueryValidator()
    {
    }
}