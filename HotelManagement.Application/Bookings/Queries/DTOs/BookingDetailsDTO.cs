using HotelManagement.Domain.Common;

namespace HotelManagement.Application.Bookings.Queries.DTOs
{
	public class BookingDetailsDTO
	{
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int TotalPrice { get; set; }
		public string BookerFullName { get; set; }
		public string RoomDetails { get; set; }
	}
}
