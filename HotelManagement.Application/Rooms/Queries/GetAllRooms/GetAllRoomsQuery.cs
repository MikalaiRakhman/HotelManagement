using MediatR;

namespace HotelManagement.Application.Rooms.Queries.GetAllRooms
{
	public record class GetAllRoomsQuery : IRequest<List<RoomDTO>>
	{
	}
}