using HotelManagement.Domain.Enums;

namespace HotelManagement.Application.Rooms.Queries
{
	public class RoomDTO
    {
		public Guid Id { get; set; }
		public int RoomNumber { get; set; }
		public RoomType RoomType { get; set; }
		public int PricePerNight { get; set; }
		public bool IsAvailable { get; set; }
	}
}
