using HotelManagement.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace HotelManagement.Infrastructure.Data.Interceotors
{
	public class AuditableEntityInterceptor : SaveChangesInterceptor
	{
		private readonly TimeProvider _dateTime;

		public AuditableEntityInterceptor(TimeProvider dateTime)
		{
			_dateTime = dateTime;			
		}

		public override InterceptionResult<int> SavingChanges (DbContextEventData eventData, InterceptionResult<int> result)
		{
			UpdateEntities(eventData.Context);

			return base.SavingChanges (eventData, result);
		}

		public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
		{
			UpdateEntities(eventData.Context);

			return await base.SavingChangesAsync(eventData, result, cancellationToken);
		}

		public void UpdateEntities(DbContext? context)
		{
			if (context == null)
			{
				return;
			}

			foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
			{
				if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities() )
				{
					var utsNow = _dateTime.GetUtcNow();

					if (entry.State == EntityState.Added)
					{
						entry.Entity.CreatedAt = utsNow.DateTime;
					}

					entry.Entity.LastModifiedAt = utsNow.DateTime;
				}				
			}			
		}		
	}

	public static class Extensions
	{
		public static bool HasChangedOwnedEntities(this EntityEntry entry)
		{
			return entry.References.Any(r =>
			r.TargetEntry != null &&
			r.TargetEntry.Metadata.IsOwned() &&
			(r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
		}
	}
}
