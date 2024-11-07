using HotelManagement.Domain.Common;

namespace HotelManagement.Application.Bookings.Queries
{
	public class BookingDTO : BaseEntity
	{
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int TotalPrice { get; set; }
		public Guid UserId { get; set; }		
		public Guid RoomId { get; set; }		
	}
}
