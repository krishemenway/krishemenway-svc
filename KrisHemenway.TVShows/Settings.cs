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

		public string User => Configuration.GetValue<string>("PushServiceUser");
		public string Password => Configuration.GetValue<string>("PushServicePassword");

		public string Host => Configuration.GetValue<string>("PushServiceHost");
		public string DatabaseName => Configuration.GetValue<string>("DatabaseName");

		public IReadOnlyList<string> FileExtensions => Configuration.GetValue<string[]>("VideoFileExtensions");

		public string ExecutablePath => Directory.GetCurrentDirectory();

		public static IConfigurationRoot Configuration { get; private set; }
	}
}
