using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Diagnostics;
using System.IO;

namespace KrisHemenway.TVShows
{
	public class Program
	{
		public static int Main(string[] args)
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(ExecutablePath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddEnvironmentVariables()
				.AddCommandLine(args)
				.Build();

			Settings = new Settings(configuration);

			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.WriteTo.File(Settings.LogFile, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10)
				.CreateLogger();

			try
			{
				Host.CreateDefaultBuilder(args)
					.UseSerilog()
					.ConfigureWebHostDefaults(webHost =>
					{
						webHost.UseKestrel();
						webHost.UseConfiguration(configuration);
						webHost.UseStartup<Startup>();
						webHost.UseUrls($"http://*:{configuration.GetValue<int>("WebPort")}");
					})
					.Build()
					.Run();

				return 0;
			}
			catch (Exception exception)
			{
				Log.Fatal(exception, "Service terminated unexpectedly");
				return 1;
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		public static string ExecutablePath { get; set; } = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

		public static Settings Settings { get; internal set; }
	}
}
