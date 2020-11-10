using Dapper;
using KrisHemenway.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace KrisHemenway.Notifications
{
	public interface INotificationStore
	{
		Task<IReadOnlyList<Notification>> FindAll(DateTime? afterTime = null);
		Notification CreateNotification(PushNotificationDetails details);
	}

	public class NotificationStore : INotificationStore
	{
		public async Task<IReadOnlyList<Notification>> FindAll(DateTime? afterTime = null)
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
				return await connection
					.QueryAsync<Notification>(sql, new { AfterTime = afterTime ?? DateTime.MinValue })
					.ContinueWith((result) => (IReadOnlyList<Notification>)result.Result.Select(BuildSentNotification).ToList());
			}
		}

		public Notification CreateNotification(PushNotificationDetails details)
		{
			const string sql = @"
				INSERT INTO public.notification
				(sent_time, title, content, type_name)
				VALUES
				(@SentTime, @Title, @Content, @TypeName)";

			using (var connection = Database.CreateConnection())
			{
				var notification = new Notification
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

		private Notification BuildSentNotification(Notification notification)
		{
			notification.SentTime = DateTime.SpecifyKind(notification.SentTime, DateTimeKind.Local);
			return notification;
		}
	}
}
