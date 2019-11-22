using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace KrisHemenway.Notifications
{
	[Route("public/api")]
	public class RecentNotificationsController : ControllerBase
	{
		[HttpGet("recent")]
		public ActionResult<RecentNotificationsResponse> FindRecentNotifications([FromQuery] DateTime fromTime)
		{
			return new RecentNotificationsResponse
			{
				Notifications = new NotificationStore().FindAll(fromTime)
			};
		}

		public class RecentNotificationsResponse
		{
			public IReadOnlyList<Notification> Notifications { get; set; }
		}
	}
}
