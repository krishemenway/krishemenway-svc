using KrisHemenway.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KrisHemenway.Notifications
{
	[Route("internal/api/notifications")]
	public class SendNotificationController : ControllerBase
	{
		public SendNotificationController(
			INotificationStore notificationStore = null,
			IFirebaseNotificationSender firebasePushNotificationSender = null)
		{
			_notificationStore = notificationStore ?? new NotificationStore();
			_firebasePusnNotificationSender = firebasePushNotificationSender ?? new FirebaseNotificationSender();
		}

		[HttpPost("send")]
		public async Task<ActionResult<Result>> SendNotification([FromBody] PushNotificationDetails detailsRequest)
		{
			var notification = await _notificationStore.CreateNotification(detailsRequest);
			_firebasePusnNotificationSender.NotifyAll(notification);

			return Result.Successful;
		}

		private readonly INotificationStore _notificationStore;
		private readonly IFirebaseNotificationSender _firebasePusnNotificationSender;
	}
}