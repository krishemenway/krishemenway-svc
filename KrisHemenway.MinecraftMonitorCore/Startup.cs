using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;

namespace KrisHemenway.MinecraftMonitor
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();

			Configuration = builder.Build();
		}

		public IConfigurationRoot Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// Add framework services.
			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
		{
			app.UseMvc();

			Scheduler = new StdSchedulerFactory().GetScheduler().Result;

			foreach (var serverInfo in new ServerInfoStore().Find())
			{
				new MinecraftStatusStore().Save(serverInfo, MinecraftStatus.Default);
				Scheduler.ScheduleJob(MinecraftServerMonitorJob.CreateJob(serverInfo), MinecraftServerMonitorJob.CreateTrigger(serverInfo));
			}

			Scheduler.Start(applicationLifetime.ApplicationStopping);
		}

		public static IScheduler Scheduler { get; private set; }
	}
}
