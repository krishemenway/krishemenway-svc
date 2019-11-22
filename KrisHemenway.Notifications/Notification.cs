using StronglyTyped.GuidIds;
using System;

namespace KrisHemenway.Notifications
{
	public class Notification
	{
		public Id<Notification> NotificationId { get; set; }

		public string Title { get; set; }
		public string Content { get; set; }

		public string TypeName { get; set; }
		public DateTime SentTime { get; set; }
	}
}
