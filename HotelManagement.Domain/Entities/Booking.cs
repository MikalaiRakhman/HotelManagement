using HotelManagement.Domain.Common;

namespace HotelManagement.Domain.Entities
{
	public class Booking : BaseEntity
	{
		public Guid UserId { get; set; }
		public Guid RoomId { get; set; }

		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int TotalPrice {  get; set; }		
	}
}
