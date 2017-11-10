using KrisHemenway.Common;

namespace KrisHemenway.TeamspeakMonitor
{
	public class TeamspeakNotificationSender
	{
		public TeamspeakNotificationSender()
		{
			_pushNotificationSender = new PushNotificationSender();
		}

		public void SendPushNotification(TeamspeakUser user)
		{
			var details = new PushNotificationDetails
			{
				Title = "Teamspeak",
				Content = $"{user.NickName} joined Teamspeak",
				TypeName = "Teamspeak"
			};

			_pushNotificationSender.NotifyAll(details);
		}

		private readonly PushNotificationSender _pushNotificationSender;
	}
}
