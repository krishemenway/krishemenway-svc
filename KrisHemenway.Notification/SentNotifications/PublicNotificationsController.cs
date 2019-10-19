using KrisHemenway.Notification.PushNotifications;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace KrisHemenway.Notification.SentNotifications
{
	[Route("public/api")]
	public class PublicNotificationsController : ControllerBase
	{
		[HttpGet("all")]
		public ActionResult<NotificationsResponse> FindAllNotifications()
		{
			return new NotificationsResponse
			{
				Notifications = new NotificationStore().FindAll()
			};
		}

		[HttpGet("recent")]
		public ActionResult<NotificationsResponse> FindRecentNotifications([FromQuery] DateTime fromTime)
		{
			return new NotificationsResponse
			{
				Notifications = new NotificationStore().FindAll(fromTime)
			};
		}

		[HttpPost("add")]
		public OkResult Add([FromQuery]string deviceToken)
		{
			new PushRecipientStore().SaveRecipient(deviceToken);
			return Ok();
		}
	}

	public class NotificationsResponse
	{
		public IReadOnlyList<SentNotification> Notifications { get; set; }
	}
}
