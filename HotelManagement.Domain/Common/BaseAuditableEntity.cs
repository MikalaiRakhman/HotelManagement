
namespace HotelManagement.Domain.Common
{
	public abstract class BaseAuditableEntity : BaseEntity
	{
		public DateTime CreatedAt { get; set; }		
		public DateTime LastModifiedAt { get; set; }		
	}
}
