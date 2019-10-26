using Dapper;
using KrisHemenway.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KrisHemenway.Notification.SentNotifications
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
				FROM public.notification
				WHERE sent_time > @AfterTime
				ORDER BY sent_time DESC";

			using (var connection = Database.CreateConnection())
			{
				return connection
					.Query<SentNotification>(sql, new { AfterTime = afterTime ?? DateTime.MinValue })
					.Select(BuildSentNotification)
					.ToList();
			}
		}

		public SentNotification CreateNotification(PushNotificationDetails details)
		{
			const string sql = @"
				INSERT INTO public.notification
				(sent_time, title, content, type_name)
				VALUES
				(@SentTime, @Title, @Content, @TypeName)";

			using (var connection = Database.CreateConnection())
			{
				var notification = new SentNotification
				{
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
}
