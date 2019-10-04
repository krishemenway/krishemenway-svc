using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;
using Serilog;

namespace KrisHemenway.MinecraftMonitor
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddOptions();
			services.AddMvc();
			services.Configure<Settings>(Program.Configuration);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostApplicationLifetime applicationLifetime, IOptions<Settings> options)
		{
			app.UseRouting();
			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

			Scheduler = new StdSchedulerFactory().GetScheduler().Result;

			var servers = new ServerInfoStore(options).Find();
			Log.Information("Servers: {Servers}", servers);

			foreach (var serverInfo in servers)
			{
				new MinecraftStatusStore().Save(serverInfo, MinecraftStatus.Default);
				Scheduler.ScheduleJob(MinecraftServerMonitorJob.CreateJob(serverInfo), MinecraftServerMonitorJob.CreateTrigger(serverInfo));
			}

			Scheduler.Start(applicationLifetime.ApplicationStopping);
		}

		public static IScheduler Scheduler { get; private set; }
	}
}
