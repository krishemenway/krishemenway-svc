using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using KrisHemenway.TVShowsCore.Jobs;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Concurrent;

namespace KrisHemenway.TVShowsCore
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvcCore().AddJsonFormatters(FixJsonCamelCasing);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime)
		{
			loggerFactory.AddConsole(Program.Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			app.UseMvc();

			Scheduler = new StdSchedulerFactory().GetScheduler().Result;
			Scheduler.ScheduleJob(RefreshTVShowsJob.CreateJob(), RefreshTVShowsJob.CreateTrigger());
			Scheduler.ScheduleJob(TVReleasingTodayJob.CreateJob(), TVReleasingTodayJob.CreateTrigger());
			Scheduler.ScheduleJob(ThisJustInJob.CreateJob(), ThisJustInJob.CreateTrigger());
			Scheduler.Start(applicationLifetime.ApplicationStopping);
		}

		private void FixJsonCamelCasing(JsonSerializerSettings settings)
		{
			if (settings.ContractResolver is DefaultContractResolver resolver)
			{
				resolver.NamingStrategy = null;  // <<!-- this removes the camelcasing
			}
		}

		public static IScheduler Scheduler { get; private set; }
	}

	//public static class FileLogExtensions
	//{
	//	public static ILoggerFactory AddFile(this ILoggerFactory loggerFactory)
	//	{
	//		loggerFactory.AddProvider(new FileLogProvider());
	//		return loggerFactory;
	//	}
	//}

	//public class FileLogProvider : ILoggerProvider
	//{
	//	public ILogger CreateLogger(string categoryName)
	//	{
	//		return _fileLogger = new FileLogger();
	//	}

	//	public void Dispose()
	//	{
	//		_fileLogger.Dispose();
	//	}

	//	private IFileLogger _fileLogger;
	//}

	//public interface IFileLogger : ILogger, IDisposable { }

	//public class FileLogger : IFileLogger
	//{
	//	public FileLogger()
	//	{
	//		_logFileStreamStore = new LogFileStreamStore();
	//	}

	//	public IDisposable BeginScope<TState>(TState state)
	//	{
	//		return this;
	//	}

	//	public void Dispose()
	//	{
	//	}

	//	public bool IsEnabled(LogLevel logLevel)
	//	{
	//		return true;
	//	}

	//	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
	//	{
	//		_logFileStreamStore.FindOrCreate(CurrentLogPath).WriteLine($"{DateTime.UtcNow.ToString("o")} {formatter(state, exception)}");
	//	}

	//	private LogFileStreamStore _logFileStreamStore;

	//	private string CurrentLogPath => Path.Combine(Directory.GetCurrentDirectory(), $"KrisHemenway.TVShowsCore-{DateTime.UtcNow.ToString("yyyy-MM-dd")}.log");
	//}

	//public class LogFileStreamStore : IDisposable
	//{
	//	static LogFileStreamStore()
	//	{

	//	}

	//	public StreamWriter FindOrCreate(string path)
	//	{
	//		return StreamsByPath.GetOrAdd(path, (p) => new Lazy<StreamWriter>(() => File.AppendText(p))).Value;
	//	}

	//	public void Dispose()
	//	{
	//		foreach (var stream in StreamsByPath.Values)
	//		{
	//			stream.Value.Dispose();
	//		}
	//	}

	//	private static ConcurrentDictionary<string, Lazy<StreamWriter>> StreamsByPath { get; set; } = new ConcurrentDictionary<string, Lazy<StreamWriter>>();
	//}
}
