using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace KrisHemenway.TomatoRouter
{
	public interface ITomatoRouterHttpClient
	{
		IReadOnlyList<DailyBandwidthUsage> LatestDailyBandwidthUsage();
	}

	public class TomatoRouterHttpClient : ITomatoRouterHttpClient
	{
		public IReadOnlyList<DailyBandwidthUsage> LatestDailyBandwidthUsage()
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", AuthorizationHeader);

				var requestTask = client
					.GetAsync(CreatePath("/bwm-daily.asp"))
					.ContinueWith(task =>
					{
						if (task.IsFaulted || task.IsCanceled)
						{
							throw new UnreachableRouterException();
						}

						return task.Result;
					});

				var response = requestTask.Result;

				if (response.StatusCode == HttpStatusCode.Unauthorized)
				{
					throw new InvalidRouterCredentialsException();
				}

				var header = "Date,TotalKilobytesDownloaded,TotalKilobytesUploaded";
				var responseAsString = response.Content.ReadAsStringAsync().Result.Replace("<pre>", header).Replace(" </pre>\r\n\r\n", "");

				using (var stringReader = new StringReader(responseAsString))
				using (var csvParser = new CsvParser(stringReader))
				using (var csvReader = new CsvReader(csvParser))
				{
					return csvReader.GetRecords<DailyBandwidthUsage>().ToList();
				}
			}
		}

		private Uri CreatePath(string path)
		{
			return new Uri($"http://{Program.Settings.RouterHost}:{Program.Settings.RouterPort}{path}");
		}

		private string AuthorizationHeader => Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Program.Settings.RouterUser}:{Program.Settings.RouterPassword}"));
	}

	public class DailyBandwidthUsage
	{
		public DateTime Date { get; set; }
		public Int64 TotalKilobytesDownloaded { get; set; }
		public Int64 TotalKilobytesUploaded { get; set; }
	}
}
