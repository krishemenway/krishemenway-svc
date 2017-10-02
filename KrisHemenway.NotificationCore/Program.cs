using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace KrisHemenway.NotificationCore
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddEnvironmentVariables()
				.Build();

			var host = new WebHostBuilder()
				.UseKestrel()
				.UseConfiguration(Configuration)
				.UseContentRoot(ExecutablePath)
				.UseStartup<Startup>()
				.UseApplicationInsights()
				.UseUrls($"http://*:{WebPort}")
				.Build();

			host.Run();
		}

		public static int WebPort => Configuration.GetValue<int>("WebPort");
		public static IConfigurationRoot Configuration { get; private set; }

		public static string ExecutablePath => Directory.GetCurrentDirectory();
	}
}
