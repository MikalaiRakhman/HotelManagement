using HotelManagement.Domain.Common;

namespace HotelManagement.Domain.Entities
{
	public class Booking : BaseEntity
	{
		public DateOnly StartDate { get; set; }
		public DateOnly EndDate { get; set; }
		public int TotalPrice {  get; set; }
		public Guid UserId { get; set; }
		public User User { get; set; }
		public Guid RoomId { get; set; }
		public Room Room { get; set; }
	}
}
