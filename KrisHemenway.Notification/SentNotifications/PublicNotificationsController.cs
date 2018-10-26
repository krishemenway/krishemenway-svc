using KrisHemenway.Notification.PushNotifications;
using Microsoft.AspNetCore.Mvc;
using System;

namespace KrisHemenway.Notification.SentNotifications
{
	[Route("public/api")]
	public class PublicNotificationsController : Controller
	{
		[HttpGet("all")]
		public IActionResult FindAllNotifications()
		{
			return Ok(new { Notifications = new NotificationStore().FindAll() });
		}

		[HttpGet("recent")]
		public IActionResult FindRecentNotifications([FromQuery] DateTime fromTime)
		{
			return Ok(new { Notifications = new NotificationStore().FindAll(fromTime) });
		}

		[HttpPost("add")]
		public IActionResult Add([FromQuery]string deviceToken)
		{
			new PushRecipientStore().SaveRecipient(deviceToken);
			return Ok();
		}
	}
}
