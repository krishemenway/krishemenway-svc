using Microsoft.AspNetCore.Mvc;

namespace KrisHemenway.NotificationCore
{
	[Route("public/api")]
	public class PublicNotificationsController : Controller
	{
		[HttpGet("all")]
		public IActionResult FindAllNotifications()
		{
			return Ok(new { Notifications = new NotificationStore().FindAll() });
		}

		[HttpPost("add")]
		public IActionResult Add([FromQuery]string deviceToken)
		{
			new PushRecipientStore().SaveRecipient(deviceToken);
			return Ok();
		}
	}
}
