namespace HotelManagement.Application.Bookings.Queries.DTOs
{
	public class BookingDTO
	{
		public Guid Id { get; set; }
		public DateOnly StartDate { get; set; }
		public DateOnly EndDate { get; set; }
		public int TotalPrice { get; set; }
		public string BookerEmail { get; set; }
		public int RoomNumber { get; set; }
	}
}
