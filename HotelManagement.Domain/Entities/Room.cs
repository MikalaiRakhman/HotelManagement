using HotelManagement.Domain.Common;
using HotelManagement.Domain.Entities.Enums;

namespace HotelManagement.Domain.Entities
{
	public class Room : BaseEntity 
	{
		public int RoomNumber { get; set; }
		public RoomType RoomType { get; set; }
		public int PricePerNight { get; set; }
		public bool IsAvailable { get; set; }
	}
}
