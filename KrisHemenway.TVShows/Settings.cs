using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;

namespace KrisHemenway.TVShows
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

		public string DatabaseUser => Configuration.GetValue<string>("TVShowDatabaseUser");
		public string DatabasePassword => Configuration.GetValue<string>("TVShowDatabasePassword");

		public string DatabaseHost => Configuration.GetValue<string>("TVShowDatabaseHost");
		public int DatabasePort => Configuration.GetValue<int>("TVShowDatabasePort");

		public string DatabaseName => Configuration.GetValue<string>("TVShowDatabaseName");

		public string ExecutablePath => Directory.GetCurrentDirectory();

		public static IConfigurationRoot Configuration { get; private set; }
	}
}
