using KrisHemenway.Common;

namespace KrisHemenway.NotificationCore
{
	public class SendNotificationRequestHandler
	{
		public SendNotificationRequestHandler(
			INotificationStore notificationStore = null,
			IFirebasePushNotificationSender firebasePushNotificationSender = null)
		{
			_notificationStore = notificationStore ?? new NotificationStore();
			_firebasePusnNotificationSender = firebasePushNotificationSender ?? new FirebasePushNotificationSender();
		}

		public Result HandleRequest(PushNotificationDetails detailsRequest)
		{
			var notification = _notificationStore.CreateNotification(detailsRequest);
			_firebasePusnNotificationSender.NotifyAll(notification);
			return Result.Successful;
		}

		private readonly INotificationStore _notificationStore;
		private readonly IFirebasePushNotificationSender _firebasePusnNotificationSender;
	}
}