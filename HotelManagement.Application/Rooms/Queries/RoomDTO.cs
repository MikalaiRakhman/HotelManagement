using HotelManagement.Domain.Common;
using HotelManagement.Domain.Entities.Enums;

namespace HotelManagement.Application.Rooms.Queries
{
    public class RoomDTO : BaseEntity
    {
		public int RoomNumber { get; set; }
		public RoomType RoomType { get; set; }
		public int PricePerNight { get; set; }
		public bool IsAvailable { get; set; }
		public ICollection<Guid> BookingsId { get; set; }
	}
}
