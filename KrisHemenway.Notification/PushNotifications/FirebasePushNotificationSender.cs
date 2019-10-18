using KrisHemenway.Notification.SentNotifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace KrisHemenway.Notification.PushNotifications
{
	public interface IFirebasePushNotificationSender
	{
		void NotifyAll(SentNotification notification);
	}

	public class FirebasePushNotificationSender : IFirebasePushNotificationSender
	{
		public async void NotifyAll(SentNotification notification)
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
				var requestBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(buildMessageJson));
				requestStream.Write(requestBytes, 0, requestBytes.Length);
			}

			using (var response = await webRequest.GetResponseAsync())
			using (var responseStream = new StreamReader(response.GetResponseStream()))
			{
				Log.LogInformation($"Notification for {notification.TypeName} response from FCM: {responseStream.ReadToEnd()}");
			}
		}

		private static string FirebaseServerKey
		{
			get { return Program.Configuration.GetValue<string>("FirebaseKey"); }
		}

		private readonly ILogger<FirebasePushNotificationSender> Log = new LoggerFactory().CreateLogger<FirebasePushNotificationSender>();
	}

	public class NotificationData
	{
		public string TypeName { get; set; }
		public string NotificationId { get; set; }
		public DateTime SentTime { get; set; }
	}
	
	public class FCMNotificationOptions
	{
		[JsonProperty(PropertyName = "body")]
		public string Body { get; set; }

		[JsonProperty(PropertyName = "title")]
		public string Title { get; set; }

		[JsonProperty(PropertyName = "icon")]
		public string Icon { get; set; }

		[JsonProperty(PropertyName = "color")]
		public string Color { get; set; }

		[JsonProperty(PropertyName = "click_action")]
		public string ClickAction { get; set; }

		[JsonProperty(PropertyName = "tag")]
		public string Tag { get; set; }
	}

	public class FCMPushNotification<T>
	{
		[JsonProperty(PropertyName = "to")]
		public string To { get; set; }

		[JsonProperty(PropertyName = "data")]
		public T Data { get; set; }

		[JsonProperty(PropertyName = "notification")]
		public FCMNotificationOptions Notification { get; set; }
	}
}
