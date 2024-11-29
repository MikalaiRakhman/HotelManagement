namespace HotelManagement.Application.Bookings.Queries.DTOs
{
	public class BookingDetailsDTO
	{
		public DateOnly StartDate { get; set; }
		public DateOnly EndDate { get; set; }
		public int TotalPrice { get; set; }
		public string BookerFullName { get; set; }
		public string RoomDetails { get; set; }
	}
}
