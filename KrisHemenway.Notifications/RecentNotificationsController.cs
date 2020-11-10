using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KrisHemenway.Notifications
{
	[Route("public/api")]
	public class RecentNotificationsController : ControllerBase
	{
		[HttpGet("recent")]
		public async Task<ActionResult<RecentNotificationsResponse>> FindRecentNotifications([FromQuery] DateTime? fromTime)
		{
			var truncatedSinceTime = new[] { fromTime ?? DefaultSinceTime, DateTime.Now.Subtract(MaximumTimePeriodToRecall) }.Max();

			return new RecentNotificationsResponse
			{
				Notifications = await new NotificationStore().FindAll(truncatedSinceTime),
			};
		}

		public static DateTime DefaultSinceTime => DateTime.Now.AddDays(30);
		public static TimeSpan MaximumTimePeriodToRecall { get; } = TimeSpan.FromDays(60);
	}

	public class RecentNotificationsResponse
	{
		public IReadOnlyList<Notification> Notifications { get; set; }
	}
}
