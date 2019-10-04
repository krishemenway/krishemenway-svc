using KrisHemenway.Notification.Reminders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;

namespace KrisHemenway.Notification
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvcCore().AddJsonOptions(FixJsonCamelCasing);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostApplicationLifetime hostApplicationLifetime)
		{
			app.UseRouting();
			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

			Scheduler = new StdSchedulerFactory().GetScheduler().Result;
			Scheduler.ScheduleJob(ReminderJob.CreateJob(), ReminderJob.CreateTrigger());
			Scheduler.Start(hostApplicationLifetime.ApplicationStopping);
		}

		private void FixJsonCamelCasing(JsonOptions options)
		{
			options.JsonSerializerOptions.PropertyNamingPolicy = null;
		}

		public static IScheduler Scheduler { get; private set; }
	}
}
