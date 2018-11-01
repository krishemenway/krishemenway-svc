using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using System;
using System.IO;

namespace KrisHemenway.TVShows
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddEnvironmentVariables()
				.AddCommandLine(args)
				.Build();

			Settings = new Settings(configuration);

			SetupLogging();

			try
			{
				StartWebHost(configuration);
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

		private static void StartWebHost(IConfigurationRoot configuration)
		{
			WebHost = new WebHostBuilder()
				.UseKestrel()
				.UseConfiguration(configuration)
				.UseStartup<Startup>()
				.UseSerilog()
				.UseUrls($"http://*:{Settings.WebPort}")
				.Build();

			WebHost.Run();
		}

		public static Settings Settings { get; private set; }
		public static IWebHost WebHost { get; private set; }
	}
}
