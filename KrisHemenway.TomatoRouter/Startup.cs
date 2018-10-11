using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Quartz;
using Quartz.Impl;

namespace KrisHemenway.TomatoRouter
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
			Scheduler.ScheduleJob(RefreshDailyBandwidthJob.CreateJob(), RefreshDailyBandwidthJob.CreateTrigger());
			Scheduler.Start(applicationLifetime.ApplicationStopping);
		}

		private void FixJsonCamelCasing(JsonSerializerSettings settings)
		{
			// this unsets the default behavior (camelCase); what you see is what you get is now default
			if (settings.ContractResolver is DefaultContractResolver resolver)
			{
				resolver.NamingStrategy = null;
			}
		}

		public static IScheduler Scheduler { get; private set; }
	}
}
