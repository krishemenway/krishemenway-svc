﻿using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using Serilog.Events;
using System.Diagnostics;

namespace KrisHemenway.Notifications
{
	public class Program
	{
		public static void Main(string[] args)
		{
			SetupLogging();

			try
			{
				SetupConfiguration();
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

		private static void SetupConfiguration()
		{
			Configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddEnvironmentVariables()
				.Build();
		}

		private static void SetupLogging()
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.WriteTo.File("notifications.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10)
				.CreateLogger();
		}

		private static void StartWebHost()
		{
			WebHost = new WebHostBuilder()
				.UseKestrel()
				.UseConfiguration(Configuration)
				.UseStartup<Startup>()
				.UseSerilog()
				.UseUrls($"http://*:{Configuration.GetValue<int>("WebPort")}")
				.Build();

			WebHost.Run();
		}

		public static IConfigurationRoot Configuration { get; private set; }
		public static IWebHost WebHost { get; private set; }

		public static string ExecutablePath { get; set; } = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
	}
}
