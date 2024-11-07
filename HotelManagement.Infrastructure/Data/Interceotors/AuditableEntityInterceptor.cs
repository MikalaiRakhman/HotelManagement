using HotelManagement.Application.Common;
using HotelManagement.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace HotelManagement.Infrastructure.Data.Interceotors
{
	public class AuditableEntityInterceptor : SaveChangesInterceptor
	{
		private readonly TimeProvider _dateTime;
		private readonly IUser _user;

		public AuditableEntityInterceptor(TimeProvider dateTime, IUser user)
		{
			_dateTime = dateTime;
			_user = user;
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
