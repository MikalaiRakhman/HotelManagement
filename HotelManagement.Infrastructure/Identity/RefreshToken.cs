namespace HotelManagement.Infrastructure.Identity
{
    public class RefreshToken
    {
		public Guid Id { get; set; }
		public string Token { get; set; }
		public DateTime Expires { get; set; }
		public Guid ApplicationUserId { get; set; }
		public ApplicationUser ApplicationUser { get; set; }
    }
}