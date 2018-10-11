using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Quartz;
using Quartz.Impl;

namespace KrisHemenway.NotificationCore
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
			app.UseMvc();

			Scheduler = new StdSchedulerFactory().GetScheduler().Result;
			Scheduler.ScheduleJob(ReminderJob.CreateJob(), ReminderJob.CreateTrigger());
			Scheduler.Start(applicationLifetime.ApplicationStopping);
		}

		private void FixJsonCamelCasing(JsonSerializerSettings settings)
		{
			var resolver = settings.ContractResolver;
			if (resolver != null)
			{
				var res = resolver as DefaultContractResolver;
				res.NamingStrategy = null;  // <<!-- this removes the camelcasing
			}
		}

		public static IScheduler Scheduler { get; private set; }
	}
}
