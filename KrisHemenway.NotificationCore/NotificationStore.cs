using Dapper;
using KrisHemenway.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KrisHemenway.NotificationCore
{
	public interface INotificationStore
	{
		IReadOnlyList<SentNotification> FindAll(DateTime? afterTime = null);
		SentNotification CreateNotification(PushNotificationDetails details);
	}

	public class NotificationStore : INotificationStore
	{
		public IReadOnlyList<SentNotification> FindAll(DateTime? afterTime = null)
		{
			const string sql = @"
				SELECT
					notification_id as notificationid,
					sent_time as senttime,
					title,
					content,
					type_name as typename
				FROM push.notification
				WHERE sent_time > @AfterTime
				ORDER BY sent_time DESC";

			using (var connection = Database.CreateConnection())
			{
				return connection.Query<SentNotification>(sql, new { AfterTime = afterTime ?? DateTime.MinValue }).Select(BuildSentNotification).ToList();
			}
		}

		public SentNotification CreateNotification(PushNotificationDetails details)
		{
			const string sql = @"
				INSERT INTO push.notification
				(notification_id, sent_time, title, content, type_name)
				VALUES
				(@NotificationId, @SentTime, @Title, @Content, @TypeName)";

			using (var connection = Database.CreateConnection())
			{
				var notification = new SentNotification
				{
					NotificationId = Guid.NewGuid(),
					Title = details.Title,
					Content = details.Content,
					SentTime = DateTime.Now,
					TypeName = details.TypeName
				};

				connection.Execute(sql, notification);

				return notification;
			}
		}

		private SentNotification BuildSentNotification(SentNotification notification)
		{
			notification.SentTime = DateTime.SpecifyKind(notification.SentTime, DateTimeKind.Local);
			return notification;
		}
	}

	public class SentNotification
	{
		public Guid NotificationId { get; set; }

		public string Title { get; set; }
		public string Content { get; set; }

		public string TypeName { get; set; }
		public DateTime SentTime { get; set; }
	}
}
