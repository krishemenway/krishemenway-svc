using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KrisHemenway.Notifications
{
	public interface IFirebaseNotificationSender
	{
		void NotifyAll(Notification notification);
	}

	public class FirebaseNotificationSender : IFirebaseNotificationSender
	{
		public async void NotifyAll(Notification notification)
		{
			var buildMessageJson = new FCMPushNotification<NotificationData>
				{
					To = "/topics/allDevices",
					Notification = new FCMNotificationOptions
					{
						Title = notification.Title,
						Body = notification.Content
					},
					Data = new NotificationData
					{
						TypeName = notification.TypeName,
						SentTime = notification.SentTime,
						NotificationId = notification.NotificationId.ToString()
					}
				};

			var webRequest = WebRequest.CreateHttp("https://fcm.googleapis.com/fcm/send");
			webRequest.Headers["Authorization"] = $"key={FirebaseServerKey}";
			webRequest.Method = "POST";
			webRequest.ContentType = "application/json; charset=UTF-8";

			using (var requestStream = await webRequest.GetRequestStreamAsync())
			{
				var requestBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(buildMessageJson));
				requestStream.Write(requestBytes, 0, requestBytes.Length);
			}

			using (var response = await webRequest.GetResponseAsync())
			using (var responseStream = new StreamReader(response.GetResponseStream()))
			{
				Log.Information("Notification for {NotificationType} response from FCM: {Response}", notification.TypeName, responseStream.ReadToEnd());
			}
		}

		private static string FirebaseServerKey
		{
			get { return Program.Configuration.GetValue<string>("FirebaseKey"); }
		}
	}

	public class NotificationData
	{
		public string TypeName { get; set; }
		public string NotificationId { get; set; }
		public DateTime SentTime { get; set; }
	}
	
	public class FCMNotificationOptions
	{
		[JsonPropertyName("body")]
		public string Body { get; set; }

		[JsonPropertyName("title")]
		public string Title { get; set; }

		[JsonPropertyName("icon")]
		public string Icon { get; set; }

		[JsonPropertyName("color")]
		public string Color { get; set; }

		[JsonPropertyName("click_action")]
		public string ClickAction { get; set; }

		[JsonPropertyName("tag")]
		public string Tag { get; set; }
	}

	public class FCMPushNotification<T>
	{
		[JsonPropertyName("to")]
		public string To { get; set; }

		[JsonPropertyName("data")]
		public T Data { get; set; }

		[JsonPropertyName("notification")]
		public FCMNotificationOptions Notification { get; set; }
	}
}
