using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace KrisHemenway.Common
{
	public interface IPushNotificationSender
	{
		void NotifyAll(PushNotificationDetails pushNotificationDetails);
	}

	public class PushNotificationDetails
	{
		public string Title { get; set; }
		public string TypeName { get; set; }
		public int AlertNumber { get; set; }
		public string Content { get; set; }
	}

	public class PushNotificationSender : IPushNotificationSender
	{
		public PushNotificationSender(HttpClient httpClient = null)
		{
			_httpClient = httpClient ?? new HttpClient();
		}

		public void NotifyAll(PushNotificationDetails details)
		{
			var content = new StringContent(JsonSerializer.Serialize(details), Encoding.UTF8, "application/json");
			_httpClient.PostAsync("http://localhost:8105/internal/api/notifications/send", content);
		}

		private readonly HttpClient _httpClient;
	}
}
