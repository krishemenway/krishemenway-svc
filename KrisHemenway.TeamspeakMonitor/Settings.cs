using Microsoft.Extensions.Configuration;
using System.IO;

namespace KrisHemenway.TeamspeakMonitor
{
	public class Settings
	{
		public Settings()
		{
			Configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddEnvironmentVariables()
				.Build();
		}

		public string TeamspeakQueryHost => Configuration.GetValue<string>("TeamspeakQueryHost");
		public ushort TeamspeakQueryPort => Configuration.GetValue<ushort>("TeamspeakQueryPort");

		public string TeamspeakQueryUser => Configuration.GetValue<string>("TeamspeakQueryUser");
		public string TeamspeakQueryPassword => Configuration.GetValue<string>("TeamspeakQueryPassword");

		public int WebPort => Configuration.GetValue<int>("WebPort");

		public IConfigurationRoot Configuration { get; private set; }
	}
}
