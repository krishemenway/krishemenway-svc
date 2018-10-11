using Microsoft.Extensions.Configuration;
using System.IO;

namespace KrisHemenway.TomatoRouter
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

		public int WebPort => Configuration.GetValue<int>("WebPort");
		public string LogFile => Configuration.GetValue<string>("LogFile");

		public string DatabaseUser => Configuration.GetValue<string>("TomatoRouterDatabaseUser");
		public string DatabasePassword => Configuration.GetValue<string>("TomatoRouterDatabasePassword");

		public string Host => Configuration.GetValue<string>("TomatoRouterDatabaseHost");
		public string DatabaseName => Configuration.GetValue<string>("TomatoRouterDatabaseName");

		public string RouterHost => Configuration.GetValue<string>("TomatoRouterHost");
		public string RouterPort => Configuration.GetValue<string>("TomatoRouterPort");

		public string RouterUser => Configuration.GetValue<string>("TomatoRouterUser");
		public string RouterPassword => Configuration.GetValue<string>("TomatoRouterPassword");

		public string ExecutablePath => Directory.GetCurrentDirectory();

		public static IConfigurationRoot Configuration { get; private set; }
	}
}
