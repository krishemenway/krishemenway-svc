using KrisHemenway.Common;
using Microsoft.AspNetCore.Mvc;

namespace KrisHemenway.NotificationCore
{
	[Route("internal/api/notifications")]
	public class InternalNotificationsController : Controller
	{
		[HttpPost("send")]
		public IActionResult SendNotification([FromBody] PushNotificationDetails details)
		{
			var notification = new NotificationStore().CreateNotification(details);
			new FirebasePushNotificationSender().NotifyAll(notification);

			return Ok();
		}
	}
}
