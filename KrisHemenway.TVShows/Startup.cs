using KrisHemenway.TVShows.Jobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Quartz;
using Quartz.Impl;
using System;

namespace KrisHemenway.TVShows
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvcCore().AddJsonFormatters(FixJsonCamelCasing);

			services.AddDistributedMemoryCache();
			services.AddSession(options =>
			{
				// Set a short timeout for easy testing.
				options.IdleTimeout = TimeSpan.FromDays(1);
				options.Cookie.HttpOnly = true;
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime)
		{
			app.UseSession();
			app.UseMvc();

			Scheduler = new StdSchedulerFactory().GetScheduler().Result;
			Scheduler.ScheduleJob(RefreshTVShowsJob.CreateJob(), RefreshTVShowsJob.CreateTrigger());
			Scheduler.ScheduleJob(TVReleasingTodayJob.CreateJob(), TVReleasingTodayJob.CreateTrigger());
			Scheduler.ScheduleJob(ThisJustInJob.CreateJob(), ThisJustInJob.CreateTrigger());
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
