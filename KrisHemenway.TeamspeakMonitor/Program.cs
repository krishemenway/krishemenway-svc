using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace KrisHemenway.TeamspeakMonitor
{
	public class Program
	{
		public static void Main(string[] args)
		{
			new WebHostBuilder()
				.UseKestrel()
				.UseConfiguration(Settings.Configuration)
				.UseStartup<Startup>()
				.UseUrls($"http://*:{Settings.WebPort}")
				.Build()
				.Run();
		}

		public static Settings Settings = new Settings();
	}
}
