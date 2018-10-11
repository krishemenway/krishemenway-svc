using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;
using System;

namespace KrisHemenway.TVShows
{
	public class Program
	{
		public static void Main(string[] args)
		{
			SetupLogging();

			try
			{
				StartWebHost();
			}
			catch (Exception exception)
			{
				Log.Fatal(exception, "Service terminated unexpectedly");
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		private static void SetupLogging()
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.WriteTo.RollingFile(Settings.LogFile)
				.CreateLogger();
		}

		private static void StartWebHost()
		{
			WebHost = new WebHostBuilder()
				.UseKestrel()
				.UseConfiguration(Settings.Configuration)
				.UseStartup<Startup>()
				.UseSerilog()
				.UseUrls($"http://*:{Settings.WebPort}")
				.Build();

			WebHost.Run();
		}

		public static Settings Settings { get; private set; } = new Settings();
		public static IWebHost WebHost { get; private set; }
	}
}
