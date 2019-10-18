using StronglyTyped.GuidIds;
using System;

namespace KrisHemenway.Notification.SentNotifications
{
	public class SentNotification
	{
		public Id<SentNotification> NotificationId { get; set; }

		public string Title { get; set; }
		public string Content { get; set; }

		public string TypeName { get; set; }
		public DateTime SentTime { get; set; }
	}
}
