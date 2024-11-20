using HotelManagement.Domain.Common;

namespace HotelManagement.Application.Bookings.Queries.DTOs
{
	public class BookingDTO : BaseEntity
	{
		public DateOnly StartDate { get; set; }
		public DateOnly EndDate { get; set; }
		public int TotalPrice { get; set; }
	}
}
